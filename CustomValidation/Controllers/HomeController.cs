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
		// todo: test nesting

		[Digits]
		public string DigitsWithCustomAttribute { get; set; }
		
		[DataType("Digits")]
		// todo: test I didn't break other DataTypes
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
	}
}
