using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Dongle.Resources;

namespace Dongle.Web.ModelAttributes
{
    public sealed class WUrlAttribute : RegularExpressionAttribute, IClientValidatable
    {
        private readonly bool _allowSpace;
        private const string PatternStr = @"((https?[\*\?]|https?|file[\*\?]|file|[\*\?]):((//)|(\\\\)))+([\*\?]*[\w\d\.:#@/;$*()%~_?\+-=&]*)";
        private const string PatternStrAllowingSpace = @"((https?[\*\?]|https?|file[\*\?]|file|[\*\?]):((//)|(\\\\)))+([\*\?]*[\w\d\ \.:#@/;$*()%~_?\+-=&]*)";

        public WUrlAttribute(bool allowSpace = false)
            : base(GetPattern(allowSpace))
        {
            _allowSpace = allowSpace;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var validationRule = new ModelClientValidationRule
                                     {
                                         ErrorMessage = DongleResource.InvalidUrl,
                                     };

            validationRule.ValidationParameters.Add("pattern", GetPattern(_allowSpace));
            validationRule.ValidationType = "regex";

            return new List<ModelClientValidationRule>{ validationRule };
        }

        private static string GetPattern(bool allowSpace = false)
        {
            return allowSpace ? PatternStrAllowingSpace : PatternStr;
        }
    }
}