using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.SemanticComparison;
using WebUtils.Serialization;
using WebUtils.System.IO;
using WebUtilsTest.Tools;

namespace WebUtilsTest.Serialization
{
    [TestClass]
    public class JsonSimpleSerializerTest
    {

        [TestMethod]
        public void SerializeToFile()
        {
            AssertFileSerialize("12345", "\"12345\"");
            AssertFileSerialize(new Foo
            {
                Name = "Wine",
                Age = 10,
                CreatedAt = new DateTime(2011, 8, 15, 12, 30, 15),
                Price = 1.25
            }, "{\"Age\":10,\"CreatedAt\":\"\\/Date(1313422215000-0300)\\/\",\"Enabled\":false,\"Name\":\"Wine\",\"Parent\":null,\"Price\":1.25}");

            //Com parent
            AssertFileSerialize(new Foo
            {
                Name = "Wine",
                Age = 10,
                CreatedAt = new DateTime(2011, 8, 15, 12, 30, 15),
                Price = 1.25,
                Parent = new Foo{ Name = "Suco de Uva"}
            }, "{\"Age\":10,\"CreatedAt\":\"\\/Date(1313422215000-0300)\\/\",\"Enabled\":false,\"Name\":\"Wine\",\"Parent\"" +
               ":{\"Age\":0,\"CreatedAt\":\"\\/Date(-62135589600000-0200)\\/\",\"Enabled\":false,\"Name\":\"Suco de Uva\"," +
               "\"Parent\":null,\"Price\":0},\"Price\":1.25}", false);
        }

        [TestMethod]
        public void UnserializeNull()
        {
            Assert.IsNull(JsonSimpleSerializer.UnserializeFromFile<object>(new FileInfo("arquivo inexistente")));
        }

        private static void AssertFileSerialize<T>(T objToSerialize, string serializedString, bool assertUnserialize = true) where T: class
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
    }
}
