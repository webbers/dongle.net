using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WebUtils.Resources;

namespace WebUtils.Mvc.ModelAttributes
{
    public class WRequiredAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly RequiredAttribute _requiredAttribute = new RequiredAttribute();

        public WRequiredAttribute()
        {
        }

        public WRequiredAttribute(string errorMessage)
            : base(errorMessage)
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var msg = ErrorMessage ?? WebUtilsResource.ThisFieldIsRequired;

            if (!_requiredAttribute.IsValid(value))
            {
                return new ValidationResult(msg, new[] { validationContext.MemberName });
            }
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var msg = ErrorMessage ?? WebUtilsResource.ThisFieldIsRequired;

            var rule = new ModelClientValidationRule
                           {
                               ErrorMessage = msg,
                               ValidationType = "wrequired",
                           };
            yield return rule;
        }
    }
}