using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebUtils.System.IO;

namespace WebUtilsTest.System.IO
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
