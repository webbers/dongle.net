using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Dongle.Resources;
using Dongle.Serialization;

namespace Dongle.Web.ModelAttributes
{

    public sealed class WDateRangeAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly DateTime _minDate = new DateTime(1900, 1, 1);
        private DateTime? _maxDate = DateTime.Now;
        private static bool _useMaxDate = false;

        public WDateRangeAttribute()
        {            
        }
        
        public WDateRangeAttribute(DateTime? minDate = null, DateTime? maxDate = null)
        {
            if (maxDate.HasValue)
            {
                _maxDate = maxDate.Value;
                _useMaxDate = true;
            }
            else
            {
                _maxDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
            }

            if (minDate.HasValue)
            {
                _minDate = minDate.Value;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(DongleResource.TheDatePeriodMustBeBetween, _minDate, _useMaxDate ? _maxDate : DateTime.Now.Date.AddDays(1).AddSeconds(-1));
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(""),
                ValidationType = "daterange",
            };
            rule.ValidationParameters.Add("max", JsonSimpleSerializer.SerializeToString(_useMaxDate ? _maxDate : DateTime.Now.Date.AddDays(1).AddSeconds(-1)));
            rule.ValidationParameters.Add("min", JsonSimpleSerializer.SerializeToString(_minDate));

            yield return rule;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
                return null;

            var val = Convert.ToDateTime(value);
            if (val >= _minDate && val <= (_useMaxDate ? _maxDate : DateTime.Now.Date.AddDays(1).AddSeconds(-1)))
            {
                return null;
            }
            return new ValidationResult(FormatErrorMessage(ErrorMessage));
        }
    }
}