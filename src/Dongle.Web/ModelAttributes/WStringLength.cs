using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Dongle.Web.Resources;

namespace Dongle.Web.ModelAttributes
{
    public class WStringLength : ValidationAttribute, IClientValidatable
    {
        private readonly StringLengthAttribute _stringLengthAttribute;
        private readonly int _maximumLength;

        public WStringLength(int maximumLength)
        {
            _maximumLength = maximumLength;
            _stringLengthAttribute = new StringLengthAttribute(maximumLength);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var msg = String.Format(DongleResource.InvalidStringLength, _maximumLength);

            if (!_stringLengthAttribute.IsValid(value))
            {
                return new ValidationResult(msg, new[] { validationContext.MemberName });
            }
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var msg = String.Format(DongleResource.InvalidStringLength, _maximumLength);

            var rule = new ModelClientValidationRule
            {
                ErrorMessage = msg,
                ValidationType = "stringlength",
            };
            yield return rule;
        }
    }
}
