using System;
using System.ComponentModel.DataAnnotations;
using Dongle.Web.ModelAttributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dongle.Web.Tests.Mvc.ModelAttributes
{
    [TestClass]
    public class RequiredIfAttributeTests
    {
        public class TestClass
        {
            public string DependentPropertyString { get; set; }
            public int DependentPropertyInt32 { get; set; }
            public int? DependentPropertyNullableInt32 { get; set; }
            public bool DependentPropertyBool { get; set; }
            public bool? DependentPropertyNullableBool { get; set; }
        }

        #region Null values passed in

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Validate_DependentStringMatches_NullValue_Throws()
        {
            var instance = new TestClass { DependentPropertyString = "ConditionalValue" };
            var attr = new WRequiredIfAttribute("DependentPropertyString", "ConditionalValue");
            var validationContext = new ValidationContext(instance, null, null);
            attr.Validate(null, validationContext);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Validate_DependentNullMatches_NullValue_Throws()
        {
            var instance = new TestClass { DependentPropertyString = null };
            var attr = new WRequiredIfAttribute("DependentPropertyString", null);
            var validationContext = new ValidationContext(instance, null, null);
            attr.Validate(null, validationContext);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Validate_DependentInt32Matches_NullValue_Throws()
        {
            var instance = new TestClass { DependentPropertyInt32 = 123 };
            var attr = new WRequiredIfAttribute("DependentPropertyInt32", 123);
            var validationContext = new ValidationContext(instance, null, null);
            attr.Validate(null, validationContext);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Validate_DependentNullableInt32Matches_NullValue_Throws()
        {
            var instance = new TestClass { DependentPropertyNullableInt32 = 123 };
            var attr = new WRequiredIfAttribute("DependentPropertyNullableInt32", 123);
            var validationContext = new ValidationContext(instance, null, null);
            attr.Validate(null, validationContext);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Validate_DependentNullableInt32NullMatches_NullValue_Throws()
        {
            var instance = new TestClass { DependentPropertyNullableInt32 = null };
            var attr = new WRequiredIfAttribute("DependentPropertyNullableInt32", null);
            var validationContext = new ValidationContext(instance, null, null);
            attr.Validate(null, validationContext);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Validate_DependentBoolMatches_NullValue_Throws()
        {
            var instance = new TestClass { DependentPropertyBool = true };
            var attr = new WRequiredIfAttribute("DependentPropertyBool", true);
            var validationContext = new ValidationContext(instance, null, null);
            attr.Validate(null, validationContext);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Validate_DependentNullableBoolMatches_NullValue_Throws()
        {
            var instance = new TestClass { DependentPropertyNullableBool = true };
            var attr = new WRequiredIfAttribute("DependentPropertyNullableBool", true);
            var validationContext = new ValidationContext(instance, null, null);
            attr.Validate(null, validationContext);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Validate_DependentNullableBoolNullMatches_NullValue_Throws()
        {
            var instance = new TestClass { DependentPropertyNullableBool = null };
            var attr = new WRequiredIfAttribute("DependentPropertyNullableBool", null);
            var validationContext = new ValidationContext(instance, null, null);
            attr.Validate(null, validationContext);
        }

        #endregion

        #region String.Empty passed in

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Validate_DependentStringMatches_EmptyStringValue_Throws()
        {
            var instance = new TestClass { DependentPropertyString = "ConditionalValue" };
            var attr = new WRequiredIfAttribute("DependentPropertyString", "ConditionalValue");
            var validationContext = new ValidationContext(instance, null, null);
            attr.Validate(String.Empty, validationContext);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Validate_DependentNullMatches_EmptyStringValue_Throws()
        {
            var instance = new TestClass { DependentPropertyString = null };
            var attr = new WRequiredIfAttribute("DependentPropertyString", null);
            var validationContext = new ValidationContext(instance, null, null);
            attr.Validate(String.Empty, validationContext);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Validate_DependentInt32Matches_EmptyStringValue_Throws()
        {
            var instance = new TestClass { DependentPropertyInt32 = 123 };
            var attr = new WRequiredIfAttribute("DependentPropertyInt32", 123);
            var validationContext = new ValidationContext(instance, null, null);
            attr.Validate(String.Empty, validationContext);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Validate_DependentNullableInt32Matches_EmptyStringValue_Throws()
        {
            var instance = new TestClass { DependentPropertyNullableInt32 = 123 };
            var attr = new WRequiredIfAttribute("DependentPropertyNullableInt32", 123);
            var validationContext = new ValidationContext(instance, null, null);
            attr.Validate(String.Empty, validationContext);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Validate_DependentNullableInt32NullMatches_EmptyStringValue_Throws()
        {
            var instance = new TestClass { DependentPropertyNullableInt32 = null };
            var attr = new WRequiredIfAttribute("DependentPropertyNullableInt32", null);
            var validationContext = new ValidationContext(instance, null, null);
            attr.Validate(String.Empty, validationContext);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Validate_DependentBoolMatches_EmptyStringValue_Throws()
        {
            var instance = new TestClass { DependentPropertyBool = true };
            var attr = new WRequiredIfAttribute("DependentPropertyBool", true);
            var validationContext = new ValidationContext(instance, null, null);
            attr.Validate(String.Empty, validationContext);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Validate_DependentNullableBoolMatches_EmptyStringValue_Throws()
        {
            var instance = new TestClass { DependentPropertyNullableBool = true };
            var attr = new WRequiredIfAttribute("DependentPropertyNullableBool", true);
            var validationContext = new ValidationContext(instance, null, null);
            attr.Validate(String.Empty, validationContext);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Validate_DependentNullableBoolNullMatches_EmptyStringValue_Throws()
        {
            var instance = new TestClass { DependentPropertyNullableBool = null };
            var attr = new WRequiredIfAttribute("DependentPropertyNullableBool", null);
            var validationContext = new ValidationContext(instance, null, null);
            attr.Validate(String.Empty, validationContext);
        }

        #endregion

        #region Non-null values passed in

        [TestMethod]
        public void Validate_DependentStringMatches_NonNullValue_DoesNotThrow()
        {
            var instance = new TestClass { DependentPropertyString = "ConditionalValue" };
            var attr = new WRequiredIfAttribute("DependentPropertyString", "ConditionalValue");
            var validationContext = new ValidationContext(instance, null, null);
            attr.Validate("Non null text", validationContext);
        }

        [TestMethod]
        public void Validate_DependentNullMatches_NonNullValue_DoesNotThrow()
        {
            var instance = new TestClass { DependentPropertyString = null };
            var attr = new WRequiredIfAttribute("DependentPropertyString", null);
            var validationContext = new ValidationContext(instance, null, null);
            attr.Validate("Non null text", validationContext);
        }

        [TestMethod]
        public void Validate_DependentInt32Matches_NonNullValue_DoesNotThrow()
        {
            var instance = new TestClass { DependentPropertyInt32 = 123 };
            var attr = new WRequiredIfAttribute("DependentPropertyInt32", 123);
            var validationContext = new ValidationContext(instance, null, null);
            attr.Validate("Non null text", validationContext);
        }

        [TestMethod]
        public void Validate_DependentNullableInt32Matches_NonNullValue_DoesNotThrow()
        {
            var instance = new TestClass { DependentPropertyNullableInt32 = 123 };
            var attr = new WRequiredIfAttribute("DependentPropertyNullableInt32", 123);
            var validationContext = new ValidationContext(instance, null, null);
            attr.Validate("Non null text", validationContext);
        }

        [TestMethod]
        public void Validate_DependentNullableInt32NullMatches_NonNullValue_DoesNotThrow()
        {
            var instance = new TestClass { DependentPropertyNullableInt32 = null };
            var attr = new WRequiredIfAttribute("DependentPropertyNullableInt32", null);
            var validationContext = new ValidationContext(instance, null, null);
            attr.Validate("Non null text", validationContext);
        }

        [TestMethod]
        public void Validate_DependentBoolMatches_NonNullValue_DoesNotThrow()
        {
            var instance = new TestClass { DependentPropertyBool = true };
            var attr = new WRequiredIfAttribute("DependentPropertyBool", true);
            var validationContext = new ValidationContext(instance, null, null);
            attr.Validate("Non null text", validationContext);
        }

        [TestMethod]
        public void Validate_DependentNullableBoolMatches_NonNullValue_DoesNotThrow()
        {
            var instance = new TestClass { DependentPropertyNullableBool = true };
            var attr = new WRequiredIfAttribute("DependentPropertyNullableBool", true);
            var validationContext = new ValidationContext(instance, null, null);
            attr.Validate("Non null text", validationContext);
        }

        [TestMethod]
        public void Validate_DependentNullableBoolNullMatches_NonNullValue_DoesNotThrow()
        {
            var instance = new TestClass { DependentPropertyNullableBool = null };
            var attr = new WRequiredIfAttribute("DependentPropertyNullableBool", null);
            var validationContext = new ValidationContext(instance, null, null);
            attr.Validate("Non null text", validationContext);
        }

        #endregion

        #region Error messages

        [TestMethod]
        public void FormatErrorMessage_CustomMessage_MatchesSpecifiedMessage()
        {
            var message = "{0} custom message";
            var attr = new WRequiredIfAttribute("Dummy", "Dummy") { ErrorMessage = message };

            var custommsg = attr.FormatErrorMessage("NAME");

            Assert.AreEqual(String.Format(message, "NAME"), custommsg);
        }

        #endregion
    }
}