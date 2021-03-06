﻿using Dongle.System;
using NUnit.Framework;

namespace Dongle.Tests.System
{
    [TestFixture]
    public class OsVersionTests
    {
        [Test]
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

            version = OsVersion.GetFromVersion("W6.1");
            Assert.AreEqual("WINPHONE", version.ShortName);
        }


        [Test]
        public void TestOsVersionName()
        {
            var version = OsVersion.GetFromVersion("2.6.2.8250.0.0.1.256");
            Assert.AreEqual("Windows 8", version.Name);
            
            version = OsVersion.GetFromVersion("2.5.0.1111.0.0.1.128");
            Assert.AreEqual("Windows 2000", version.Name);
            
            version = OsVersion.GetFromVersion("A3.2.1");
            Assert.AreEqual("Android", version.Name);

            version = OsVersion.GetFromVersion("I5.0");
            Assert.AreEqual("Ios", version.Name);

            version = OsVersion.GetFromVersion("M5.0");
            Assert.AreEqual("Mac OS", version.Name);

            version = OsVersion.GetFromVersion("L6.0.52146");
            Assert.AreEqual("Linux", version.Name);

            version = OsVersion.GetFromVersion("2.6.1.8250.0.0.1.256");
            Assert.AreEqual("Windows 7", version.Name);

            version = OsVersion.GetFromVersion("2.5.1.2600.3.0.1.256");
            Assert.AreEqual("Windows XP SP3 Professional", version.Name);

            version = OsVersion.GetFromVersion("2.5.1.2600.3.0.1.768");
            Assert.AreEqual("Windows XP SP3 Home Edition", version.Name);

            version = OsVersion.GetFromVersion("2.6.1.7601.1.0.2.18");
            Assert.AreEqual("Windows Server 2008 R2", version.Name);
            Assert.AreEqual("2.6.1.7601.1.0.2.18", version.Version);
        }
    }
}
