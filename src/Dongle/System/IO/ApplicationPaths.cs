using System.IO;
using System.Reflection;

namespace Dongle.System.IO
{
    public static class ApplicationPaths
    {
        public static string RootDirectory
        {
            get
            {
                var uri = Path.GetDirectoryName(Assembly.GetAssembly(typeof(ApplicationPaths)).CodeBase);
                return uri.Replace(@"file:\", "") + @"\";
            }
        }

        public static string DataDirectory
        {
            get { return RootDirectory + @"App_Data\"; }
        }
    }
}
