using System;
using System.Collections.Generic;
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
    public class CsvSerializerTest
    {
        private const string CsvExpected =
            "Name,Age,CreatedAt,Price,Enabled\r\n" + "Silvio Santos,82,1930-12-12,1.72,TRUE\r\n"
            + "Hebe Camargo,83,1929-03-08 12:00:00,1.6,FALSE\r\n";

        private const string CsvExpectedWithResource =
            "Name From Resource,Age From Resource,CreatedAt,Price,Enabled\r\n"
            + "Silvio Santos,82,1930-12-12,\"1,72\",VERDADEIRO\r\n"
            + "Hebe Camargo,83,1929-03-08 12:00:00,\"1,6\",FALSO\r\n";

        [TestMethod]
        public void SerializeFooList()
        {
            var serializer = new CsvSerializer<Foo>();
            var actual = Encoding.UTF8.GetString(serializer.Serialize(Foo.FooArray));
            Assert.AreEqual(CsvExpected, actual);
        }

        [TestMethod]
        public void SerializeDicionary()
        {
            var serializer = new CsvSerializer<Dictionary<string, object>>();
            var actual = Encoding.UTF8.GetString(serializer.Serialize(Foo.FooDictionary));
            Assert.AreEqual(CsvExpected, actual);
        }

        [TestMethod]
        public void SerializeFooListWithResourceAndCulture()
        {
            var serializer = new CsvSerializer<Foo>(FooResource.ResourceManager);
            var actual = Encoding.UTF8.GetString(serializer.Serialize(Foo.FooArray, CultureInfo.GetCultureInfo("pt-BR")));
            Assert.AreEqual(CsvExpectedWithResource, actual);
        }
    }
}