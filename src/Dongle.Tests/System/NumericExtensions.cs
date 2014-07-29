using Dongle.System;
using NUnit.Framework;

namespace Dongle.Tests.System
{
    [TestFixture]
    public class NumericExtensions
    {
        [Test]
        public void TestToHex()
        {
            long input = 1234;
            Assert.AreEqual("000004D2", input.ToHex());

            input = 12345678901234;
            Assert.AreEqual("B3A73CE2FF2", input.ToHex());
        }

        [Test]
        public void TestToFastHex()
        {
            long input = 1234;
            Assert.AreEqual("000004D2", input.ToHexFast());

            input = 12345678901234;
            Assert.AreEqual("B3A73CE2FF2", input.ToHexFast());
        }
        
        [Test]
        public void TestToFastHexFromNegativeDecimal()
        {
            long input = -933489160;
            Assert.AreEqual("C85C15F8", input.ToHexFast());
        }
    }
}
