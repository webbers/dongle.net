using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Dongle.Web.ModelAttributes
{
    public class WMinDateFieldAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly string _dateTimePickerField;

        public WMinDateFieldAttribute(string dateTimePickerField)
        {
            _dateTimePickerField = dateTimePickerField;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(""),
                ValidationType = "daterangefield",
            };
            rule.ValidationParameters.Add("minfield", _dateTimePickerField);

            yield return rule;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return new ValidationResult(FormatErrorMessage(ErrorMessage));
        }

        public override string FormatErrorMessage(string name)
        {
            return "ok";
        }
    }
}
