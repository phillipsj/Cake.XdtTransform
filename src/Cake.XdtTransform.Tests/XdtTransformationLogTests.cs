using System;
using System.Linq;
using Microsoft.Web.XmlTransform;
using Cake.XdtTransform.Tests.Util;
using Should;

namespace Cake.XdtTransform.Tests {
    public sealed class XdtTransformationLogTests {
        public void ImplementsEveryInterfaceMethodAndSavesAllLogEntries() {
            CultureUtil.UseGBCulture(() => {
                var log = new XdtTransformationLog();

                log.HasError.ShouldBeFalse();
                log.HasException.ShouldBeFalse();
                log.HasWarning.ShouldBeFalse();
                log.Log.Count().ShouldEqual(0);

                log.LogMessage("Message {0} {1}", new object[] {"1", 2});
                log.HasError.ShouldBeFalse();
                log.HasException.ShouldBeFalse();
                log.HasWarning.ShouldBeFalse();
                log.Log.Count().ShouldEqual(1);
                log.Log.ElementAt(0).ToString().ShouldContain("[MessageType:Message] Message 1 2");

                log.LogMessage(MessageType.Verbose, "Message {0} {1}", new object[] {"1", 2});
                log.HasError.ShouldBeFalse();
                log.HasException.ShouldBeFalse();
                log.HasWarning.ShouldBeFalse();
                log.Log.Count().ShouldEqual(2);
                log.Log.ElementAt(1).ToString().ShouldContain("[MessageType:Message] [MessageVerbosityType:Verbose] Message 1 2");

                log.LogWarning("File", "Message {0} {1}", new object[] {"1", 2});
                log.HasError.ShouldBeFalse();
                log.HasException.ShouldBeFalse();
                log.HasWarning.ShouldBeTrue();
                log.Log.Count().ShouldEqual(3);
                log.Log.ElementAt(2).ToString().ShouldContain("[MessageType:Warning] [File:File] Message 1 2");

                log.LogWarning("File", 30, 40, "Message {0} {1}", new object[] {"1", 2});
                log.HasError.ShouldBeFalse();
                log.HasException.ShouldBeFalse();
                log.HasWarning.ShouldBeTrue();
                log.Log.Count().ShouldEqual(4);
                log.Log.ElementAt(3).ToString().ShouldContain("[MessageType:Warning] [File:File] [LineNumber:30] [LinePosition:40] Message 1 2");

                log.LogError("Message {0} {1}", new object[] {"1", 2});
                log.HasError.ShouldBeTrue();
                log.HasException.ShouldBeFalse();
                log.HasWarning.ShouldBeTrue();
                log.Log.Count().ShouldEqual(5);
                log.Log.ElementAt(4).ToString().ShouldContain("[MessageType:Error] Message 1 2");

                log.LogError("File", "Message {0} {1}", new object[] {"1", 2});
                log.HasError.ShouldBeTrue();
                log.HasException.ShouldBeFalse();
                log.HasWarning.ShouldBeTrue();
                log.Log.Count().ShouldEqual(6);
                log.Log.ElementAt(5).ToString().ShouldContain("[MessageType:Error] [File:File] Message 1 2");

                log.LogError("File", 30, 40, "Message {0} {1}", new object[] {"1", 2});
                log.HasError.ShouldBeTrue();
                log.HasException.ShouldBeFalse();
                log.HasWarning.ShouldBeTrue();
                log.Log.Count().ShouldEqual(7);
                log.Log.ElementAt(6).ToString().ShouldContain("[MessageType:Error] [File:File] [LineNumber:30] [LinePosition:40] Message 1 2");

                Exception exception = null;
                try {
                    throw new Exception("Exception was thrown");
                }
                catch (Exception ex) {
                    exception = ex;
                }

                log.LogErrorFromException(exception);
                log.HasError.ShouldBeTrue();
                log.HasException.ShouldBeTrue();
                log.HasWarning.ShouldBeTrue();
                log.Log.Count().ShouldEqual(8);
                log.Log.ElementAt(7).ToString().ShouldContain("[MessageType:Exception] Exception: System.Exception: Exception was thrown");

                log.LogErrorFromException(exception, "File");
                log.HasError.ShouldBeTrue();
                log.HasException.ShouldBeTrue();
                log.HasWarning.ShouldBeTrue();
                log.Log.Count().ShouldEqual(9);
                log.Log.ElementAt(8).ToString().ShouldContain("[MessageType:Exception] [File:File] Exception: System.Exception: Exception was thrown");

                log.LogErrorFromException(exception, "File", 30, 40);
                log.HasError.ShouldBeTrue();
                log.HasException.ShouldBeTrue();
                log.HasWarning.ShouldBeTrue();
                log.Log.Count().ShouldEqual(10);
                log.Log.ElementAt(9).ToString().ShouldContain("[MessageType:Exception] [File:File] [LineNumber:30] [LinePosition:40] Exception: System.Exception: Exception was thrown");

                log.StartSection("Message {0} {1}", new object[] {"1", 2});
                log.HasError.ShouldBeTrue();
                log.HasException.ShouldBeTrue();
                log.HasWarning.ShouldBeTrue();
                log.Log.Count().ShouldEqual(11);
                log.Log.ElementAt(10).ToString().ShouldContain("[MessageType:Section] Message 1 2");

                log.StartSection(MessageType.Verbose, "Message {0} {1}", new object[] {"1", 2});
                log.HasError.ShouldBeTrue();
                log.HasException.ShouldBeTrue();
                log.HasWarning.ShouldBeTrue();
                log.Log.Count().ShouldEqual(12);
                log.Log.ElementAt(11).ToString().ShouldContain("[MessageType:Section] [MessageVerbosityType:Verbose] Message 1 2");

                log.EndSection("Message {0} {1}", new object[] {"1", 2});
                log.HasError.ShouldBeTrue();
                log.HasException.ShouldBeTrue();
                log.HasWarning.ShouldBeTrue();
                log.Log.Count().ShouldEqual(13);
                log.Log.ElementAt(12).ToString().ShouldContain("[MessageType:Section] Message 1 2");

                log.EndSection(MessageType.Verbose, "Message {0} {1}", new object[] {"1", 2});
                log.HasError.ShouldBeTrue();
                log.HasException.ShouldBeTrue();
                log.HasWarning.ShouldBeTrue();
                log.Log.Count().ShouldEqual(14);
                log.Log.ElementAt(13).ToString().ShouldContain("[MessageType:Section] [MessageVerbosityType:Verbose] Message 1 2");
            });
        }
    }
}