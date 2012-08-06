using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WebUtils.Resources;

namespace WebUtils.Mvc.ModelAttributes
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
                ErrorMessage = WebUtilsResource.InvalidHexadecimal,
            };

            validationRule.ValidationParameters.Add("pattern", PatternStr);
            validationRule.ValidationType = "regex";

            return new List<ModelClientValidationRule> { validationRule };
        }
    }
}