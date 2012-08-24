using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Dongle.Web.Resources;

namespace Dongle.Web.ModelAttributes
{
    public sealed class WHexadecimalAttribute : RegularExpressionAttribute, IClientValidatable
    {
        private static string PatternStr;


        public WHexadecimalAttribute(int maxLenght = 8)
            : base("^[a-fA-F0-9]{" + maxLenght + "}$")
        {

        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var validationRule = new ModelClientValidationRule
            {
                ErrorMessage = DongleResource.InvalidHexadecimal,
            };

            validationRule.ValidationParameters.Add("pattern", PatternStr);
            validationRule.ValidationType = "regex";

            return new List<ModelClientValidationRule> { validationRule };
        }
    }
}