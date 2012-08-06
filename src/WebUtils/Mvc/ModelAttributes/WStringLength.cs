using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WebUtils.Resources;

namespace WebUtils.Mvc.ModelAttributes
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
            var msg = ErrorMessage ?? WebUtilsResource.InvalidStringLength;

            if (!_requiredAttribute.IsValid(value))
            {
                return new ValidationResult(msg, new[] { validationContext.MemberName });
            }
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var msg = ErrorMessage ?? WebUtilsResource.InvalidStringLength;

            var rule = new ModelClientValidationRule
            {
                ErrorMessage = msg,
                ValidationType = "wstringlength",
            };
            yield return rule;
        }
    }
}
