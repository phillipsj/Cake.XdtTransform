using Microsoft.Web.XmlTransform;
using System;

namespace Cake.XdtTransform
{
    /// <summary>
    /// Entry from <see cref="XdtTransformationLogEntry"/>  
    /// </summary>
    public class XdtTransformationLogEntry
    {
        /// <summary>
        /// Time of entry 
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;

        /// <summary>
        ///  <see cref="Microsoft.Web.XmlTransform.MessageType"/> if supplied, depicts verbosity
        /// </summary>
        public MessageType? MessageVerbosityType { get; set; }

        /// <summary>
        /// MessageType, if supplied, i.e. "Message","Warning","Error" etc.
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        /// File, if supplied
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// LineNumber, if supplied
        /// </summary>
        public int? LineNumber { get; set; }

        /// <summary>
        /// LinePosition, if supplied
        /// </summary>
        public int? LinePosition { get; set; }

        /// <summary>
        /// Exception, if supplied
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Message before format
        /// </summary>
        public string Message { get; set; } = "";

        /// <summary>
        /// Message arguments for format
        /// </summary>
        public object[] MessageArgs { get; set; } = new object[0];

        /// <summary>
        /// Outputs all available information as a string
        /// </summary>
        public override string ToString()
        {
            return $"[{Timestamp}] "
                + FormatOrEmptyString(MessageType, () => $"[MessageType:{MessageType}] ")
                + FormatOrEmptyString(MessageVerbosityType, () => $"[MessageVerbosityType:{MessageVerbosityType}] ")
                + FormatOrEmptyString(File, () => $"[File:{File}] ")               
                + FormatOrEmptyString(LineNumber, () => $"[LineNumber:{LineNumber}] ")
                + FormatOrEmptyString(LinePosition, () => $"[LinePosition:{LinePosition}] ")
                + FormatOrEmptyString(Exception, () => $"Exception: {Exception.ToString()} ")
                + FormatOrEmptyString(Message, () => string.Format(Message, MessageArgs));
        }

        private static string FormatOrEmptyString<T>(T item, Func<string> format)
        {
            if(item == null)
            {
                return "";
            }

            return format();
        }

    }
}
