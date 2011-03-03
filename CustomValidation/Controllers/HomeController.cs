using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace CustomValidation.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
		[HttpGet]
        public ActionResult Index()
        {
            return View();
        }

		[HttpPost]
		public ActionResult Index(MyModel model)
		{
			if (ModelState.IsValid)
			{
				return View();
			}
			return View(model);
		}

    }
	#region ValidationAttribute
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
	#endregion

	#region DataTypeAdapter
	// This requires code in global.asax:
	//    DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(DataTypeAttribute), typeof(DigitsDataTypeAdapter));
	public class DigitsDataTypeAdapter : DataAnnotationsModelValidator<DataTypeAttribute>
	{
		private static readonly Regex Validator = new Regex(@"^\d+$");
		private const string DigitsErrorMessage = @"The {0} field should only contain digits.";

		public DigitsDataTypeAdapter(ModelMetadata metadata, ControllerContext context, DataTypeAttribute attribute)
			: base(metadata, context, attribute) {}

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
	}
	#endregion

	#region CustomType

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

	[ModelBinder(typeof(SimpleTypeToStringBinder))]
	public class SimpleType<T> : ISimpleType
		where T : SimpleType<T>
	{
		private readonly string _content;

		public SimpleType() { }
		public SimpleType(string content)
		{
			_content = content;
		}

		public static implicit operator string(SimpleType<T> simpleType)
		{
			return simpleType.ToString();
		}

		protected string Content
		{
			get { return _content; }
		}

		public override string ToString()
		{
			return _content;
		}
	}

	public interface ISimpleType{}

	public class SimpleTypeToStringBinder : IModelBinder
	{
		#region IModelBinder Members

		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var objValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ConvertTo(typeof(string));
			var stringValue = (string)objValue;
			return string.IsNullOrEmpty(stringValue) ? null : Activator.CreateInstance(bindingContext.ModelType, stringValue);
		}

		#endregion
	}
	#endregion

	public class MyModel
	{
		// todo: test nesting
		
		[DataType("Digits")]
		// todo: test I didn't break other DataTypes
		public string DigitsWithDataTypeAttribute { get; set; }

		[Digits]
		public string DigitsWithCustomAttribute { get; set; }

		// todo: Figure out how to get display name
		// todo: Fix client-side validation
		public Digits DigitsAsCustomType { get; set; }
	}
}
