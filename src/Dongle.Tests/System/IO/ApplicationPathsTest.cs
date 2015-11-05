using Dongle.System.IO;
using NUnit.Framework;

namespace Dongle.Tests.System.IO
{
    [TestFixture]
    public class ApplicationPathsTest
    {
        [Test]
        public void Test()
        {
            Assert.IsNotNull(ApplicationPaths.RootDirectory);
            Assert.AreEqual(ApplicationPaths.RootDirectory + @"App_Data\", ApplicationPaths.DataDirectory);
        }
    }
}
