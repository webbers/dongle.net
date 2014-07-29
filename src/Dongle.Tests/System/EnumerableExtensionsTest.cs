using System.Linq;
using Dongle.System;
using NUnit.Framework;

namespace Dongle.Tests.System
{
    [TestFixture]
    public class EnumerableExtensionsTest
    {
        [Test]
        public void TestDistribute()
        {
            var values = new [] {10d, 70d, 1d}.Distribute(100);
            Assert.AreEqual(100, values.Sum());

            values = new[] { 1d, 1d, 1d }.Distribute(100);
            Assert.AreEqual(100, values.Sum());

            values = new[] { 10d }.Distribute(100);
            Assert.AreEqual(100, values.Sum());

            values = new[] { 100d, 200d }.Distribute(100);
            Assert.AreEqual(100, values.Sum());
        }
    }
}
