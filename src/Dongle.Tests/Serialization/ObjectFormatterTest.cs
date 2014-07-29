using System;
using System.Globalization;

using Dongle.Serialization;
using Dongle.Tests.Tools;

using NUnit.Framework;

namespace Dongle.Tests.Serialization
{
    [TestFixture]
    public class ObjectFormatterTest
    {
        private readonly CultureInfo _ptBr = CultureInfo.GetCultureInfo("pt-BR");
        private readonly CultureInfo _enUs = CultureInfo.GetCultureInfo("en-US");
        private readonly CultureInfo _esEs = CultureInfo.GetCultureInfo("es-ES");

        [Test]
        public void FormatNumber()
        {
            Assert.AreEqual("1.5", ObjectFormatter.Format(1.5, CultureInfo.InvariantCulture));
            Assert.AreEqual("\"1,5\"", ObjectFormatter.Format(1.5, _ptBr));
            Assert.AreEqual("1.5", ObjectFormatter.Format(1.5, _enUs));
            Assert.AreEqual("\"1,5\"", ObjectFormatter.Format(1.5, _esEs));

            Assert.AreEqual("2", ObjectFormatter.Format(2, CultureInfo.InvariantCulture));
            Assert.AreEqual("-2", ObjectFormatter.Format(-2, CultureInfo.InvariantCulture));
        }

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
        public void FormatList()
        {
            var sample = new object[] { 1.5, "abc", 3, new DateTime(2010, 5, 14), null };
            Assert.AreEqual("\"1.5, abc, 3, 2010-05-14, \"", ObjectFormatter.Format(sample, CultureInfo.InvariantCulture));
            Assert.AreEqual("\"1,5, abc, 3, 2010-05-14, \"", ObjectFormatter.Format(sample, _ptBr));
            Assert.AreEqual("\"1.5, abc, 3, 2010-05-14, \"", ObjectFormatter.Format(sample, _enUs));
            Assert.AreEqual("\"1,5, abc, 3, 2010-05-14, \"", ObjectFormatter.Format(sample, _esEs));
        }

        [Test]
        public void FormatBoolean()
        {
            const bool sampleTrue = true;
            Assert.AreEqual("TRUE", ObjectFormatter.Format(sampleTrue, CultureInfo.InvariantCulture));
            Assert.AreEqual("VERDADEIRO", ObjectFormatter.Format(sampleTrue, _ptBr));
            Assert.AreEqual("TRUE", ObjectFormatter.Format(sampleTrue, _enUs));
            Assert.AreEqual("VERDADERO", ObjectFormatter.Format(sampleTrue, _esEs));

            const bool sampleFalse = false;
            Assert.AreEqual("FALSE", ObjectFormatter.Format(sampleFalse, CultureInfo.InvariantCulture));
            Assert.AreEqual("FALSO", ObjectFormatter.Format(sampleFalse, _ptBr));
            Assert.AreEqual("FALSE", ObjectFormatter.Format(sampleFalse, _enUs));
            Assert.AreEqual("FALSO", ObjectFormatter.Format(sampleFalse, _esEs));
        }
    }
}