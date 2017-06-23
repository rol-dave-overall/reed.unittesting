----------------------------------------------------------
---------- reed unit testing xml transformation ----------
----------------------------------------------------------

Use this package in your unit testing projects to allow xml transformations of files to be validated during the 
build process. The package provides a simple validator that can be used in a unit test to check a transformation
file is valid and will execute without any issues, below is an example of its usage.

----Example Usage----
var transformValidator = new XmlTransformationValidator();
Assert.IsTrue(transformValidator.Validate("web.config", "web.dev.config"), validator.ErrorLog);

The exmaple validate that the "web.dev.config" transformation file will apply to the "web.config" file and fail 
when it doesnt, errors are reported by accessing the "ErrorLog" property.

----Remarks----
To be able to use the validator in your unit test projects you will need to ensure that the config and transformation
files are copied into the compiled bin folder so that the validator can access them.
