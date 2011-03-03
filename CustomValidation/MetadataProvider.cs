using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CustomValidation.Controllers;

namespace CustomValidation
{
	public class MetadataProvider : DataAnnotationsModelMetadataProvider
	{
		protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType,
			Func<object> modelAccessor, Type modelType, string propertyName)
		{
			// Default metadata implementations
			var metadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);

			// Auto-sentence case for display name
			if (typeof(ISimpleType).IsAssignableFrom(metadata.ModelType))
			{
				metadata.DisplayName = metadata.PropertyName;
			}

			return metadata;
		}
	}
}