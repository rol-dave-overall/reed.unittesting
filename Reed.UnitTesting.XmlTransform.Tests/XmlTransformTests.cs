using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reed.UnitTesting.Validators;

namespace Reed.UnitTesting.XmlTransform.Tests
{
    [TestClass]
    public class XmlTransformTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowExceptionWhenSourceIsNull()
        {
            var newValidator = new XmlTransformationValidator();

            newValidator.Validate(null, "test");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowExceptionWhenSourceIsEmpty()
        {
            var newValidator = new XmlTransformationValidator();

            newValidator.Validate(string.Empty, "test");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowExceptionWhenTransformationIsNull()
        {
            var newValidator = new XmlTransformationValidator();

            newValidator.Validate("test", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowExceptionWhenTransformationIsEmpty()
        {
            var newValidator = new XmlTransformationValidator();

            newValidator.Validate("test", string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowExceptionWhenSourceNotFound()
        {
            var newValidator = new XmlTransformationValidator();

            newValidator.Validate("test", string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ThrowExceptionWhenTransformationNotFound()
        {
            var newValidator = new XmlTransformationValidator();

            newValidator.Validate("test", "test");
        }

        [TestMethod]
        public void Validation_AttibuteFormatting()
        {
            Transform_TestRunner_ExpectSuccess(Properties.Resources.AttributeFormatting_source, Properties.Resources.AttributeFormatting_transform);
        }

        [TestMethod]
        public void Validation_TagFormatting()
        {
            Transform_TestRunner_ExpectSuccess(Properties.Resources.TagFormatting_source, Properties.Resources.TagFormatting_transform);
        }

        [TestMethod]
        public void Validation_ErrorAndWarning()
        {
            Transform_TestRunner_ExpectFail(Properties.Resources.WarningsAndErrors_source, Properties.Resources.WarningsAndErrors_transform, Properties.Resources.WarningsAndErrors);
        }

        [TestMethod]
        public void Validation_ExpectSuccess()
        {
            Transform_TestRunner_ExpectSuccess(Properties.Resources.Web, Properties.Resources.Web_Release);
        }

        private void Transform_TestRunner_ExpectSuccess(string source, string transform)
        {
            string src = CreateATestFile("source.config", source);
            string transformFile = CreateATestFile("transform.config", transform);
            XmlTransformationValidator validator = new XmlTransformationValidator();

            bool succeed = validator.Validate(src, transformFile, false);

            //test
            Assert.AreEqual(true, succeed);
            Assert.AreEqual(string.Empty, validator.ErrorLog);
        }

        private void Transform_TestRunner_ExpectFail(string source, string transform, string expectedLog)
        {
            string src = CreateATestFile("source.config", source);
            string transformFile = CreateATestFile("transform.config", transform);
            XmlTransformationValidator validator = new XmlTransformationValidator();

            bool succeed = validator.Validate(src, transformFile, false);

            //test
            Assert.AreEqual(false, succeed);
            CompareMultiLines(expectedLog, validator.VerboseLog);
        }

        private string CreateATestFile(string filename, string contents)
        {
            string file = GetTestFilePath(filename);
            File.WriteAllText(file, contents);
            return file;
        }

        private void CompareMultiLines(string baseline, string result)
        {
            string[] baseLines = baseline.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            string[] resultLines = result.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            for (int i = 0; i < baseLines.Length; i++)
            {
                Assert.AreEqual(baseLines[i], resultLines[i], $"line {i} at baseline file is not matched");
            }
        }

        private string GetTestFilePath(string filename)
        {
            string folder = Path.Combine(this.TestContext.TestDeploymentDir, this.TestContext.TestName);
            Directory.CreateDirectory(folder);
            string file = Path.Combine(folder, filename);
            return file;
        }
    }
}