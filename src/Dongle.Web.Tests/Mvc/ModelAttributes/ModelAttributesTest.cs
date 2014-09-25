using System;
using System.Linq;
using Dongle.Web.ModelAttributes;
using NUnit.Framework;

namespace Dongle.Web.Tests.Mvc.ModelAttributes
{
    [TestFixture]
    public class ModelAttributesTest
    {
        [Test]
        public void TestEmailAttribute()
        {
            var attrib = new WEmailAttribute();
            Assert.IsTrue(attrib.IsValid("a@b.com"));
            Assert.IsFalse(attrib.IsValid("silvio santos"));

            var rules = attrib.GetClientValidationRules(null, null);

            Assert.IsTrue(rules.First().ErrorMessage == "Invalid e-mail" || rules.First().ErrorMessage == "E-mail inválido");
        }

        [Test]
        public void TestHexadecimalAttribute()
        {
            var attrib = new WHexadecimalAttribute(1);
            Assert.IsTrue(attrib.IsValid("ABCDEF11"));
            Assert.IsFalse(attrib.IsValid("ZCXASD01"));

            var attrib2 = new WHexadecimalAttribute(2);
            Assert.IsTrue(attrib2.IsValid("FFDF896700004712"));
            Assert.IsFalse(attrib2.IsValid("FFDF8X67000Z4712"));
            Assert.IsFalse(attrib2.IsValid("FFDF8367000A47122"));

            var attrib3 = new WHexadecimalAttribute(1, 2);
            Assert.IsTrue(attrib3.IsValid("ABCDEF11"));
            Assert.IsTrue(attrib3.IsValid("FFDF896700004712"));

            var rules = attrib.GetClientValidationRules(null, null);

            Assert.IsTrue(rules.First().ErrorMessage == "The value must be a hexadecimal with 1 octet (s)" ||
                          rules.First().ErrorMessage == "O valor precisa ser um hexadecimal com 1 octeto(s)");
        }

        [Test]
        public void TestDomain()
        {
            var attrib = new WDomainAttribute();
            Assert.IsTrue(attrib.IsValid("abc.com.br"));
            Assert.IsTrue(attrib.IsValid("google.com"));
            Assert.IsTrue(attrib.IsValid("*.google.com"));
            Assert.IsTrue(attrib.IsValid("www*.google.com"));
            Assert.IsTrue(attrib.IsValid("?.google.com"));
            Assert.IsTrue(attrib.IsValid("www?.google.com"));
            Assert.IsTrue(attrib.IsValid("google.co.ke"));
            Assert.IsTrue(attrib.IsValid("silvio"));
            Assert.IsTrue(attrib.IsValid("stage.www2.gastecnologia"));
            Assert.IsFalse(attrib.IsValid("silvio santos"));

            var rules = attrib.GetClientValidationRules(null, null);

            Assert.IsTrue(rules.First().ErrorMessage == "Invalid domain" || rules.First().ErrorMessage == "Domínio inválido");
        }

        [Test]
        public void TestUrl()
        {
            var attrib = new WUrlAttribute();
            Assert.IsTrue(attrib.IsValid("http://ab.com"), "Minimun neh!!! normal url");
            Assert.IsTrue(attrib.IsValid("file:///c:/xxx.exe"), "Can be a file");
            Assert.IsFalse(attrib.IsValid("http://ab.com/ teste.html"), "Can't have whitespace");
            Assert.IsTrue(attrib.IsValid("*://*.dominio.com.br*"), "asterix in any place");

            Assert.IsFalse(attrib.IsValid("**://*.dominio.com.br*"), "Can't have double asteristcs");
            Assert.IsFalse(attrib.IsValid("silvio santos"), "need to be a url powww");
            Assert.IsTrue(attrib.IsValid("http://ab.com/%20teste.html"), "must accept %");

            var rules = attrib.GetClientValidationRules(null, null);

            Assert.IsTrue(rules.First().ErrorMessage == "Invalid URL" ||
                          rules.First().ErrorMessage == "URL inválida");
        }

        [Test]
        public void TestProxy()
        {
            var attrib = new WProxyAttribute();
            Assert.IsTrue(attrib.IsValid("192.168.0.20"), "Can be a ip");
            Assert.IsTrue(attrib.IsValid("ns1.dominio.com"), "Can have DNS");
            Assert.IsTrue(attrib.IsValid("http://www.dominio.com.br/"), "Can be a URL");
            Assert.IsTrue(attrib.IsValid("*.dominio.com"), "Can have a *");

            Assert.IsFalse(attrib.IsValid("**.dominio.com"), "Can not have * sequentially");
            Assert.IsFalse(attrib.IsValid("?.dominio.com"), "Can not have a ?");

            var rules = attrib.GetClientValidationRules(null, null);

            Assert.IsTrue(rules.First().ErrorMessage == "Invalid proxy" ||
                          rules.First().ErrorMessage == "Proxy inválido");
        }

