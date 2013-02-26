using System.Globalization;
using System.Text;

using Dongle.Serialization;
using Dongle.Tests.Tools;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dongle.Tests.Serialization
{
    [TestClass]
    [DeploymentItem(@"pt-BR\Dongle.resources.dll", "pt-BR")]
    [DeploymentItem(@"es-ES\Dongle.resources.dll", "es-ES")]
    public class FixedWidthTextSerializerTest
    {
        private const string Expected =
            "-----------------------------------------------------------------------------------------------\r\n"
            + "Name                           Age        CreatedAt            Price      Enabled              \r\n"
            + "-----------------------------------------------------------------------------------------------\r\n"
            + "Silvio Santos                  82         1930-12-12           1.72       TRUE                 \r\n"
            + "Hebe Camargo                   83         1929-03-08 12:00:00  1.6        FALSE                ";

        private const string ExpectedWithResource =
            "-----------------------------------------------------------------------------------------------\r\n"
            + "Name From Resource             Age From R CreatedAt            Price      Enabled              \r\n"
            + "-----------------------------------------------------------------------------------------------\r\n"
            + "Silvio Santos                  82         1930-12-12           1,72       VERDADEIRO           \r\n"
            + "Hebe Camargo                   83         1929-03-08 12:00:00  1,6        FALSO                ";

        [TestMethod]
        public void FixedWidthSerializeFooList()
        {
            var serializer = new FixedWidthTextSerializer<Foo>();
            var actual = Encoding.UTF8.GetString(serializer.Serialize(Foo.FooArray));
            Assert.AreEqual(Expected, actual);
        }

        [TestMethod]
        public void FixedWidthSerializeDicionary()
        {
            //Todo: Make work with dictionaries
            /*var serializer = new FixedWidthTextSerializer<Dictionary<string, object>>();
            var actual = Encoding.UTF8.GetString(serializer.Serialize(Foo.FooDictionary));
            Assert.AreEqual(Expected, actual);*/
        }

        [TestMethod]
        public void FixedWidthSerializeFooListWithResourceAndCulture()
        {
            var serializer = new FixedWidthTextSerializer<Foo>(FooResource.ResourceManager);
            var actual = Encoding.UTF8.GetString(serializer.Serialize(Foo.FooArray, CultureInfo.GetCultureInfo("pt-BR")));
            Assert.AreEqual(ExpectedWithResource, actual);
        }
    }
}