using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using Microsoft.Win32;

namespace Dongle.Configuration
{
    public static class WindowsServices
    {
        public static void InstallService(string serviceName, string displayName, string filePath)
        {
            ExecuteCommand(Environment.SystemDirectory + @"\sc.exe", "create \"" + serviceName + "\" binPath= \"" + filePath + "\" start= auto DisplayName= \"" + displayName + "\"");
            SetServiceAutoStart(serviceName);
            SetServiceFailureAction(serviceName);
            StartService(serviceName);
        }

        public static void ReinstallService(string serviceName, string displayName, string filePath)
        {
            StopService(serviceName);

            var i = 0;

            while (GetWindowsServiceStatus(serviceName) != ServiceControllerStatus.Stopped && i < 30)
            {
                i++;
                Thread.CurrentThread.Join(1000);
            }

            ExecuteCommand(Environment.SystemDirectory + @"\sc.exe", "config \"" + serviceName + "\" binPath= \"" + filePath + "\" start= auto DisplayName= \"" + displayName + "\"");
            SetServiceAutoStart(serviceName);
            SetServiceFailureAction(serviceName);
            StartService(serviceName);
        }

        public static void RemoveService(string serviceName)
        {
            StopService(serviceName);
            ExecuteCommand(Environment.SystemDirectory + @"\sc.exe", "delete \"" + serviceName + "\"");
        }

        public static void SetServiceAutoStart(string serviceName)
        {
            ExecuteCommand(Environment.SystemDirectory + @"\sc.exe", "config \"" + serviceName + "\" start= auto");
        }

        public static void SetServiceFailureAction(string serviceName)
        {
            ExecuteCommand(Environment.SystemDirectory + @"\sc.exe", "failure \"" + serviceName + "\" reset= 300 actions= restart/500");
        }

        public static void StartService(string serviceName)
        {
            ExecuteCommand(Environment.SystemDirectory + @"\sc.exe", "start \"" + serviceName + "\"");
        }

        public static void StopService(string serviceName)
        {
            ExecuteCommand(Environment.SystemDirectory + @"\sc.exe", "stop \"" + serviceName + "\"");
        }

        public static bool StopWindowsService(string serviceFullName, long timeout = 10)
        {
            if (!WindowsServiceExists(serviceFullName))
            {
                return true;
            }
            var trys = 0;
            bool stopped;
            do
            {
                StopService(serviceFullName);

                var status = GetWindowsServiceStatus(serviceFullName);

                if (status == null)
                {
                    return false;
                }

                stopped = status == ServiceControllerStatus.Stopped;

                if (stopped)
                {
                    break;
                }
                trys++;
                Thread.CurrentThread.Join(1000);
            }
            while (trys < timeout);
            return stopped;
        }

        public static void RestartWindowsService(string serviceFullName, long timeout = 10)
        {
            var stopped = StopWindowsService(serviceFullName, timeout);
            if (stopped)
            {
                StartService(serviceFullName);
            }
        }

        public static bool WindowsServiceExists(string serviceName)
        {
            return Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\" + serviceName) != null;
        }

        public static ServiceControllerStatus? GetWindowsServiceStatus(string serviceName)
        {
            var service = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == serviceName);
            if (service != null)
            {
                return service.Status;
            }
            return null;
        }

        public static string GetServiceImagePath(string serviceName, string machineName)
        {
            var registryPath = @"SYSTEM\CurrentControlSet\Services\" + serviceName;
            var keyHklm = Registry.LocalMachine;

            var key = !string.IsNullOrEmpty(machineName) ? RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, machineName).OpenSubKey(registryPath) : keyHklm.OpenSubKey(registryPath);
            if (key == null)
            {
                return "";
            }
            var value = key.GetValue("ImagePath").ToString();
            key.Close();

            if (string.IsNullOrEmpty(machineName))
            {
                return Environment.ExpandEnvironmentVariables(value).Replace("\"", "");
            }
            key = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, machineName).OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\");
            if (key == null)
            {
                return "";
            }
            var expandedSystemRoot = key.GetValue("SystemRoot").ToString();
            key.Close();
            value = value.Replace("%SystemRoot%", expandedSystemRoot).Replace("\"", "");
            return value;
        }

        /// <summary>
        /// Executa um comando
        /// </summary>
        public static void ExecuteCommand(string command, string parameters)
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo(command, parameters)
                    {
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                process.WaitForExit();
            }
            catch (ThreadAbortException)
            {
#if DEBUG
                Debug.WriteLine("Tread abortada. Nada a fazer.");
#endif
            }
        }
    }
}
