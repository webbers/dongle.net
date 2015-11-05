using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Dongle.Resources;

namespace Dongle.Web.ModelAttributes
{
    public sealed class WMultipleEmailAttribute : RegularExpressionAttribute, IClientValidatable
    {
        private const string PatternStr =
            @"^(([\w\%\-\._\!]+)@((([0-9]{1,3}\.){3}[0-9])|(([\w]+\.[A-Za-z]{2,})(\.[A-Za-z]{1,2})?))(\,?))+$";

        public WMultipleEmailAttribute()
            : base(PatternStr)
        {
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var validationRule = new ModelClientValidationRule
            {
                ErrorMessage = DongleResource.InvalidEmail,
            };

            validationRule.ValidationParameters.Add("pattern", PatternStr);
            validationRule.ValidationType = "regex";

            return new List<ModelClientValidationRule> { validationRule };
        }
    }
}