using Microsoft.Web.XmlTransform;
using System;
using System.Text;

namespace Reed.UnitTesting.Validators
{
    /// <summary>
    /// Used as the logger for the <see cref="XmlTransformationValidator"/> and implements the <see cref="IXmlTransformationLogger"/> interface.
    /// </summary>
    internal class XmlTransformationLogger : IXmlTransformationLogger
    {
        #region Fields

        /// <summary>
        /// Provides the storage for all logged error messages.
        /// </summary>
        private readonly StringBuilder errorMessageStore = new StringBuilder();

        /// <summary>
        /// Provides the storage for all logged warning messages.
        /// </summary>
        private readonly StringBuilder warningMessageStore = new StringBuilder();

        /// <summary>
        /// Provides the storage for all logged messages.
        /// </summary>
        private readonly StringBuilder verboseMessageStore = new StringBuilder();

        private const string IndentStringPiece = "  ";

        /// <summary>
        /// Indicates whether stack trace information should be logged for exceptions.
        /// </summary>
        private readonly bool stackTrace;

        /// <summary>
        /// Indicates if warning messages should be treated as errors
        /// </summary>
        private readonly bool treatWarningsAsErrors;

        /// <summary>
        /// Backing store for the <see cref="IndentLevel"/> property.
        /// </summary>
        private int indentLevel;

        /// <summary>
        /// Backing store for the <see cref="IndentString"/> property.
        /// </summary>
        private string indentString;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets a string representation of the logged errors.
        /// </summary>
        /// <value>The logged errors.</value>
        public string ErrorLog => this.errorMessageStore.ToString();

        /// <summary>
        /// Gets a string representation of the logged warnings.
        /// </summary>
        /// <value>The logged warnings.</value>
        public string WarningLog => this.warningMessageStore.ToString();

        /// <summary>
        /// Gets a string representation of the verbose log.
        /// </summary>
        /// <value>The verbose log.</value>
        public string VerboseLog => this.verboseMessageStore.ToString();

        /// <summary>
        /// Gets a value indicating whether or not an error message has been logged in the current instance.
        /// </summary>
        /// <value><b>true</b> if an error messages has been logged; otherwise <b>false</b>.</value>
        public bool HasLoggedErrors => this.errorMessageStore.Length > 0;

        /////// <summary>
        /////// Gets a value indicating whether or not an error message has been logged in the current instance.
        /////// </summary>
        /////// <value><b>true</b> if an error messages has been logged; otherwise <b>false</b>.</value>
        ////public bool HasLoggedWarnings => this.warningMessageStore.Length > 0;

        private int IndentLevel
        {
            get => this.indentLevel;
            set
            {
                if (this.indentLevel != value)
                {
                    this.indentLevel = value;
                    this.indentString = null;
                }
            }
        }

