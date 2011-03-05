ASP.NET MVC3 Custom Validation
==============================

This project is an experiment to find out all of the different ways that data type validation can be performed in ASP.NET MVC3. The documentation for validation in MVC 3 is hard to find and often hard to understand.

By data type validation we mean things like email address, digits, date string etc. as opposed to the semantics of the data (e.g. minimum of 2 characters, between 10 and 15, etc.).

So far we have found 3 ways to perform data type validation:

* Custom validation attributes; e.g.
		
		[Digits]
		public string DigitsWithCustomAttribute { get; set; }

* Custom Data Type attribute; e.g.
		
		[DataType("Digits")]
		public string DigitsWithDataTypeAttribute { get; set; }

* Custom type; e.g.
		
		public Digits DigitsAsCustomType { get; set; }

This project presents the example of validating that a model property is a string consisting of only digits both server-side and (unobtrusively) client-side.

The controller and corresponding ViewModel with the properties that have the attributes is at Controllers\HomeController.cs. The implementations for each of the validation implementations can be found at:

* Validation\CustomAttribute\
* Validation\CustomDataTypeAttribute\
* Validation\CustomType\