using System;
using System.ComponentModel.DataAnnotations.MaxLengthAttribute;
using System.Globalization;
using Dongle.Reflection.PropertySetters;
using NUnit.Framework;

namespace Dongle.Tests.Serialization
{
    [TestFixture]
    public class PropertySettersTest
    {
        [Flags]
        public enum SimpleEnum
        {
            None = 0,
            Number1 = 1,
            Number2 = 2,
            Number4 = 4
        }

        public class SimpleClass
        {
            public string StringField;

            [MaxLength(Length = 4)]
            public string LimitedStringProperty { get; set; }

            public string StringProperty { get; set; }

            public int IntProperty { get; set; }
            
            public long LongProperty { get; set; }
            
            public DateTime DateTimeProperty { get; set; }

            public SimpleEnum EnumProperty { get; set; }
        }
        
        [Test]
        public void TestBypassSetter()
        {
            var obj = new SimpleClass();
            var setter = new BypassSetter(new PropertySetterBase.FieldMapData (typeof(SimpleClass).GetProperty("StringProperty")));
            setter.Set(obj, "alo mundo");
            Assert.AreEqual("alo mundo", obj.StringProperty);

            var setter2 = new BypassSetter(new PropertySetterBase.FieldMapData (typeof(SimpleClass).GetProperty("IntProperty")));
            setter2.Set(obj, int.MaxValue.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(int.MaxValue, obj.IntProperty);

            var setter3 = new BypassSetter(new PropertySetterBase.FieldMapData (typeof(SimpleClass).GetProperty("LongProperty") ));
            setter3.Set(obj, Int64.MaxValue.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(Int64.MaxValue, obj.LongProperty);

            var setter4 = new BypassSetter(new PropertySetterBase.FieldMapData (typeof(SimpleClass).GetProperty("LongProperty") ));
            setter4.Set(obj, "127");
            Assert.AreEqual(127, obj.LongProperty);

            var setter5 = new BypassSetter(new PropertySetterBase.FieldMapData (typeof(SimpleClass).GetProperty("DateTimeProperty") ));
            setter5.Set(obj, "2011-05-01T07:34:42-5:00");
            Assert.AreEqual(DateTime.Parse("2011-05-01T07:34:42-5:00"), obj.DateTimeProperty);
        }

        [Test]
        public void TestDateTimeSetter()
        {
            var obj = new SimpleClass();
            var fieldMap = new PropertySetterBase.FieldMapData(typeof (SimpleClass).GetProperty("DateTimeProperty"), "dd/MMM/yyyy:HH:mm:ss");

            var setter = new DateTimeSetter(fieldMap);
            setter.Set(obj, "20/Dez/2010:00:02:38");
            Assert.AreEqual(DateTime.Parse("2010-12-20T00:02:38"), obj.DateTimeProperty);
            
            setter.Set(obj, "20/Dec/2010:00:02:38");
            Assert.AreEqual(DateTime.Parse("2010-12-20T00:02:38"), obj.DateTimeProperty);

            setter.Set(obj, "2010-12-20 00:02:38");
            Assert.AreEqual(DateTime.Parse("2010-12-20T00:02:38"), obj.DateTimeProperty);

            setter.Set(obj, "1313029269");
            Assert.AreEqual(DateTime.Parse("2011-08-11T02:21:09"), obj.DateTimeProperty);

            setter.Set(obj, "26/04/2013 17:16:26");
            Assert.AreEqual(DateTime.Parse("2013-04-26T17:16:26"), obj.DateTimeProperty);
        }

        [Test]
        public void TestHexToLongDWordSetter()
        {
            var obj = new SimpleClass();
            
            var fieldMap = new PropertySetterBase.FieldMapData(typeof (SimpleClass).GetProperty("LongProperty"));
            
            var setter = new HexToLongDWordSetter(fieldMap);
            setter.Set(obj, "21C5431");
            Assert.AreEqual(35410993, obj.LongProperty);

            setter.Set(obj, "2F15DB8A4C");
            Assert.AreEqual(202230172236, obj.LongProperty);

            obj.LongProperty = -933489160;
            var parsedValue = setter.Get(obj);
            Assert.AreEqual("C85C15F8", parsedValue);
        }

        [Test]
        public void TestHexToLongQWordSetter()
        {
            var obj = new SimpleClass();
            var fieldMap = new PropertySetterBase.FieldMapData (typeof(SimpleClass).GetProperty("LongProperty"));

            var setter = new HexToLongQWordSetter(fieldMap);
            setter.Set(obj, "21C5431");
            Assert.AreEqual(35410993, obj.LongProperty);
        }
        
        [Test]
        public void TestVarCharSetter()
        {
            const string stringBig = "Esta string tem mais de 20 caracteres e precisa ser cortada aqui senão vai gerar uma exception.";

            var obj = new SimpleClass();
            var fieldMap = new PropertySetterBase.FieldMapData(typeof(SimpleClass).GetProperty("StringProperty")) {MaxLength = 20 };

            var setter = new VarCharSetter(fieldMap);
            setter.Set(obj, stringBig);
            Assert.AreEqual(stringBig.Substring(0, 20), obj.StringProperty);
        }

        [Test]
        public void TestEnumSetter()
        {
            var fieldMap = new PropertySetterBase.FieldMapData(typeof(SimpleClass).GetProperty("EnumProperty"));
            var setter = new EnumSetter<SimpleEnum>(fieldMap);
            
            var obj = new SimpleClass();
            setter.Set(obj, "1");
            Assert.AreEqual(SimpleEnum.Number1, obj.EnumProperty);

            obj = new SimpleClass();
            setter.Set(obj, "3");
            Assert.AreEqual(SimpleEnum.Number1 | SimpleEnum.Number2, obj.EnumProperty);

            obj = new SimpleClass();
            setter.Set(obj, "9999");
            Assert.AreEqual((SimpleEnum)9999, obj.EnumProperty);

            obj = new SimpleClass();
            setter.Set(obj, "asfdsdafadf");
            Assert.AreEqual(SimpleEnum.None, obj.EnumProperty);
        }
    }
}
