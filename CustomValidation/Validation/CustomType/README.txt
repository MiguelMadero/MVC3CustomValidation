CustomValidatableObjectAdapter.cs is a copy of one of the files in the MVC3 source code with a modification to pass the Metadata property name and display name into the validation context so it can be accessed by the validation logic within the Digits class.

It is registered as the default Validatable Object Adapter Factory inside Application_Start().