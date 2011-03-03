using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CustomValidation.Validation.CustomType
{
	public class MetadataProvider : DataAnnotationsModelMetadataProvider
	{
		protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType,
			Func<object> modelAccessor, Type modelType, string propertyName)
		{
			// Default metadata implementations
			var metadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);

			// Populate the display name for a simple type model
			if (typeof(ISimpleType).IsAssignableFrom(metadata.ModelType))
			{
				metadata.DisplayName = metadata.PropertyName;
			}

			return metadata;
		}

		public static void Register()
		{
			ModelMetadataProviders.Current = new MetadataProvider();
		}
	}
}