        private string IndentString
        {
            get
            {
                if (this.indentString == null)
                {
                    this.indentString = string.Empty;

                    for (var i = 0; i < this.indentLevel; i++)
                    {
                        this.indentString += IndentStringPiece;
                    }
                }
                return this.indentString;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="XmlTransformationLogger"/> with the specified values.
        /// </summary>
        /// <param name="stackTrace"><b>true</b> if stack trace information should be logged for exceptions; otherwise <b>false</b>.</param>
        /// <param name="treatWarningsAsErrors"><b>true</b> if warning messages should be treated as errors; otherwise <b>false</b>.</param>
        public XmlTransformationLogger(bool stackTrace, bool treatWarningsAsErrors)
        {
            this.stackTrace = stackTrace;
            this.treatWarningsAsErrors = treatWarningsAsErrors;
        }

        #endregion Constructors

        #region IXmlTransformationLogger Implementation

        void IXmlTransformationLogger.EndSection(string message, params object[] messageArgs)
        {
            ((IXmlTransformationLogger)this).EndSection(MessageType.Normal, message, messageArgs);
        }

        void IXmlTransformationLogger.EndSection(MessageType type, string message, params object[] messageArgs)
        {
            if (this.IndentLevel > 0)
            {
                this.IndentLevel--;
            }
            ((IXmlTransformationLogger)this).LogMessage(type, message, messageArgs);
        }

        void IXmlTransformationLogger.LogError(string message, params object[] messageArgs)
        {
            //this.LogError(message, messageArgs);
            this.LogError(null, 0, 0, message, messageArgs);
        }

        void IXmlTransformationLogger.LogError(string file, string message, params object[] messageArgs)
        {
            ((IXmlTransformationLogger)this).LogError(file, 0, 0, message, messageArgs);
        }

        void IXmlTransformationLogger.LogError(string file, int lineNumber, int linePosition, string message,
            params object[] messageArgs)
        {
            this.LogError(file, lineNumber, linePosition, string.Format(message, messageArgs));
        }

        void IXmlTransformationLogger.LogErrorFromException(Exception ex)
        {
            this.LogErrorFromException(ex, this.stackTrace, this.stackTrace, null);
        }

        void IXmlTransformationLogger.LogErrorFromException(Exception ex, string file)
        {
            this.LogErrorFromException(ex, this.stackTrace, this.stackTrace, file);
        }

        void IXmlTransformationLogger.LogErrorFromException(Exception ex, string file, int lineNumber, int linePosition)
        {
            var message = ex.Message;
            if (this.stackTrace)
            {
                var stringBuilder = new StringBuilder();
                for (var ex2 = ex; ex2 != null; ex2 = ex2.InnerException)
                {
                    stringBuilder.AppendFormat("{0} : {1}", ex2.GetType().Name, ex2.Message);
                    stringBuilder.AppendLine();
                    if (!string.IsNullOrEmpty(ex2.StackTrace))
                    {
                        stringBuilder.Append(ex2.StackTrace);
                    }
                }
                message = stringBuilder.ToString();
            }
            ((IXmlTransformationLogger)this).LogError(file, lineNumber, linePosition, message);
        }

        void IXmlTransformationLogger.LogMessage(string message, params object[] messageArgs)
        {
            ((IXmlTransformationLogger)this).LogMessage(MessageType.Normal, message, messageArgs);
        }

        void IXmlTransformationLogger.LogMessage(MessageType type, string message, params object[] messageArgs)
        {
            this.LogMessage(type, string.Concat(this.IndentString, message), messageArgs);
        }

        void IXmlTransformationLogger.LogWarning(string message, params object[] messageArgs)
        {
            if (this.treatWarningsAsErrors)
            {
                this.LogWarning(null, 0, 0, message, messageArgs);
                //this.LogWarning(message, messageArgs);
            }
            else
            {
                this.LogError(null, 0, 0, message, messageArgs);
                //this.LogError(message, messageArgs);
            }
        }

        void IXmlTransformationLogger.LogWarning(string file, string message, params object[] messageArgs)
        {
            if (this.treatWarningsAsErrors)
            {
                ((IXmlTransformationLogger)this).LogError(file, 0, 0, message, messageArgs);
            }
            else
            {
                ((IXmlTransformationLogger)this).LogWarning(file, 0, 0, message, messageArgs);
            }
        }

        void IXmlTransformationLogger.LogWarning(string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
        {
            if (this.treatWarningsAsErrors)
            {
                this.LogError(file, lineNumber, linePosition, string.Format(message, messageArgs));
            }
            else
            {
                this.LogWarning(file, lineNumber, linePosition, string.Format(message, messageArgs));
            }
        }

        void IXmlTransformationLogger.StartSection(string message, params object[] messageArgs)
        {
            ((IXmlTransformationLogger)this).StartSection(MessageType.Normal, message, messageArgs);
        }

        void IXmlTransformationLogger.StartSection(MessageType type, string message, params object[] messageArgs)
        {
            ((IXmlTransformationLogger)this).LogMessage(type, message, messageArgs);
            this.IndentLevel++;
        }

        #endregion IXmlTransformationLogger Implementation

        #region Methods

        /// <summary>Logs a message with the specified string and importance.</summary>
        /// <param name="importance">One of the enumeration values that specifies the importance of the message.</param>
        /// <param name="message">The message.</param>
        /// <param name="messageArgs">The arguments for formatting the message.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="message" /> is null.</exception>
        internal void LogMessage(MessageType importance, string message, params object[] messageArgs)
        {
            this.verboseMessageStore.AppendLine(string.Format(message, messageArgs));
        }

        /////// <summary>Logs an error with the specified message.</summary>
        /////// <param name="message">The message.</param>
        /////// <param name="messageArgs">Optional arguments for formatting the message string.</param>
        /////// <exception cref="T:System.ArgumentNullException">
        ///////   <paramref name="message" /> is null.</exception>
        ////private void LogError(string message, params object[] messageArgs)
        ////{
        ////    this.LogError(null, 0, 0, message, messageArgs);
        ////}

        /// <summary>Logs an error using the specified message and other error details.</summary>
        /// <param name="file">The path to the file containing the error.</param>
        /// <param name="lineNumber">The line in the file where the error occurs.</param>
        /// <param name="columnNumber">The column in the file where the error occurs.</param>
        /// <param name="message">The message.</param>
        /// <param name="messageArgs">Optional arguments for formatting the message string.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="message" /> is null.</exception>
        private void LogError(string file, int lineNumber, int columnNumber, string message, params object[] messageArgs)
        {
            if (lineNumber > 0)
            {
                var format = "{0} ({1}, {2}) error: {3}";
                this.errorMessageStore.AppendLine(string.Format(format, System.IO.Path.GetFileName(file), lineNumber,
                    columnNumber,
                    string.Format(message, messageArgs)));
                this.verboseMessageStore.AppendLine(string.Format(format, System.IO.Path.GetFileName(file), lineNumber,
                    columnNumber,
                    string.Format(message, messageArgs)));
            }
            else
            {
                this.errorMessageStore.AppendLine(message);
                this.verboseMessageStore.AppendLine(message);
            }
        }

        /// <summary>Logs an error using the message, and optionally the stack-trace from the given exception and any inner exceptions.</summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="showStackTrace">true to include the stack trace in the log; otherwise, false.</param>
        /// <param name="showDetail">true to log exception types and any inner exceptions; otherwise, false.</param>
        /// <param name="file">The name of the file related to the exception, or null if the project file should be logged.</param>
        private void LogErrorFromException(Exception exception, bool showStackTrace, bool showDetail, string file)
        {
            string text;
            if (!showDetail && Environment.GetEnvironmentVariable("MSBUILDDIAGNOSTICS") == null)
            {
                text = exception.Message;
                if (showStackTrace)
                {
                    text = text + Environment.NewLine + exception.StackTrace;
                }
            }
            else
            {
                var stringBuilder = new StringBuilder(200);
                do
                {
                    stringBuilder.Append(exception.GetType().Name);
                    stringBuilder.Append(": ");
                    stringBuilder.AppendLine(exception.Message);
                    if (showStackTrace)
                    {
                        stringBuilder.AppendLine(exception.StackTrace);
                    }
                    exception = exception.InnerException;
                } while (exception != null);
                text = stringBuilder.ToString();
            }
            this.LogError(file, 0, 0, text, new object[0]);
        }

        /////// <summary>Logs a warning with the specified message.</summary>
        /////// <param name="message">The message.</param>
        /////// <param name="messageArgs">Optional arguments for formatting the message string.</param>
        /////// <exception cref="T:System.ArgumentNullException">
        ///////   <paramref name="message" /> is null.</exception>
        ////private void LogWarning(string message, params object[] messageArgs)
        ////{
        ////    this.LogWarning(null, 0, 0, message, messageArgs);
        ////}

        /// <summary>Logs a warning using the specified message and other warning details.</summary>
        /// <param name="file">The path to the file containing the warning.</param>
        /// <param name="lineNumber">The line in the file where the warning occurs.</param>
        /// <param name="columnNumber">The column in the file where the warning occurs.</param>
        /// <param name="message">The message.</param>
        /// <param name="messageArgs">Optional arguments for formatting the message string.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="message" /> is null.</exception>
        private void LogWarning(string file, int lineNumber, int columnNumber, string message, params object[] messageArgs)
        {
            var format = "{0} ({1}, {2}) warning: {3}";

            this.warningMessageStore.AppendLine(string.Format(format, System.IO.Path.GetFileName(file), lineNumber,
                columnNumber,
                string.Format(message, messageArgs)));

            this.verboseMessageStore.AppendLine(string.Format(format, System.IO.Path.GetFileName(file), lineNumber,
                columnNumber,
                string.Format(message, messageArgs)));
        }

        /////// <summary>
        /////// Resets the current instance of all stored messages.
        /////// </summary>
        ////public void Reset()
        ////{
        ////    this.errorMessageStore.Clear();
        ////    this.warningMessageStore.Clear();
        ////    this.verboseMessageStore.Clear();
        ////}

        #endregion Methods
    }
}