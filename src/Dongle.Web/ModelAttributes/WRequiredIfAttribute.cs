using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Dongle.Resources;

namespace Dongle.Web.ModelAttributes
{
    public class WRequiredIfAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly RequiredAttribute _requiredAttribute = new RequiredAttribute();

        public string ConditionalPropertyId { get; set; }
        public object ConditionalValue { get; set; }

        public WRequiredIfAttribute(string conditionalPropertyId, object conditionalValue)
            : this(conditionalPropertyId, conditionalValue, null)
        {
        }

        public WRequiredIfAttribute(string conditionalPropertyId, object conditionalValue, string errorMessage)
            : base(errorMessage)
        {
            ConditionalPropertyId = conditionalPropertyId;
            ConditionalValue = conditionalValue;
        }

        //Validação do Server Side
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (MustValidate(value, ConditionalPropertyId, ConditionalValue, validationContext))
            {
                if (!_requiredAttribute.IsValid(value))
                {
                    var msg = ErrorMessage ?? DongleResource.ThisFieldIsRequired;

                    return new ValidationResult(msg, new[] { validationContext.MemberName });
                }
            }

            return ValidationResult.Success;
        }

        //Validação do Client Side
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var msg = ErrorMessage ?? DongleResource.ThisFieldIsRequired;

            var rule = new ModelClientValidationRule
                           {
                               ErrorMessage = msg,
                               ValidationType = "wrequiredif",
                           };

            var conditionalProperty = GetConditionalPropertyRealId(metadata, context as ViewContext);

            var conditionalValue = (ConditionalValue ?? "").ToString();
            if (ConditionalValue != null && ConditionalValue is bool)
                conditionalValue = conditionalValue.ToLower();

            rule.ValidationParameters.Add("conditionalproperty", conditionalProperty);
            rule.ValidationParameters.Add("conditionalvalue", conditionalValue);

            yield return rule;
        }

        private string GetConditionalPropertyRealId(ModelMetadata metadata, ViewContext viewContext)
        {
            var conditionalPropertyId = viewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(ConditionalPropertyId);
            var thisField = metadata.PropertyName + "_";
            if (conditionalPropertyId.StartsWith(thisField))
            {
                conditionalPropertyId = conditionalPropertyId.Substring(thisField.Length);
            }
            return conditionalPropertyId;
        }

        protected object GetConditionalPropertyValue(string conditionalProperty, ValidationContext validationContext)
        {
            var containerType = validationContext.ObjectInstance.GetType();
            var property = containerType.GetProperty(conditionalProperty);

            if (property == null)
            {
                throw new MissingMemberException(containerType.Name, conditionalProperty);
            }

            var conditionalPropertyValue = property.GetValue(validationContext.ObjectInstance, null);
            return conditionalPropertyValue;
        }

        protected bool MustValidate(object value, string conditionalProperty, object conditionalValue, ValidationContext validationContext)
        {
            var propertyValue = GetConditionalPropertyValue(conditionalProperty, validationContext);

            if(propertyValue == null && conditionalValue == null)
            {
                return true;
            }

            if(propertyValue != null && propertyValue.Equals(conditionalValue))
            {
                return true;
            }
            return false;
        }
    }
}