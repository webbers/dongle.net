using System;
using System.ComponentModel.DataAnnotations.MaxLengthAttribute;
using Dongle.Reflection.PropertySetters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dongle.Tests.Serialization
{
    [TestClass]
    public class PropertySettersTest
    {        
        public class SimpleClass
        {
            public string StringField;

            [MaxLength(Length = 4)]
            public string LimitedStringProperty { get; set; }

            public string StringProperty { get; set; }

            public int IntProperty { get; set; }
            
            public long LongProperty { get; set; }
            
            public DateTime DateTimeProperty { get; set; }
        }

        [TestMethod]
        public void TestBrowserSetter()
        {
            var fieldMap = new PropertySetterBase.FieldMapData(typeof(SimpleClass).GetProperty("StringProperty"));
            var setter = new BrowserSetter(fieldMap);
            var obj = new SimpleClass();

            setter.FieldMap.SetterParameters = "shortname";
            setter.Set(obj, "IE8.00.6001.18702");
            Assert.AreEqual("IE", obj.StringProperty);

            obj = new SimpleClass();
            setter.FieldMap.SetterParameters = "version";
            setter.Set(obj, "IE8.00.6001.18702");
            Assert.AreEqual("8.00.6001.18702", obj.StringProperty);

            obj = new SimpleClass();
            setter.FieldMap.SetterParameters = "name";
            setter.Set(obj, "[teste]0");
            Assert.AreEqual("*", obj.StringProperty);

            obj = new SimpleClass();
            setter.FieldMap.SetterParameters = "version";
            setter.Set(obj, "[chromium-browser]0");
            Assert.IsNull(obj.StringProperty);

            obj = new SimpleClass();
            setter.FieldMap.SetterParameters = "version";
            setter.Set(obj, "");
            Assert.IsNull(obj.StringProperty);

            obj = new SimpleClass();
            setter.FieldMap.SetterParameters = "name";
            setter.Set(obj, "SA");
            Assert.AreEqual("SAFARI", obj.StringProperty);

            obj = new SimpleClass();
            setter.FieldMap.SetterParameters = "shortname";
            setter.Set(obj, "SA");
            Assert.AreEqual("SA", obj.StringProperty);

            obj = new SimpleClass();
            setter.FieldMap.SetterParameters = "version";
            setter.Set(obj, "SA");
            Assert.IsNull(obj.StringProperty);

            obj = new SimpleClass();
            setter.FieldMap.SetterParameters = "name";
            setter.Set(obj, "nonono");
            Assert.AreEqual("*", obj.StringProperty);

            obj = new SimpleClass();
            setter.FieldMap.SetterParameters = "shortname";
            setter.Set(obj, "");
            Assert.AreEqual("*", obj.StringProperty);

            //Testando propriedade com limite de caracteres
            obj = new SimpleClass();
            setter.FieldMap.SetterParameters = "version";
            setter.FieldMap.MaxLength = 3;
            setter.Set(obj, "IE8.00.6001.18702");
            Assert.AreEqual("8.0", obj.StringProperty);

            //Testando field
            obj = new SimpleClass();
            setter.FieldMap.Field = typeof(SimpleClass).GetField("StringField");
            setter.FieldMap.MemberIsField = true;
            setter.FieldMap.MaxLength = 4;
            setter.FieldMap.SetterParameters = "version";
            setter.Set(obj, "IE8.00.6001.18702");
            Assert.AreEqual("8.00", obj.StringField);
        }
        
        [TestMethod]
        public void TestBypassSetter()
        {
            var obj = new SimpleClass();
            var setter = new BypassSetter(new PropertySetterBase.FieldMapData (typeof(SimpleClass).GetProperty("StringProperty")));
            setter.Set(obj, "alo mundo");
            Assert.AreEqual("alo mundo", obj.StringProperty);

            var setter2 = new BypassSetter(new PropertySetterBase.FieldMapData (typeof(SimpleClass).GetProperty("IntProperty")));
            setter2.Set(obj, int.MaxValue.ToString());
            Assert.AreEqual(int.MaxValue, obj.IntProperty);

            var setter3 = new BypassSetter(new PropertySetterBase.FieldMapData (typeof(SimpleClass).GetProperty("LongProperty") ));
            setter3.Set(obj, Int64.MaxValue.ToString());
            Assert.AreEqual(Int64.MaxValue, obj.LongProperty);

            var setter4 = new BypassSetter(new PropertySetterBase.FieldMapData (typeof(SimpleClass).GetProperty("LongProperty") ));
            setter4.Set(obj, "127");
            Assert.AreEqual(127, obj.LongProperty);

            var setter5 = new BypassSetter(new PropertySetterBase.FieldMapData (typeof(SimpleClass).GetProperty("DateTimeProperty") ));
            setter5.Set(obj, "2011-05-01T07:34:42-5:00");
            Assert.AreEqual(DateTime.Parse("2011-05-01T07:34:42-5:00"), obj.DateTimeProperty);
        }

        [TestMethod]
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

        [TestMethod]
        public void TestHexToLongDWordSetter()
        {
            var obj = new SimpleClass();
            var fieldMap = new PropertySetterBase.FieldMapData(typeof (SimpleClass).GetProperty("LongProperty"));
            
            var setter = new HexToLongDWordSetter(fieldMap);
            setter.Set(obj, "21C5431");
            Assert.AreEqual(35410993, obj.LongProperty);
        }

        [TestMethod]
        public void TestHexToLongQWordSetter()
        {
            var obj = new SimpleClass();
            var fieldMap = new PropertySetterBase.FieldMapData (typeof(SimpleClass).GetProperty("LongProperty"));

            var setter = new HexToLongQWordSetter(fieldMap);
            setter.Set(obj, "21C5431");
            Assert.AreEqual(35410993, obj.LongProperty);
        }
        

        [TestMethod]
        public void TestOperatingSystemSetter()
        {
            //Testando o OsName
            var evnt = new SimpleClass();
            var fieldMap = new PropertySetterBase.FieldMapData(typeof(SimpleClass).GetProperty("StringProperty"), "name");

            var setter = new OperatingSystemSetter(fieldMap);
            setter.Set(evnt, "2.6.2.8250.0.0.1.256");
            Assert.AreEqual("Windows 8 Ultimate", evnt.StringProperty);

            setter = new OperatingSystemSetter(fieldMap);
            setter.Set(evnt, "2.5.0.1111.0.0.1.128");
            Assert.AreEqual("Windows 2000 Professional", evnt.StringProperty);

            setter = new OperatingSystemSetter(fieldMap);
            setter.Set(evnt, "A3.2.1");
            Assert.AreEqual("Android", evnt.StringProperty);

            setter = new OperatingSystemSetter(fieldMap);
            setter.Set(evnt, "I5.0");
            Assert.AreEqual("Ios", evnt.StringProperty);

            setter = new OperatingSystemSetter(fieldMap);
            setter.Set(evnt, "M5.0");
            Assert.AreEqual("Mac OS", evnt.StringProperty);

            setter = new OperatingSystemSetter(fieldMap);
            setter.Set(evnt, "L6.0.52146");
            Assert.AreEqual("Linux", evnt.StringProperty);

            setter = new OperatingSystemSetter(fieldMap);
            setter.Set(evnt, "2.6.1.8250.0.0.1.256");
            Assert.AreEqual("Windows 7 Ultimate", evnt.StringProperty);

            setter = new OperatingSystemSetter(fieldMap);
            setter.Set(evnt, "2.6.1.7601.1.0.2.18");
            Assert.AreEqual("Windows Server 2008 R2 Home", evnt.StringProperty);

            //Testando o OsShortName
            fieldMap.Property = typeof(SimpleClass).GetProperty("StringProperty");            
            fieldMap.SetterParameters = "shortname";
            setter = new OperatingSystemSetter(fieldMap);
            setter.Set(evnt, "2.6.2.8250.0.0.1.256");
            Assert.AreEqual("WIN8", evnt.StringProperty);

            setter = new OperatingSystemSetter(fieldMap);
            setter.Set(evnt, "A3.2.1");
            Assert.AreEqual("ADR", evnt.StringProperty);

            setter = new OperatingSystemSetter(fieldMap);
            setter.Set(evnt, "I5.0");
            Assert.AreEqual("IOS", evnt.StringProperty);

            setter = new OperatingSystemSetter(fieldMap);
            setter.Set(evnt, "M5.0");
            Assert.AreEqual("MAC", evnt.StringProperty);

            setter = new OperatingSystemSetter(fieldMap);
            setter.Set(evnt, "L6.0.52146");
            Assert.AreEqual("LINUX", evnt.StringProperty);

            setter = new OperatingSystemSetter(fieldMap);
            setter.Set(evnt, "2.6.1.8250.0.0.1.256");
            Assert.AreEqual("WIN7", evnt.StringProperty);

            setter = new OperatingSystemSetter(fieldMap);
            setter.Set(evnt, "2.6.1.7601.1.0.2.18");
            Assert.AreEqual("2008R2", evnt.StringProperty);            
        }

        [TestMethod]
        public void TestVarCharSetter()
        {
            const string stringBig = "Esta string tem mais de 20 caracteres e precisa ser cortada aqui senão vai gerar uma exception.";

            var obj = new SimpleClass();
            var fieldMap = new PropertySetterBase.FieldMapData(typeof(SimpleClass).GetProperty("StringProperty")) {MaxLength = 20 };

            var setter = new VarCharSetter(fieldMap);
            setter.Set(obj, stringBig);
            Assert.AreEqual(stringBig.Substring(0, 20), obj.StringProperty);
        }
    }
}
