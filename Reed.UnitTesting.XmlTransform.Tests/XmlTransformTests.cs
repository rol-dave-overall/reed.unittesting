using System;
using System.IO;
using NUnit.Framework;
using Reed.UnitTesting.Validators;

namespace Reed.UnitTesting.XmlTransform.Tests
{
    [TestFixture]
    public class XmlTransformTests
    {
        public TestContext TestContext { get; set; }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowExceptionWhenSourceIsNull()
        {
            var newValidator = new XmlTransformationValidator();

            newValidator.Validate(null, "test");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowExceptionWhenSourceIsEmpty()
        {
            var newValidator = new XmlTransformationValidator();

            newValidator.Validate(string.Empty, "test");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowExceptionWhenTransformationIsNull()
        {
            var newValidator = new XmlTransformationValidator();

            newValidator.Validate("test", null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowExceptionWhenTransformationIsEmpty()
        {
            var newValidator = new XmlTransformationValidator();

            newValidator.Validate("test", string.Empty);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowExceptionWhenSourceNotFound()
        {
            var newValidator = new XmlTransformationValidator();

            newValidator.Validate("test", string.Empty);
        }

        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ThrowExceptionWhenTransformationNotFound()
        {
            var newValidator = new XmlTransformationValidator();

            newValidator.Validate("test", "test");
        }

        [Test]
        public void Validation_AttibuteFormatting()
        {
            Transform_TestRunner_ExpectSuccess(Properties.Resources.AttributeFormatting_source, Properties.Resources.AttributeFormatting_transform, Properties.Resources.AttributeFormatting);
        }

        [Test]
        public void Validation_TagFormatting()
        {
            Transform_TestRunner_ExpectSuccess(Properties.Resources.TagFormatting_source, Properties.Resources.TagFormatting_transform, Properties.Resources.TagFormatting);
        }

        [Test]
        public void Validation_ErrorAndWarning()
        {
            Transform_TestRunner_ExpectFail(Properties.Resources.WarningsAndErrors_source, Properties.Resources.WarningsAndErrors_transform, Properties.Resources.WarningsAndErrors);
        }

        [Test]
        public void Validation_ErrorAndWarning_DualLogs()
        {
            Transform_TestRunner_ExpectFail(Properties.Resources.WarningsAndErrors_source, Properties.Resources.WarningsAndErrors_transform, Properties.Resources.WarningsAndErrors);

            string src = CreateATestFile("source.config", Properties.Resources.WarningsAndErrors_source);
            string transformFile = CreateATestFile("transform.config", Properties.Resources.WarningsAndErrors_transform);
            XmlTransformationValidator validator = new XmlTransformationValidator();

            bool succeed = validator.Validate(src, transformFile, false);

            //test
            Assert.AreEqual(false, succeed);
            Assert.AreNotEqual(validator.ErrorLog, string.Empty);
            Assert.AreNotEqual(validator.WarningLog, string.Empty);
        }

        [Test]
        public void Validation_ErrorAndWarning_SingleLog()
        {
            Transform_TestRunner_ExpectFail(Properties.Resources.WarningsAndErrors_source, Properties.Resources.WarningsAndErrors_transform, Properties.Resources.WarningsAndErrors);

            string src = CreateATestFile("source.config", Properties.Resources.WarningsAndErrors_source);
            string transformFile = CreateATestFile("transform.config", Properties.Resources.WarningsAndErrors_transform);
            XmlTransformationValidator validator = new XmlTransformationValidator();

            bool succeed = validator.Validate(src, transformFile);

            //test
            Assert.AreEqual(false, succeed);
            Assert.AreNotEqual(validator.ErrorLog, string.Empty);
            Assert.AreEqual(validator.WarningLog, string.Empty);
        }

        [Test]
        public void Validation_ExpectSuccess()
        {
            Transform_TestRunner_ExpectSuccess(Properties.Resources.Web, Properties.Resources.Web_Release, string.Empty);
        }

        private void Transform_TestRunner_ExpectSuccess(string source, string transform, string expectedLog)
        {
            string src = CreateATestFile("source.config", source);
            string transformFile = CreateATestFile("transform.config", transform);
            XmlTransformationValidator validator = new XmlTransformationValidator();

            bool succeed = validator.Validate(src, transformFile, false);

            //test
            Assert.AreEqual(true, succeed);
            Assert.AreEqual(string.Empty, validator.ErrorLog);

            if (expectedLog != string.Empty)
            {
                CompareMultiLines(expectedLog, validator.VerboseLog);
            }
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
            string folder = Path.Combine(TestContext.CurrentContext.WorkDirectory, TestContext.CurrentContext.Test.Name);
            Directory.CreateDirectory(folder);
            string file = Path.Combine(folder, filename);
            return file;
        }
    }
}