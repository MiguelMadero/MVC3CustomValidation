using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace CustomValidation.Validation.CustomAttribute
{
    public class DigitsAttribute : ValidationAttribute, IClientValidatable
    {

        private static readonly Regex Validator = new Regex(@"^\d+$");
        private const string DigitsErrorMessage = @"The {0} field should only contain digits.";

        public override bool IsValid(object value)
        {
            var strValue = value as string;
            if (!string.IsNullOrEmpty(strValue) && !Validator.IsMatch(strValue))
            {
                return false;
            }
            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(DigitsErrorMessage, name);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.DisplayName ?? metadata.PropertyName),
                ValidationType = "digits"
            };
        }
    }
}