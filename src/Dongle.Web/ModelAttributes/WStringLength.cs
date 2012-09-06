using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Dongle.Web.Resources;

namespace Dongle.Web.ModelAttributes
{
    public class WStringLength : StringLengthAttribute, IClientValidatable
    {
        private readonly RequiredAttribute _requiredAttribute = new RequiredAttribute();

        public WStringLength(int maximumLength)
            : base(maximumLength)
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var msg = ErrorMessage ?? DongleResource.InvalidStringLength;

            if (!_requiredAttribute.IsValid(value))
            {
                return new ValidationResult(msg, new[] { validationContext.MemberName });
            }
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var msg = ErrorMessage ?? DongleResource.InvalidStringLength;

            var rule = new ModelClientValidationRule
            {
                ErrorMessage = msg,
                ValidationType = "wstringlength",
            };
            yield return rule;
        }
    }
}
