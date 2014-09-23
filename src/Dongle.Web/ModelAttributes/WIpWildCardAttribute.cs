using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Dongle.Web.Resources;

namespace Dongle.Web.ModelAttributes
{
    public sealed class WIpWildCardAttribute : RegularExpressionAttribute, IClientValidatable
    {
        private const string PatternStr =
            @"^((((\*|\?)|25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){1,3}((\*|\?)|25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))$";

        public WIpWildCardAttribute()
            : base(PatternStr)
        {
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var validationRule = new ModelClientValidationRule
            {
                ErrorMessage = DongleResource.InvalidIpAddress,
            };

            validationRule.ValidationParameters.Add("pattern", PatternStr);
            validationRule.ValidationType = "regex";

            return new List<ModelClientValidationRule> { validationRule };
        }
    }
}