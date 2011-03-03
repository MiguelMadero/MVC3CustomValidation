using System;
using System.Web.Mvc;

namespace CustomValidation.Validation.CustomType
{
	/// <summary>
	/// Allows custom classes to pretend they are strings.
	/// </summary>
	/// <remarks>
	/// This class requires the data type inheriting it to have the following as a minimum:
	/// <pre><code>
	/// public class Digits : SimpleType&lt;CustomDataTypeName%gt;
	///	{
	///		public CustomDataTypeName(string content) : base(content) { }
	///		public static implicit operator CustomDataTypeName(string value)
	///		{
	///			return string.IsNullOrEmpty(value) ? null : new CustomDataTypeName(value);
	///		}
	/// }
	/// </code></pre>
	/// </remarks>
	/// <typeparam name="T">The custom data type you are trying to pretend is a string</typeparam>
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

	/// <summary>
	/// Dummy interface to allow calls to IsAssignableFrom to work on SimpleType&lt;&gt; objects.
	/// </summary>
	public interface ISimpleType { }

	/// <summary>
	/// Allow SimpleType objects to be bound to a string value when being bound by a Model Binder.
	/// </summary>
	public class SimpleTypeToStringBinder : IModelBinder
	{
		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var objValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ConvertTo(typeof(string));
			var stringValue = (string) objValue;
			return string.IsNullOrEmpty(stringValue) ? null : Activator.CreateInstance(bindingContext.ModelType, stringValue);
		}
	}
}