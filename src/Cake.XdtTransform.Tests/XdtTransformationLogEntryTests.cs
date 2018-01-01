using System;
using Microsoft.Web.XmlTransform;
using Cake.XdtTransform.Tests.Util;
using Should;

namespace Cake.XdtTransform.Tests
{
    public sealed class XdtTransformationLogEntryTests
    {
        public void ForEmptyItemToStringIsMinimum()
        {
            var stringRepresentation = "";
             var item = new XdtTransformationLogEntry();
            item.Timestamp = new DateTime(2000, 1, 2, 3, 4, 5);

            CultureUtil.UseGBCulture(() =>
            {              
                stringRepresentation = item.ToString();             
            });

            stringRepresentation.ShouldEqual("[02/01/2000 03:04:05] ");
        }

        public void ForFullItemToStringIsMaximum()
        {
            var item = new XdtTransformationLogEntry();
            item.File = "File";
            item.LineNumber = 10;
            item.LinePosition = 20;
            item.Message = "Message {0} {1}";
            item.MessageArgs = new object[] { "arg0", 30 };
            item.MessageVerbosityType = MessageType.Verbose;
            item.MessageType = "Type";
            item.Timestamp = new DateTime(2000, 1, 2, 3, 4, 5);

            try
            {
                throw new Exception("Exception was thrown.");
            }
            catch (Exception ex)
            {
                item.Exception = ex;
            }

            var stringRepresentation = "";

            CultureUtil.UseGBCulture(() =>
            {
                stringRepresentation = item.ToString();              
            });

            stringRepresentation.ShouldStartWith(@"[02/01/2000 03:04:05] [MessageType:Type] [MessageVerbosityType:Verbose] [File:File] [LineNumber:10] [LinePosition:20] Exception: System.Exception: Exception was thrown.
   at Cake.XdtTransform.Tests.XdtTransformationLogEntryTests.ForFullItemToStringIsMaximum() in ");

            stringRepresentation.ShouldContain("Message arg0 30");
        }
    }
}
