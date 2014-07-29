using System.Globalization;
using Dongle.Serialization;
using Dongle.System.IO;
using Dongle.Tests.Tools;

using NUnit.Framework;

namespace Dongle.Tests.Serialization
{
    [TestFixture]
    /*[DeploymentItem(@"pt-BR\Dongle.resources.dll", "pt-BR")]
    [DeploymentItem(@"es-ES\Dongle.resources.dll", "es-ES")]*/
    public class FixedWidthTextSerializerTest
    {
        private const string Expected =
            "-----------------------------------------------------------------------------------------------\r\n"
            + "Name                           Age        CreatedAt            Price      Enabled              \r\n"
            + "-----------------------------------------------------------------------------------------------\r\n"
            + "Silvio Santos                  82         1930-12-12           1.72       TRUE                 \r\n"
            + "Hebe Camargo                   83         1929-03-08 12:00:00  1.6        FALSE                ";

        private const string ExpectedWithResource =
            "------------------------------------------------------------------------------------------------------\r\n"
            + "Name From Resource             Age From Resource CreatedAt            Price      Enabled              \r\n"
            + "------------------------------------------------------------------------------------------------------\r\n"
            + "Silvio Santos                  82                1930-12-12           1,72       VERDADEIRO           \r\n"
            + "Hebe Camargo                   83                1929-03-08 12:00:00  1,6        FALSO                ";

        [Test]
        public void FixedWidthSerializeFooList()
        {
            var serializer = new FixedWidthTextSerializer<Foo>();
            var actual = DongleEncoding.Default.GetString(serializer.Serialize(Foo.FooArray));
            Assert.AreEqual(Expected, actual);
        }

        [Test]
        public void FixedWidthSerializeDicionary()
        {
            //Todo: Make work with dictionaries
            /*var serializer = new FixedWidthTextSerializer<Dictionary<string, object>>();
            var actual = DongleEncoding.Default.GetString(serializer.Serialize(Foo.FooDictionary));
            Assert.AreEqual(Expected, actual);*/
        }

        [Test]
        public void FixedWidthSerializeFooListWithResourceAndCulture()
        {
            var serializer = new FixedWidthTextSerializer<Foo>(FooResource.ResourceManager);
            var actual = DongleEncoding.Default.GetString(serializer.Serialize(Foo.FooArray, CultureInfo.GetCultureInfo("pt-BR")));
            Assert.AreEqual(ExpectedWithResource, actual);
        }
    }
}