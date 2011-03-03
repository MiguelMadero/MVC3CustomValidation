using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace CustomValidation.Validation.CustomDataTypeAttribute
{
	public class DigitsDataTypeAdapter : DataAnnotationsModelValidator<DataTypeAttribute>
	{
		private static readonly Regex Validator = new Regex(@"^\d+$");
		private const string DigitsErrorMessage = @"The {0} field should only contain digits.";

		public DigitsDataTypeAdapter(ModelMetadata metadata, ControllerContext context, DataTypeAttribute attribute)
			: base(metadata, context, attribute) { }

		public override IEnumerable<ModelValidationResult> Validate(object container)
		{
			//if (Attribute.CustomDataType != "Digits")
			//	return base.Validate(container);

			var strValue = container.GetType().GetProperty(Metadata.PropertyName).GetValue(container, null) as string;
			if (!string.IsNullOrEmpty(strValue) && !Validator.IsMatch(strValue))
			{
				yield return new ModelValidationResult
				{
					Message = string.Format(DigitsErrorMessage, Metadata.DisplayName ?? Metadata.PropertyName)
				};
			}
		}

		public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
		{
			yield return new ModelClientValidationRule
			{
				ErrorMessage = string.Format(DigitsErrorMessage, Metadata.DisplayName ?? Metadata.PropertyName),
				ValidationType = "digits"
			};
		}

		/// <summary>
		/// Registers the custom data type adapter with MVC.
		/// </summary>
		/// <remarks>
		/// Must be called from global.asax.cs within Application_Start() for this attribute to work.
		/// You will need to do something similar for each data type adapter that you create.
		/// </remarks>
		public static void Register()
		{
			DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(DataTypeAttribute), typeof(DigitsDataTypeAdapter));
		}
	}
}