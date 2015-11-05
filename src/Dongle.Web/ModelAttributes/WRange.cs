using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Dongle.Resources;

namespace Dongle.Web.ModelAttributes
{

    public sealed class WRangeAttribute : RangeAttribute, IClientValidatable
    {
        private readonly int _startValue;
        private readonly int _endValue;

        public WRangeAttribute(int startValue, int endValue): base(startValue, endValue)
        {
            _startValue = startValue;
            _endValue = endValue;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(DongleResource.TheValueMustBeBetween, _startValue, _endValue);
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
            if (val >= Convert.ToInt32(Minimum) && val <= Convert.ToInt32(Maximum))
                return null;
           

            return new ValidationResult(
                FormatErrorMessage(ErrorMessage)
            );
        }
    }
}