using System.Linq;
using System.Text;
using Dongle.Web.ModelAttributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dongle.Web.Tests.Mvc.ModelAttributes
{
    [TestClass]
    public class ModelAttributesTest
    {
        [TestMethod]
        public void TestEmailAttribute()
        {
            var attrib = new WEmailAttribute();
            Assert.IsTrue(attrib.IsValid("a@b.com"));
            Assert.IsFalse(attrib.IsValid("silvio santos"));

            var rules = attrib.GetClientValidationRules(null, null);

            Assert.AreEqual("Invalid e-mail", rules.First().ErrorMessage);
        }

        [TestMethod]
        public void TestHexadecimalAttribute()
        {
            var attrib = new WHexadecimalAttribute();
            Assert.IsTrue(attrib.IsValid("ABCDEF11"));
            Assert.IsFalse(attrib.IsValid("ZCXASD01"));

            var rules = attrib.GetClientValidationRules(null, null);

            Assert.AreEqual("Invalid hexadecimal", rules.First().ErrorMessage);
        }

        [TestMethod]
        public void TestDomain()
        {
            var attrib = new WDomainAttribute();
            Assert.IsTrue(attrib.IsValid("abc.com.br"));
            Assert.IsTrue(attrib.IsValid("google.com"));
            Assert.IsTrue(attrib.IsValid("google.co.ke"));
            Assert.IsFalse(attrib.IsValid("silvio"));
            Assert.IsFalse(attrib.IsValid("silvio santos"));

            var rules = attrib.GetClientValidationRules(null, null);

            Assert.AreEqual("Invalid domain", rules.First().ErrorMessage);
        }

        [TestMethod]
        public void TestUrl()
        {
            var attrib = new WUrlAttribute();
            Assert.IsTrue(attrib.IsValid("http://ab.com"));
            Assert.IsFalse(attrib.IsValid("silvio santos"));

            var rules = attrib.GetClientValidationRules(null, null);

            Assert.AreEqual("Invalid URL", rules.First().ErrorMessage);
        }

        [TestMethod]
        public void TestIpV4()
        {
            var attrib = new WIpV4Attribute();
            Assert.IsTrue(attrib.IsValid("192.169.1.1"));
            Assert.IsFalse(attrib.IsValid("256.1.3.4"));
            Assert.IsFalse(attrib.IsValid("023.44.33.22"));
            Assert.IsFalse(attrib.IsValid("10.57.98.23."));

            var rules = attrib.GetClientValidationRules(null, null);

            Assert.AreEqual("Invalid IP address", rules.First().ErrorMessage);
        }

        [TestMethod]
        public void TestStringLength()
        {
            const int maxLenght = 3;
            var attrib = new WStringLength(maxLenght);
            var builder = new StringBuilder();
            for (var i = 0; i < maxLenght; i++)
            {
                builder.Append("a");
            }
            Assert.IsTrue(attrib.IsValid(builder.ToString()));
            Assert.IsFalse(attrib.IsValid(builder + "a"));
        }
    }
}