using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;
using WebUtils.Resources;

namespace WebUtils.Mvc.ModelAttributes
{

    public sealed class WRangeAttribute : RangeAttribute, IClientValidatable
    {
        private int _startValue;
        private int _endValue;

        public WRangeAttribute(int startValue, int endValue): base(startValue, endValue)
        {
            _startValue = startValue;
            _endValue = endValue;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(WebUtilsResource.TheValueMustBeBetween, _startValue, _endValue);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(""),
                ValidationType = "range",
            };
            rule.ValidationParameters.Add("min", _startValue);
            rule.ValidationParameters.Add("max", _endValue);
            yield return rule;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return null;

            if (String.IsNullOrEmpty(value.ToString()))
                return null;

           
            var val = Convert.ToInt32(value);
            if (val >= Convert.ToInt32(this.Minimum) && val <= Convert.ToInt32(this.Maximum))
                return null;
           

            return new ValidationResult(
                FormatErrorMessage(this.ErrorMessage)
            );
        }
    }
}