﻿using System;
using System.Globalization;
using System.IO;
using Dongle.Serialization;
using Dongle.System.IO;
using Dongle.Tests.Tools;
using NUnit.Framework;
using Ploeh.SemanticComparison;

namespace Dongle.Tests.Serialization
{
    [TestFixture]
    public class JsonSimpleSerializerTest
    {

        [Test]
        public void SerializeToFile()
        {
            AssertFileSerialize("12345", "\"12345\"");
            var objectToSerialize = new Foo
                                    {
                                        Name = "Wine",
                                        Age = 10,
                                        CreatedAt = new DateTime(2011, 8, 15, 12, 30, 15),
                                        Price = 1.25
                                    };
            AssertFileSerialize(objectToSerialize, "{\"Name\":\"Wine\",\"Age\":10,\"CreatedAt\":\"\\/Date(1313422215000-0300)\\/\",\"Price\":1.25,\"Enabled\":false,\"Parent\":null}");

            //Com parent
            var parent = new Foo { Name = "Suco de Uva" };
            objectToSerialize = new Foo
                                    {
                                        Name = "Wine",
                                        Age = 10,
                                        CreatedAt = new DateTime(2011, 8, 15, 12, 30, 15),
                                        Price = 1.25,
                                        Parent = parent
                                    };
            AssertFileSerialize(objectToSerialize, "{\"Name\":\"Wine\",\"Age\":10,\"CreatedAt\":\"\\/Date(1313422215000-0300)\\/\",\"Price\":1.25,\"Enabled\":false,\"Parent\":{\"Name\":\"Suco de Uva\",\"Age\":0,\"CreatedAt\":\"\\/Date(-62135596800000)\\/\",\"Price\":0.0,\"Enabled\":false,\"Parent\":null}}", false);
        }

        [Test]
        public void UnserializeNull()
        {
            Assert.IsNull(JsonSimpleSerializer.UnserializeFromFile<object>(new FileInfo("arquivo inexistente")));
        }

        private static void AssertFileSerialize<T>(T objToSerialize, string serializedString, bool assertUnserialize = true) where T : class
        {
            var directory = new DirectoryInfo(ApplicationPaths.RootDirectory);
            var file = new FileInfo(directory.FullName + "\\test.json");
            JsonSimpleSerializer.SerializeToFile(file, objToSerialize);
            var fileContent = file.GetContent();
            Assert.AreEqual(serializedString, fileContent);

            if (assertUnserialize)
            {
                var unserialized = JsonSimpleSerializer.UnserializeFromFile<T>(file);
                var likeness = new Likeness<T, T>(objToSerialize);
                Assert.AreEqual(likeness, unserialized);
            }
        }

        [Test]
        public void UnserializeFromStringTest()
        {
            const string fooString = @"{""Name"":""Wine"",""Age"":10,""CreatedAt"":""03/04/2001"",""Price"":1.25,""Enabled"":false,""Parent"":null}";

            var fooDeserializedPtBr = JsonSimpleSerializer.UnserializeFromString<Foo>(fooString, new CultureInfo("pt-BR"));
            var fooDeserializedEnUs = JsonSimpleSerializer.UnserializeFromString<Foo>(fooString, new CultureInfo("en-US"));

            Assert.AreEqual(new DateTime(2001, 04, 03), fooDeserializedPtBr.CreatedAt);
            Assert.AreEqual(new DateTime(2001, 03, 04), fooDeserializedEnUs.CreatedAt);
        }

        [Test]
        public void UnserializeFromStringToObjectTest()
        {
            const string fooString = @"{""Name"":""Wine"",""Age"":10,""CreatedAt"":""03/04/2001"",""Price"":1.25,""Enabled"":false,""Parent"":null}";

            var fooDeserializedPtBr = JsonSimpleSerializer.UnserializeObject(fooString, typeof(Foo), new CultureInfo("pt-BR"));
            var fooDeserializedEnUs = JsonSimpleSerializer.UnserializeObject(fooString, typeof(Foo), new CultureInfo("en-US"));
            Assert.AreEqual(typeof(Foo), fooDeserializedPtBr.GetType());
            Assert.AreEqual(new DateTime(2001, 04, 03), ((Foo)fooDeserializedPtBr).CreatedAt);
            Assert.AreEqual(new DateTime(2001, 03, 04), ((Foo)fooDeserializedEnUs).CreatedAt);
        }

        [Test]
        public void GetNodeValueFromJsonTest()
        {
            const string fooString = @"{""Name"":""Wine"",""Age"":10,""CreatedAt"":""03/04/2001"",""Price"":1.25,""Enabled"":false,""Parent"":null}";
            var value = JsonSimpleSerializer.GetNodeValueFromJson(fooString, "Name");
            Assert.AreEqual("Wine", value);
        }


        [Test]
        public void GetInvalidNodeValueFromJsonTest()
        {
            const string fooString = @"{""Name"":""Wine"",""Age"":10,""CreatedAt"":""03/04/2001"",""Price"":1.25,""Enabled"":false,""Parent"":null}";
            const string invalidPropertyName = "InvalidPropertyName";

            var value = JsonSimpleSerializer.GetNodeValueFromJson(fooString, invalidPropertyName);
            
            Assert.IsNull(value);
        }
    }
}
