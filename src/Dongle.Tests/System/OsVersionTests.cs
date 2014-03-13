using System;
using Dongle.System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dongle.Tests.System
{
    [TestClass]
    public class OsVersionTests
    {
        [TestMethod]
        public void TestOsVersionShortName()
        {
            var version = OsVersion.GetFromVersion("2.6.2.8250.0.0.1.256");
            Assert.AreEqual("WIN8", version.ShortName);
            
            version = OsVersion.GetFromVersion("A3.2.1");
            Assert.AreEqual("ADR", version.ShortName);

            version = OsVersion.GetFromVersion("I5.0");
            Assert.AreEqual("IOS", version.ShortName);
            
            version = OsVersion.GetFromVersion("M5.0");
            Assert.AreEqual("MAC", version.ShortName);

            version = OsVersion.GetFromVersion("L6.0.52146");
            Assert.AreEqual("LINUX", version.ShortName);
            
            version = OsVersion.GetFromVersion("2.6.1.8250.0.0.1.256");
            Assert.AreEqual("WIN7", version.ShortName);

            version = OsVersion.GetFromVersion("2.6.1.7601.1.0.2.18");
            Assert.AreEqual("2008R2", version.ShortName);

            version = OsVersion.GetFromVersion("C4920.82.0");
            Assert.AreEqual("CROS", version.ShortName);
        }


        [TestMethod]
        public void TestOsVersionName()
        {
            var version = OsVersion.GetFromVersion("2.6.2.8250.0.0.1.256");
            Assert.AreEqual("Windows 8 Ultimate", version.Name);
            
            version = OsVersion.GetFromVersion("2.5.0.1111.0.0.1.128");
            Assert.AreEqual("Windows 2000 Professional", version.Name);
            
            version = OsVersion.GetFromVersion("A3.2.1");
            Assert.AreEqual("Android", version.Name);

            version = OsVersion.GetFromVersion("I5.0");
            Assert.AreEqual("Ios", version.Name);

            version = OsVersion.GetFromVersion("M5.0");
            Assert.AreEqual("Mac OS", version.Name);

            version = OsVersion.GetFromVersion("L6.0.52146");
            Assert.AreEqual("Linux", version.Name);

            version = OsVersion.GetFromVersion("2.6.1.8250.0.0.1.256");
            Assert.AreEqual("Windows 7 Ultimate", version.Name);

            version = OsVersion.GetFromVersion("2.6.1.7601.1.0.2.18");
            Assert.AreEqual("Windows Server 2008 R2 Home", version.Name);
            Assert.AreEqual("2.6.1.7601.1.0.2.18", version.Version);            
        }
    }
}
