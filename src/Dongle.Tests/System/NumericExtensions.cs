using Dongle.System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dongle.Tests.System
{
    [TestClass]
    public class NumericExtensions
    {
        [TestMethod]
        public void TestToHex()
        {
            long input = 1234;
            Assert.AreEqual("000004D2", input.ToHex());

            input = 12345678901234;
            Assert.AreEqual("B3A73CE2FF2", input.ToHex());
        }

        [TestMethod]
        public void TestToFastHex()
        {
            long input = 1234;
            Assert.AreEqual("000004D2", input.ToHexFast());

            input = 12345678901234;
            Assert.AreEqual("B3A73CE2FF2", input.ToHexFast());
        }
        
        [TestMethod]
        public void TestToFastHexFromNegativeDecimal()
        {
            long input = -933489160;
            Assert.AreEqual("C85C15F8", input.ToHexFast());
        }
    }
}
