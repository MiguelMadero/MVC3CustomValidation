﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace CustomValidation.Validation.CustomType
{
	public class Digits : SimpleType<Digits>, IValidatableObject, IClientValidatable
	{
		private static readonly Regex Validator = new Regex(@"^\d+$");
		private const string ErrorMessage = @"The {0} field should only contain digits.";

		public Digits(string content) : base(content) { }
		public static implicit operator Digits(string value)
		{
			return string.IsNullOrEmpty(value) ? null : new Digits(value);
		}

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (!string.IsNullOrEmpty(Content) && !Validator.IsMatch(Content))
			{
				yield return new ValidationResult(string.Format(ErrorMessage, validationContext.DisplayName));
			}
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			yield return new ModelClientValidationRule
			{
				ErrorMessage = string.Format(ErrorMessage, metadata.DisplayName ?? metadata.PropertyName),
				ValidationType = "digits"
			};
		}
	}
}