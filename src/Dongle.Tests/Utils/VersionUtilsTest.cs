using System;
using Dongle.Utils;
using NUnit.Framework;

namespace Dongle.Tests.Utils
{
    [TestFixture]
    public class VersionUtilsTest
    {
        [Test]
        public void CompareWithPreviousVersionSplit()
        {
            Assert.IsTrue("2.6.0.6000.0.0.1.256".CompareWithPreviousVersionSplit("2.5.1.6700.0.0.1.256"));
            Assert.IsFalse("2.5.1.6700.0.0.1.256".CompareWithPreviousVersionSplit("2.6.0.6000.0.0.1.256"));
            Assert.IsTrue("2.5.1.6700.0.0.1.256".CompareWithPreviousVersionSplit("2.5.1.6700.0.0.1.256"));
            Assert.IsTrue("".CompareWithPreviousVersionSplit("2.5.1.6700.0.0.1.256"));
            Assert.IsTrue("2.5.1.6700.0.0.1.256".CompareWithPreviousVersionSplit(""));
            Assert.IsTrue("2.5.1".CompareWithPreviousVersionSplit("1.3.A"));
            Assert.IsFalse("1.33.A".CompareWithPreviousVersionSplit("1.48.B"));
            Assert.IsTrue("xpto".CompareWithPreviousVersionSplit("1.0"));
            Assert.IsTrue("1.0".CompareWithPreviousVersionSplit("xpto"));
        }

        [Test]
        public void CompareWithPreviousVersion()
        {
            Assert.IsTrue("6.0.4.5".CompareWithPreviousVersion("5.1.5.1") <= 0);
            Assert.IsFalse("5.1.67.0".CompareWithPreviousVersion("6.0.6000") <= 0);
            Assert.IsTrue("5.1.67.0".CompareWithPreviousVersion("5.1.67.0") <= 0);
            Assert.IsTrue("".CompareWithPreviousVersion("5.1.67.0") <= 0);
            Assert.IsTrue("5.1.67.0".CompareWithPreviousVersion("") <= 0);
            Assert.IsTrue("2.5.1".CompareWithPreviousVersion("1.3.A") <= 0);
        }

        [Test]
        public void CompareVersionMethods()
        {
            var start = DateTime.Now;
            for (var i = 0; i < 500000; i++)
            {
                "6.0.4.5".CompareWithPreviousVersion("5.1.5.1");
                "5.1.67.0".CompareWithPreviousVersion("6.0.6000");
                "5.1.67.0".CompareWithPreviousVersion("5.1.67.0");
                "".CompareWithPreviousVersion("5.1.67.0");
                "5.1.67.0".CompareWithPreviousVersion("");
            }
            Console.WriteLine("CompareWithPreviousVersion: " + (DateTime.Now - start).TotalMilliseconds);

            start = DateTime.Now;
            for (var i = 0; i < 500000; i++)
            {
                "6.0.4.5".CompareWithPreviousVersionSplit("5.1.5.1");
                "5.1.67.0".CompareWithPreviousVersionSplit("6.0.6000");
                "5.1.67.0".CompareWithPreviousVersionSplit("5.1.67.0");
                "".CompareWithPreviousVersionSplit("5.1.67.0");
                "5.1.67.0".CompareWithPreviousVersionSplit("");
            }
            Console.WriteLine("CompareWithPreviousVersionSplit: " + (DateTime.Now - start).TotalMilliseconds);
        }
    }
}