        [Test]
        public void TestIpV4()
        {
            var attrib = new WIpAttribute();
            Assert.IsTrue(attrib.IsValid("192.169.1.1"));
            Assert.IsFalse(attrib.IsValid("256.1.3.4"));
            Assert.IsFalse(attrib.IsValid("023.44.33.22"));
            Assert.IsFalse(attrib.IsValid("10.57.98.23."));

            var rules = attrib.GetClientValidationRules(null, null);

            Assert.IsTrue(rules.First().ErrorMessage == "Invalid IP address" || rules.First().ErrorMessage == "Endereço IP inválido");
        }

        [Test]
        public void TestIpV4WildCard()
        {
            var attrib = new WIpWildCardAttribute();
            Assert.IsTrue(attrib.IsValid("*.*"));
            Assert.IsTrue(attrib.IsValid("?.*"));
            Assert.IsTrue(attrib.IsValid("192.*"));
            Assert.IsTrue(attrib.IsValid("192.?"));
            Assert.IsTrue(attrib.IsValid("192.168.1.*"));
            Assert.IsFalse(attrib.IsValid("19*.*"));
            Assert.IsFalse(attrib.IsValid("*"));

            var rules = attrib.GetClientValidationRules(null, null);

            Assert.IsTrue(rules.First().ErrorMessage == "Invalid IP address" || rules.First().ErrorMessage == "Endereço IP inválido");
        }

        [Test]
        public void TestIpV6()
        {
            var attrib = new WIpAttribute();
            Assert.IsTrue(attrib.IsValid("::1"));
            Assert.IsTrue(attrib.IsValid("fe80::20c:29ff:fe09:ebc8"));
            Assert.IsTrue(attrib.IsValid("805B:2D9D:DC28:0000:0000:0000:D4C8:1FFF"));
            Assert.IsTrue(attrib.IsValid("805B:2D9D:DC28:0:0:0:D4C8:1FFF"));
            Assert.IsTrue(attrib.IsValid("FF00:4502:0:0:0:0:0:42"));
            Assert.IsTrue(attrib.IsValid("FF00:4502::42"));
            Assert.False(attrib.IsValid("805B::DC28::D4C8:1FFF"));

            var rules = attrib.GetClientValidationRules(null, null);

            Assert.IsTrue(rules.First().ErrorMessage == "Invalid IP address" || rules.First().ErrorMessage == "Endereço IP inválido");
        }

        [Test]
        public void TestIpV6WildCard()
        {
            var attrib = new WIpWildCardAttribute();
            Assert.IsTrue(attrib.IsValid("*:2:3:*:5:6:7:?"));
            Assert.IsTrue(attrib.IsValid("*::"));
            Assert.IsTrue(attrib.IsValid("*:2:?:4:5:6:7::"));
            Assert.IsTrue(attrib.IsValid("1::*"));
            Assert.IsTrue(attrib.IsValid("?:2:?:4:5:?::*"));
            Assert.IsTrue(attrib.IsValid("1::*:8"));
            Assert.IsTrue(attrib.IsValid("1:2:3:4:5::*:8"));
            Assert.IsTrue(attrib.IsValid("*:2:?:4:5::?"));
            Assert.IsTrue(attrib.IsValid("?::*:4:5:6:?:*"));
            Assert.IsTrue(attrib.IsValid("::2:?:4:5:6:7:*"));
            Assert.IsTrue(attrib.IsValid("::*"));
            Assert.IsTrue(attrib.IsValid("fe80::*:?:8%eth0"));
            Assert.IsTrue(attrib.IsValid("fe80::?:8%*"));
            Assert.IsTrue(attrib.IsValid("::255.*.?.255"));
            Assert.IsTrue(attrib.IsValid("::255.*.255.?"));
            Assert.IsTrue(attrib.IsValid("::ffff:0:255.?.255.*"));
            Assert.IsTrue(attrib.IsValid("::?:?:255.?.255.*"));
            Assert.IsTrue(attrib.IsValid("*:?:3:4::*.0.2.33"));
            Assert.IsTrue(attrib.IsValid("64:ff9b::192.0.2.?"));

            var rules = attrib.GetClientValidationRules(null, null);

            Assert.IsTrue(rules.First().ErrorMessage == "Invalid IP address" || rules.First().ErrorMessage == "Endereço IP inválido");
        }

        [Test]
        public void TestDateRange()
        {
            var attrib = new WDateRangeAttribute();
            Assert.IsFalse(attrib.IsValid(DateTime.Now.AddDays(2)));
            Assert.IsTrue(attrib.IsValid(DateTime.Now.AddDays(-2)));
        }

        [Test]
        public void TestStringLength()
        {
            var attrib = new WStringLengthAttribute(8);
            Assert.IsFalse(attrib.IsValid("123456789"));
            Assert.IsTrue(attrib.IsValid("1234"));
        }
    }
}