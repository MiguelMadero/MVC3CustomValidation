using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace CustomValidation.Validation.CustomDataTypeAttribute
{
	public class CustomDataTypeAdapter : DataAnnotationsModelValidator<DataTypeAttribute>
	{
		private static readonly Regex DigitsValidator = new Regex(@"^\d+$");
		private const string DigitsErrorMessage = @"The {0} field should only contain digits.";

		public CustomDataTypeAdapter(ModelMetadata metadata, ControllerContext context, DataTypeAttribute attribute)
			: base(metadata, context, attribute) { }

		public override IEnumerable<ModelValidationResult> Validate(object container)
		{
			if (Attribute.CustomDataType == "Digits")
			{
				var strValue = container.GetType().GetProperty(Metadata.PropertyName).GetValue(container, null) as string;
				if (!string.IsNullOrEmpty(strValue) && !DigitsValidator.IsMatch(strValue))
				{
					return new[] {
						new ModelValidationResult
						{
							Message = string.Format(DigitsErrorMessage, Metadata.DisplayName ?? Metadata.PropertyName)
						}
					};
				}
			}

			return base.Validate(container);
		}

		public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
		{
			if (Attribute.CustomDataType == "Digits")
			{
				return new[] {
					new ModelClientValidationRule
					{
						ErrorMessage = string.Format(DigitsErrorMessage, Metadata.DisplayName ?? Metadata.PropertyName),
						ValidationType = "digits"
					}
				};
			}

			return base.GetClientValidationRules();
		}

		/// <summary>
		/// Registers the custom data type adapter with MVC.
		/// </summary>
		/// <remarks>
		/// Must be called from global.asax.cs within Application_Start() for these custom data type validations to work.
		/// </remarks>
		public static void Register()
		{
			DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(DataTypeAttribute), typeof(CustomDataTypeAdapter));
		}
	}
}