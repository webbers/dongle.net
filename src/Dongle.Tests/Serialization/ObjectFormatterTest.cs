using System;
using System.Data.SqlTypes;
using System.Globalization;

using Dongle.Serialization;
using Dongle.Tests.Tools;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dongle.Tests.Serialization
{
    [TestClass]
    public class ObjectFormatterTest
    {
        private readonly CultureInfo _ptBr = CultureInfo.GetCultureInfo("pt-BR");
        private readonly CultureInfo _enUs = CultureInfo.GetCultureInfo("en-US");
        private readonly CultureInfo _esEs = CultureInfo.GetCultureInfo("es-ES");

        [TestMethod]
        public void FormatNumber()
        {
            Assert.AreEqual("1.5", ObjectFormatter.Format(1.5, CultureInfo.InvariantCulture));
            Assert.AreEqual("\"1,5\"", ObjectFormatter.Format(1.5, _ptBr));
            Assert.AreEqual("1.5", ObjectFormatter.Format(1.5, _enUs));
            Assert.AreEqual("\"1,5\"", ObjectFormatter.Format(1.5, _esEs));

            Assert.AreEqual("2", ObjectFormatter.Format(2, CultureInfo.InvariantCulture));
            Assert.AreEqual("-2", ObjectFormatter.Format(-2, CultureInfo.InvariantCulture));
        }

        [TestMethod]
        public void FormatDateTime()
        {
            //As datas precisam ser iguais indiferente da Culture para aumentar a compatibilidade com o Excel
            var sample = new DateTime(2010, 5, 14, 1, 2, 3);
            const string Expected = "2010-05-14 01:02:03";

            Assert.AreEqual(Expected, ObjectFormatter.Format(sample, CultureInfo.InvariantCulture));
            Assert.AreEqual(Expected, ObjectFormatter.Format(sample, _ptBr));
            Assert.AreEqual(Expected, ObjectFormatter.Format(sample, _enUs));
            Assert.AreEqual(Expected, ObjectFormatter.Format(sample, _esEs));
        }

        [TestMethod]
        public void FormatDate()
        {
            //As datas precisam ser iguais indiferente da Culture para aumentar a compatibilidade com o Excel
            var sample = new DateTime(2010, 5, 14);
            const string Expected = "2010-05-14";

            Assert.AreEqual(Expected, ObjectFormatter.Format(sample, CultureInfo.InvariantCulture));
            Assert.AreEqual(Expected, ObjectFormatter.Format(sample, _ptBr));
            Assert.AreEqual(Expected, ObjectFormatter.Format(sample, _enUs));
            Assert.AreEqual(Expected, ObjectFormatter.Format(sample, _esEs));
        }

        [TestMethod]
        public void FormatNull()
        {
            Assert.AreEqual("", ObjectFormatter.Format(null, CultureInfo.InvariantCulture));
            Assert.AreEqual("", ObjectFormatter.Format(null, _ptBr));
            Assert.AreEqual("", ObjectFormatter.Format(null, _enUs));
            Assert.AreEqual("", ObjectFormatter.Format(null, _esEs));

            Assert.AreEqual("", ObjectFormatter.Format(new FooNullable(), CultureInfo.InvariantCulture));
            Assert.AreEqual("", ObjectFormatter.Format(new FooNullable(), _ptBr));
            Assert.AreEqual("", ObjectFormatter.Format(new FooNullable(), _enUs));
            Assert.AreEqual("", ObjectFormatter.Format(new FooNullable(), _esEs));
        }

        [TestMethod]
        public void FormatList()
        {
            var sample = new object[] { 1.5, "abc", 3, new DateTime(2010, 5, 14), null };
            Assert.AreEqual("\"1.5, abc, 3, 2010-05-14, \"", ObjectFormatter.Format(sample, CultureInfo.InvariantCulture));
            Assert.AreEqual("\"1,5, abc, 3, 2010-05-14, \"", ObjectFormatter.Format(sample, _ptBr));
            Assert.AreEqual("\"1.5, abc, 3, 2010-05-14, \"", ObjectFormatter.Format(sample, _enUs));
            Assert.AreEqual("\"1,5, abc, 3, 2010-05-14, \"", ObjectFormatter.Format(sample, _esEs));
        }

        [TestMethod]
        [DeploymentItem(@"pt-BR\Dongle.resources.dll", "pt-BR")]
        [DeploymentItem(@"es-ES\Dongle.resources.dll", "es-ES")]
        public void FormatBoolean()
        {
            const bool SampleTrue = true;
            Assert.AreEqual("TRUE", ObjectFormatter.Format(SampleTrue, CultureInfo.InvariantCulture));
            Assert.AreEqual("VERDADEIRO", ObjectFormatter.Format(SampleTrue, _ptBr));
            Assert.AreEqual("TRUE", ObjectFormatter.Format(SampleTrue, _enUs));
            Assert.AreEqual("VERDADERO", ObjectFormatter.Format(SampleTrue, _esEs));

            const bool SampleFalse = false;
            Assert.AreEqual("FALSE", ObjectFormatter.Format(SampleFalse, CultureInfo.InvariantCulture));
            Assert.AreEqual("FALSO", ObjectFormatter.Format(SampleFalse, _ptBr));
            Assert.AreEqual("FALSE", ObjectFormatter.Format(SampleFalse, _enUs));
            Assert.AreEqual("FALSO", ObjectFormatter.Format(SampleFalse, _esEs));
        }
    }
}