using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Dongle.Web.Resources;

namespace Dongle.Web.ModelAttributes
{
    public sealed class WMultipleEmailAttribute : RegularExpressionAttribute, IClientValidatable
    {
        private const string PatternStr =
            @"^([\w+-.%]+@[\w-.]+\.[A-Za-z]{2,4},?)+$";

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