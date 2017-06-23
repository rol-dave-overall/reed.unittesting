using System.IO;
using Microsoft.Web.XmlTransform;

namespace Reed.UnitTesting.Validators
{
    /// <summary>
    /// Validates xml transformations for testing purposes.
    /// </summary>
    public class XmlTransformationValidator
    {
        #region Fields

        /// <summary>
        /// Contains a reference to the <see cref="XmlTransformationLogger"/> used by this instance.
        /// </summary>
        private readonly XmlTransformationLogger transformationLogger = new XmlTransformationLogger(true, true);

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets a value containing all the errors logged for the last validation run.
        /// </summary>
        /// <value>A list of the logged errors for the last validation run.</value>
        public string ErrorLog => this.transformationLogger.ErrorLog;

        /// <summary>
        /// Gets a value containing all the logged information for the last validation run.
        /// </summary>
        /// <value>A list of the all the logged for the last validation run.</value>
        public string VerboseLog => this.transformationLogger.VerboseLog;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Validates the specified <paramref name="transformation"/> against the specififed <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The name and location of the source file to apply the transformation against.</param>
        /// <param name="transformation">The name and location of the transformation file to apply against the source.</param>
        /// <returns><b>true</b> if the transformation was successful; otherwise <b>false</b>.</returns>
        /// <remarks>The <see cref="ErrorLog"/> and <see cref="VerboseLog"/> properties can be used to determine the steps the transformation has done and any errors that occurred.</remarks>
        public bool Validate(string source, string transformation)
        {
            this.transformationLogger.Reset();
            this.transformationLogger.LogMessage(MessageType.Verbose, "Applying transformations '{0}' on file '{1}'...", transformation, source);

            var sourceDocument = new XmlTransformableDocument { PreserveWhitespace = true };
            sourceDocument.Load(source);

            var transformFile = File.ReadAllText(transformation);
            var xmlTransformation = new XmlTransformation(transformFile, false, this.transformationLogger);

            if (!xmlTransformation.Apply(sourceDocument) || this.transformationLogger.HasLoggedErrors)
            {
                this.transformationLogger.LogMessage(MessageType.Normal, "Error while applying transformations '{0}'.", transformation,
                    source);
                return false;
            }
            return true;
        }

        #endregion Methods
    }
}