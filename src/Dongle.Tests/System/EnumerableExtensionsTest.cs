using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Dongle.System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dongle.Tests.System
{
    [TestClass]
    public class EnumerableExtensionsTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var values = new [] {10d, 70d, 1d}.Distribute(100);
            Assert.AreEqual(100, values.Sum());

            values = new[] { 1d, 1d, 1d }.Distribute(100);
            Assert.AreEqual(100, values.Sum());

            values = new[] { 10d }.Distribute(100);
            Assert.AreEqual(100, values.Sum());

            values = new[] { 100d, 200d }.Distribute(100);
            Assert.AreEqual(100, values.Sum());

            values = new[] { 0d, 0d }.Distribute(100);
            Assert.AreEqual(100, values.Sum());
        }
    }
}
