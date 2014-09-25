using Dongle.Utils;
using NUnit.Framework;

namespace Dongle.Tests.Utils
{
    [TestFixture]
    public class UrlUtilsTest
    {
        [Test]
        public void IsIp()
        {
            //IPV4
            Assert.IsTrue(UrlUtils.IsIp("192.169.1.1"));
            Assert.IsFalse(UrlUtils.IsIp("256.1.3.4"));
            Assert.IsFalse(UrlUtils.IsIp("023.44.33.22"));
            Assert.IsFalse(UrlUtils.IsIp("10.57.98.23."));
            Assert.IsFalse(UrlUtils.IsIp("1234"));

            //IPV6
            Assert.IsTrue(UrlUtils.IsIp("::1"));
            Assert.IsTrue(UrlUtils.IsIp("fe80::20c:29ff:fe09:ebc8"));
            Assert.IsTrue(UrlUtils.IsIp("805B:2D9D:DC28:0000:0000:0000:D4C8:1FFF"));
            Assert.IsTrue(UrlUtils.IsIp("805B:2D9D:DC28:0:0:0:D4C8:1FFF"));
            Assert.IsTrue(UrlUtils.IsIp("FF00:4502:0:0:0:0:0:42"));
            Assert.IsTrue(UrlUtils.IsIp("FF00:4502::42"));
            Assert.IsFalse(UrlUtils.IsIp("805B::DC28::D4C8:1FFF"));
        }
    }
}
