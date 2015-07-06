using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Dongle.Resources;

namespace Dongle.Web.ModelAttributes
{
    public sealed class WHexadecimalAttribute : RegularExpressionAttribute, IClientValidatable
    {
        private readonly uint _octetCount;
        private readonly uint _maxOctetCount;

        public WHexadecimalAttribute(uint octetCount)
            : base("^([a-fA-F0-9]{8}){" + (octetCount == 0 ? 1 : octetCount) + "}$")
        {
            _octetCount = octetCount;
        }

        public WHexadecimalAttribute(uint minOctetCount, uint maxOctetCount)
            : base("^([a-fA-F0-9]{8}){" + (minOctetCount == 0 ? 1 : minOctetCount) + "," + (maxOctetCount == 0 ? 1 : maxOctetCount) + "}$")
        {
            _maxOctetCount = maxOctetCount;
            _octetCount = minOctetCount;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var validationRule = new ModelClientValidationRule
            {
                ErrorMessage = _maxOctetCount != 0 ? string.Format(DongleResource.InvalidHexadecimalRange, _octetCount, _maxOctetCount) : string.Format(DongleResource.InvalidHexadecimal, _octetCount),
            };

            validationRule.ValidationParameters.Add("pattern", Pattern);
            validationRule.ValidationType = "regex";

            return new List<ModelClientValidationRule> { validationRule };
        }
    }
}