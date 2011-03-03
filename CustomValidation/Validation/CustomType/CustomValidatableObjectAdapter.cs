using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace CustomValidation.Validation.CustomType
{
	public class CustomValidatableObjectAdapter : ModelValidator
	{
		public CustomValidatableObjectAdapter(ModelMetadata metadata, ControllerContext context)
			: base(metadata, context)
		{}

		public override IEnumerable<ModelValidationResult> Validate(object container)
		{
			object model = Metadata.Model;
			if (model == null)
			{
			   return Enumerable.Empty<ModelValidationResult>();
			}

			var validatable = model as IValidatableObject;
			if (validatable == null)
			{
				throw new InvalidOperationException("The model wasn't an IValidatableObject");
			}

			// Get the Metadata from the property being validated and pass that through as the ValidationContext
			//	this differs from the default within MVC where the ValidationContext is the context
			//	of the type being validated rather than the property using that type
			var validationContext = new ValidationContext(validatable, null, null)
				{
					DisplayName = Metadata.DisplayName ?? Metadata.PropertyName,
					MemberName = Metadata.PropertyName
				};

			return ConvertResults(validatable.Validate(validationContext));
		}

		private static IEnumerable<ModelValidationResult> ConvertResults(IEnumerable<ValidationResult> results)
		{
			foreach (ValidationResult result in results)
			{
				if (result != ValidationResult.Success)
				{
					if (result.MemberNames == null || !result.MemberNames.Any())
					{
						yield return new ModelValidationResult { Message = result.ErrorMessage };
					}
					else
					{
						foreach (string memberName in result.MemberNames)
						{
							yield return new ModelValidationResult { Message = result.ErrorMessage, MemberName = memberName };
						}
					}
				}
			}
		}

		public static void Register()
		{
			DataAnnotationsModelValidatorProvider.RegisterDefaultValidatableObjectAdapterFactory((m, c) => new CustomValidatableObjectAdapter(m, c));
		}
	}
}
