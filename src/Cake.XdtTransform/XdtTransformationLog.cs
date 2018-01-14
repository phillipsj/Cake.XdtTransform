using Microsoft.Web.XmlTransform;
using System;
using System.Collections.Generic;

namespace Cake.XdtTransform
{
    /// <summary>
    /// Implementation of IXmlTransformationLogger that simply saves all entries.
    /// </summary>
    public class XdtTransformationLog : IXmlTransformationLogger
    {
        /// <summary>
        /// String marker for entries containing errors.
        /// </summary>
        public const string Error = "Error";
        /// <summary>
        /// String marker for entries containing exceptions.
        /// </summary>
        public const string Exception = "Exception";
        /// <summary>
        /// String marker for entries containing warnings.
        /// </summary>
        public const string Warning = "Warning";
        /// <summary>
        /// String marker for entries containing messages.
        /// </summary>
        public const string Message = "Message";
        /// <summary>
        /// String marker for entries entries containing section start'end information.
        /// </summary>
        public const string Section = "Section";
        /// <summary>
        /// Log entries.
        /// </summary>
        public List<XdtTransformationLogEntry> Log { get; set; } = new List<XdtTransformationLogEntry>();
        /// <summary>
        /// True if at least one entry was for an error.
        /// </summary>
        public bool HasError { get; set; } = false;
        /// <summary>
        /// True if at least one entry was for an exception.
        /// </summary>
        public bool HasException { get; set; } = false;
        /// <summary>
        /// True if at least one entry was for a warning.
        /// </summary>
        public bool HasWarning { get; set; } = false;
        /// <summary>
        /// Logging interface implementation.
        /// </summary>
        public void EndSection(string message, params object[] messageArgs)
        {
            Log.Add(new XdtTransformationLogEntry()
            {
                MessageType = Section,
                Message = message,
                MessageArgs = messageArgs
            });
        }
        /// <summary>
        /// Logging interface implementation.
        /// </summary>
        public void EndSection(MessageType type, string message, params object[] messageArgs)
        {
            Log.Add(new XdtTransformationLogEntry()
            {
                MessageType = Section,
                MessageVerbosityType = type,
                Message = message,
                MessageArgs = messageArgs
            });
        }
        /// <summary>
        /// Logging interface implementation.
        /// </summary>
        public void LogError(string message, params object[] messageArgs)
        {
            HasError = true;           
            Log.Add(new XdtTransformationLogEntry()
            {
                MessageType = Error,
                Message = message,
                MessageArgs = messageArgs
            });
        }
        /// <summary>
        /// Logging interface implementation.
        /// </summary>
        public void LogError(string file, string message, params object[] messageArgs)
        {
            HasError = true;
            Log.Add(new XdtTransformationLogEntry()
            {
                MessageType = Error,
                File = file,
                Message = message,
                MessageArgs = messageArgs
            });
        }
        /// <summary>
        /// Logging interface implementation.
        /// </summary>
        public void LogError(string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
        {
            HasError = true;
            Log.Add(new XdtTransformationLogEntry()
            {
                MessageType = Error,
                File = file,
                LineNumber = lineNumber,
                LinePosition = linePosition,
                Message = message,
                MessageArgs = messageArgs
            });
        }
        /// <summary>
        /// Logging interface implementation.
        /// </summary>
        public void LogErrorFromException(Exception ex)
        {
            HasException = true;
            Log.Add(new XdtTransformationLogEntry()
            {
                MessageType = Exception,
                Exception = ex
            });
        }
        /// <summary>
        /// Logging interface implementation.
        /// </summary>
        public void LogErrorFromException(Exception ex, string file)
        {
            HasException = true;
            Log.Add(new XdtTransformationLogEntry()
            {
                MessageType = Exception,
                Exception = ex,
                File = file
            });
        }
        /// <summary>
        /// Logging interface implementation.
        /// </summary>
        public void LogErrorFromException(Exception ex, string file, int lineNumber, int linePosition)
        {
            HasException = true;
            Log.Add(new XdtTransformationLogEntry()
            {
                MessageType = Exception,
                Exception = ex,
                File = file,
                LineNumber = lineNumber,
                LinePosition = linePosition
            });
        }
        /// <summary>
        /// Logging interface implementation.
        /// </summary>
        public void LogMessage(string message, params object[] messageArgs)
        {
            Log.Add(new XdtTransformationLogEntry()
            {
                MessageType = Message,
                Message = message,
                MessageArgs = messageArgs
            });
        }
        /// <summary>
        /// Logging interface implementation.
        /// </summary>
        public void LogMessage(MessageType type, string message, params object[] messageArgs)
        {
            Log.Add(new XdtTransformationLogEntry()
            {
                MessageType = Message,
                MessageVerbosityType = type,
                Message = message,
                MessageArgs = messageArgs
            });
        }
        /// <summary>
        /// Logging interface implementation.
        /// </summary>
        public void LogWarning(string message, params object[] messageArgs)
        {
            HasWarning = true;
            Log.Add(new XdtTransformationLogEntry()
            {
                MessageType = Warning,
                Message = message,
                MessageArgs = messageArgs
            });
        }
        /// <summary>
        /// Logging interface implementation.
        /// </summary>
        public void LogWarning(string file, string message, params object[] messageArgs)
        {
            HasWarning = true;
            Log.Add(new XdtTransformationLogEntry()
            {
                MessageType = Warning,
                File = file,
                Message = message,
                MessageArgs = messageArgs
            });
        }
        /// <summary>
        /// Logging interface implementation.
        /// </summary>
        public void LogWarning(string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
        {
            HasWarning = true;
            Log.Add(new XdtTransformationLogEntry()
            {
                MessageType = Warning,
                File = file,
                LineNumber = lineNumber,
                LinePosition = linePosition,
                Message = message,
                MessageArgs = messageArgs
            });
        }
        /// <summary>
        /// Logging interface implementation.
        /// </summary>
        public void StartSection(string message, params object[] messageArgs)
        {
            Log.Add(new XdtTransformationLogEntry()
            {
                MessageType = Section,
                Message = message,
                MessageArgs = messageArgs
            });
        }
        /// <summary>
        /// Logging interface implementation.
        /// </summary>
        public void StartSection(MessageType type, string message, params object[] messageArgs)
        {
            Log.Add(new XdtTransformationLogEntry()
            {
                MessageVerbosityType = type,
                MessageType = Section,
                Message = message,
                MessageArgs = messageArgs
            });
        }
    }
}
