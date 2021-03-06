using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Dongle.Resources;

namespace Dongle.Web.ModelAttributes
{
    public sealed class WIpWildCardAttribute : RegularExpressionAttribute, IClientValidatable
    {
        private const string PatternStr =
            @"^((((\*|\?)|25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){1,3}((\*|\?)|25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|(((\*|\?)|[0-9a-fA-F]{1,4}):){7,7}((\*|\?)|[0-9a-fA-F]{1,4})|(((\*|\?)|[0-9a-fA-F]{1,4}):){1,7}:|(((\*|\?)|[0-9a-fA-F]{1,4}):){1,6}:((\*|\?)|[0-9a-fA-F]{1,4})|(((\*|\?)|[0-9a-fA-F]{1,4}):){1,5}(:((\*|\?)|[0-9a-fA-F]{1,4})){1,2}|(((\*|\?)|[0-9a-fA-F]{1,4}):){1,4}(:((\*|\?)|[0-9a-fA-F]{1,4})){1,3}|(((\*|\?)|[0-9a-fA-F]{1,4}):){1,3}(:((\*|\?)|[0-9a-fA-F]{1,4})){1,4}|(((\*|\?)|[0-9a-fA-F]{1,4}):){1,2}(:((\*|\?)|[0-9a-fA-F]{1,4})){1,5}|((\*|\?)|[0-9a-fA-F]{1,4}):((:((\*|\?)|[0-9a-fA-F]{1,4})){1,6})|:((:((\*|\?)|[0-9a-fA-F]{1,4})){1,7}|:)|fe80:(:((\*|\?)|[0-9a-fA-F]{0,4})){0,4}%((\*|\?)|[0-9a-zA-Z]{1,})|::(((\*|\?)|ffff)(:((\*|\?)|(0{1,4}))){0,1}:){0,1}(((\*|\?)|25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]).){3,3}((\*|\?)|25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|(((\*|\?)|[0-9a-fA-F]{1,4}):){1,4}:(((\*|\?)|25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]).){3,3}((\*|\?)|25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))$";

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