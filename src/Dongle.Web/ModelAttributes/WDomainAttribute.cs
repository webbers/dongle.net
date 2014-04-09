using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Dongle.Web.Resources;

namespace Dongle.Web.ModelAttributes
{
    public sealed class WDomainAttribute : RegularExpressionAttribute, IClientValidatable
    {
        private const string PatternStr = @"^([a-zA-Z0-9\?\*]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9\*\?])?\.)+([a-zA-Z]{0,}$)|^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";

        public WDomainAttribute()
            : base(PatternStr)
        {
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var validationRule = new ModelClientValidationRule
            {
                ErrorMessage = DongleResource.InvalidDomain,
            };

            validationRule.ValidationParameters.Add("pattern", PatternStr);
            validationRule.ValidationType = "regex";

            return new List<ModelClientValidationRule> { validationRule };
        }
    }
}