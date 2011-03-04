/* ****************************************************************************
 *
 * Copyright (c) Microsoft Corporation. All rights reserved.
 *
 * This software is subject to the Microsoft Public License (Ms-PL). 
 * A copy of the license can be found in the license.htm file included 
 * in this distribution.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * ***************************************************************************/


namespace CustomValidation.Validation.CustomType {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Mvc.Resources;

    public class CustomValidatableObjectAdapter : ModelValidator {
        public CustomValidatableObjectAdapter(ModelMetadata metadata, ControllerContext context)
            : base(metadata, context) {
        }

        public override IEnumerable<ModelValidationResult> Validate(object container) {
            // NOTE: Container is never used here, because IValidatableObject doesn't give you
            // any way to get access to your container.

            object model = Metadata.Model;
            if (model == null) {
                return Enumerable.Empty<ModelValidationResult>();
            }

            IValidatableObject validatable = model as IValidatableObject;
            if (validatable == null) {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.CurrentCulture,
                        "The model object inside the metadata claimed to be compatible with {0}, but was actually {1}.",
                        typeof(IValidatableObject).FullName,
                        model.GetType().FullName
                    )
                );
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

        private IEnumerable<ModelValidationResult> ConvertResults(IEnumerable<ValidationResult> results) {
            foreach (ValidationResult result in results) {
                if (result != ValidationResult.Success) {
                    if (result.MemberNames == null || !result.MemberNames.Any()) {
                        yield return new ModelValidationResult { Message = result.ErrorMessage };
                    }
                    else {
                        foreach (string memberName in result.MemberNames) {
                            yield return new ModelValidationResult { Message = result.ErrorMessage, MemberName = memberName };
                        }
                    }
                }
            }
        }
    }
}
