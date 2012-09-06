using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Dongle.Web.Resources;

namespace Dongle.Web.ModelAttributes
{
    public sealed class WUrlAttribute : RegularExpressionAttribute, IClientValidatable
    {
        private const string PatternStr = @"((https?):((//)|(\\\\))+[\w\d:#@%/;$()~_?\+-=\\\.&]*)";

        public WUrlAttribute()
            : base(PatternStr)
        {
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var validationRule = new ModelClientValidationRule
                                     {
                                         ErrorMessage = DongleResource.InvalidUrl,
                                     };

            validationRule.ValidationParameters.Add("pattern", PatternStr);
            validationRule.ValidationType = "regex";

            return new List<ModelClientValidationRule>{ validationRule };
        }
    }
}