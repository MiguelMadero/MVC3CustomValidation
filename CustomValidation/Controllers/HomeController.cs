using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CustomValidation.Validation.CustomAttribute;
using CustomValidation.Validation.CustomType;

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

	public class MyModel
	{

		[Digits]
		public string DigitsWithCustomAttribute { get; set; }
		
		[DataType("Digits")]
		public string DigitsWithDataTypeAttribute { get; set; }

		// todo: Fix client-side validation
		public Digits DigitsAsCustomType { get; set; }

		// Test the other DataTypes still work:
		
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		// Test DisplayName works:

		[Digits]
		[DisplayName("Digits 1")]
		public string CustomAttributeWithLabel { get; set; }

		[DataType("Digits")]
		[DisplayName("Digits 2")]
		public string DataTypeAttributeWithLabel { get; set; }

		[DisplayName("Digits 3")]
		public Digits CustomTypeWithLabel { get; set; }

		// Test nesting:

		public MyNestedModel MyNestedModel { get; set; }

	}

	public class MyNestedModel
	{
		[Digits]
		public string NestedCustomAttribute { get; set; }

		[DataType("Digits")]
		public string NestedDataTypeAttribute { get; set; }

		public Digits NestedCustomType { get; set; }
	}
}
