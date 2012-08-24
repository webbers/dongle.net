using Dongle.System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dongle.Tests.System.IO
{
    [TestClass]
    public class ApplicationPathsTest
    {
        [TestMethod]
        public void Test()
        {
            Assert.IsNotNull(ApplicationPaths.RootDirectory);
            Assert.AreEqual(ApplicationPaths.RootDirectory + @"App_Data\", ApplicationPaths.DataDirectory);
        }
    }
}
