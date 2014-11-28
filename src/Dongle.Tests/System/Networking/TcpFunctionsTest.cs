using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Dongle.System.IO;
using Dongle.System.Networking.Tcp;
using NUnit.Framework;

namespace Dongle.Tests.System.Networking
{
    public class TcpFunctionsTest
    {
        public List<LogItem> Log = new List<LogItem>();
        public TcpClient TcpClient;
        public TcpServer TcpServer;

        public void StartClient()
        {
            TcpClient = new TcpClient();
            TcpClient.OnCommandSent += (command, bytesSent) => AddLogLine("COMMAND SENT " + command, "Client");
            TcpClient.OnConnect += success => AddLogLine("CONNECTED " + success, "Client");
            TcpClient.OnFileSent += () =>
            {
                AddLogLine("FILE SENT", "Client");
                TcpClient.WaitForCommand();
            };
            TcpClient.OnCommandReceived += command => AddLogLine("COMMAND RECEIVED " + command, "Client");
            TcpClient.OnFileReceived += fileData =>
            {
                File.WriteAllBytes(ApplicationPaths.CurrentPath + @"Client.txt", fileData);
                AddLogLine("FILE RECEIVED", "Client");
                TcpClient.SendCommand("MESSAGE:FILE RECEIVED IN CLIENT");
            };
            TcpClient.Connect(IPAddress.IPv6Loopback, 56000);
        }

        public void StartServer()
        {
            TcpServer = new TcpServer();
            TcpServer.OnStart += () => AddLogLine("SERVER STARTED", "Server");
            TcpServer.OnCommandReceived += (connectionId, command) =>
            {
                AddLogLine("COMMAND RECEIVED " + command, "Server");
                if (command.StartsWith("HELLO"))
                {
                    TcpServer.SendCommand(connectionId, "HELLO CLIENT");
                    TcpServer.WaitForCommand(connectionId);
                }
                else if (command.StartsWith("FILE"))
                {
                    TcpServer.WaitForFile(connectionId, int.Parse(command.Split(':')[1]));
                }
                else if (command.StartsWith("GETFILE"))
                {
                    TcpServer.SendFile(connectionId, new byte[] { 72, 69, 76, 76, 79, 32, 83, 62, 67, 72, 69, 76, 76, 79, 32, 83, 62, 67, 72, 69, 76, 76, 79, 32, 83, 62, 67 });
                    AddLogLine("FILE SEND STARTED", "Server");
                }
                else if (command.StartsWith("QUIT"))
                {
                    TcpServer.CloseConnection(connectionId);
                    AddLogLine("CONNECTION CLOSED", "Server");
                }
                else
                {
                    TcpServer.WaitForCommand(connectionId);
                }
            };
            TcpServer.OnCommandSent += (command, bytesSent) => AddLogLine("COMMAND SENT " + command, "Server");
            TcpServer.OnClientDisconnected += (connectionId, endPoint) => AddLogLine("DISCONNECTED OF CLIENT", "Server");
            TcpServer.OnFileReceived += (connectionId, fileData) =>
            {
                File.WriteAllBytes(ApplicationPaths.CurrentPath + @"Server.txt", fileData);
                AddLogLine("FILE RECEIVED", "Server");
                TcpServer.SendCommand(connectionId, "MESSAGE:FILE RECEIVED IN SERVER");
                TcpServer.WaitForCommand(connectionId);
            };

            TcpServer.OnFileSent += (connectionId, fileData) =>
            {
                AddLogLine("FILE SENT", "Server");
                TcpServer.WaitForCommand(connectionId);
            };

            TcpServer.OnClientConnected += connectionId => AddLogLine("CONNECTED WITH CLIENT", "Server");

            TcpServer.Start();
        }

        protected void AssertIsTrue(Func<bool> func, int maxIteractions = 10)
        {
            for (var i = 0; i < maxIteractions; i++)
            {
                if (func())
                {
                    return;
                }
                Thread.CurrentThread.Join(100);
            }
            Assert.Fail("Condição deveria retornar true.");
        }

        public void AssertLastLog(string source, string message, int maxIteractions = 10)
        {
            var lastLog = "";
            message = message.ToLower().Trim();
            for (var i = 0; i < maxIteractions; i++)
            {
                lock (Log)
                {
                    lastLog = Log.Count() != 0 ? Log.Last(l => l.Source == source).LogMessage.ToLower().Trim() : "";
                }
                if (lastLog == message)
                {
                    return;
                }
                Thread.CurrentThread.Join(100);
            }
            Assert.Fail("Log esperado - " + source + ": " + message + "\r\n" + "Log recebido - " + lastLog);
        }

        public void AssertAnyOf(string source, string message, int count = 20, int maxIteractions = 1000)
        {
            message = message.ToLower().Trim();
            for (var i = 0; i < maxIteractions; i++)
            {
                List<LogItem> logs;
                lock (Log)
                {
                    logs = Log.Where(l => l.Source == source).ToList();
                }
                while (logs.Count() != 0 && logs.Count() > count)
                {
                    logs.Remove(logs.First());
                }
                if (logs.Any(l => l.LogMessage.ToLower().Trim() == message))
                {
                    return;
                }
                Thread.CurrentThread.Join(10);
            }
            Assert.Fail("Log não encontrado entre os " + count + " últimos: " + message);
        }

        public void AddLogLine(string message, string source)
        {
            lock (Log)
            {
                Log.Add(new LogItem(source, message));
            }      
        }

        public class LogItem
        {
            public string Source { get; set; }
            public string LogMessage { get; set; }

            public LogItem(string source, string logMessage)
            {
                Source = source;
                LogMessage = logMessage;
            }
        }
    }
}
