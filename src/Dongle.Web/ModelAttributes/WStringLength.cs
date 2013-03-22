using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Dongle.Web.Resources;

namespace Dongle.Web.ModelAttributes
{
    public class WStringLength : StringLengthAttribute, IClientValidatable
    {
        private readonly int _maxlength;

        public WStringLength(int maximumLength)
            : base(maximumLength)
        {
            _maxlength = maximumLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (Convert.ToInt32(value) <= _maxlength)
            {
                return null;
            }
            return new ValidationResult(FormatErrorMessage(ErrorMessage));
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var msg = ErrorMessage ?? DongleResource.InvalidStringLength;

            var rule = new ModelClientValidationRule
            {
                ErrorMessage = msg,
                ValidationType = "wstringlength",
            };
            rule.ValidationParameters.Add("maxlength", _maxlength);
            yield return rule;
        }
    }
}
