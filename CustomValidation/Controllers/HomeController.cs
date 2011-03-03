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
