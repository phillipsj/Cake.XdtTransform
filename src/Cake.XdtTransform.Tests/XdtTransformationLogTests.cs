using System;
using System.Linq;
using Cake.XdtTransform.Tests.Util;
using DotNet.Xdt;
using FluentAssertions;
using Xunit;

namespace Cake.XdtTransform.Tests {
    public sealed class XdtTransformationLogTests {
        [Fact]
        public void ImplementsEveryInterfaceMethodAndSavesAllLogEntries() {
            CultureUtil.UseGBCulture(() => {
                var log = new XdtTransformationLog();

                log.HasError.Should().BeFalse();
                log.HasException.Should().BeFalse();
                log.HasWarning.Should().BeFalse();
                log.Log.Count().Should().Equals(0);

                log.LogMessage("Message {0} {1}", new object[] {"1", 2});
                log.HasError.Should().BeFalse();
                log.HasException.Should().BeFalse();
                log.HasWarning.Should().BeFalse();
                log.Log.Count().Should().Equals(1);
                log.Log.ElementAt(0).ToString().Should().Contain("[MessageType:Message] Message 1 2");

                log.LogMessage(MessageType.Verbose, "Message {0} {1}", new object[] {"1", 2});
                log.HasError.Should().BeFalse();
                log.HasException.Should().BeFalse();
                log.HasWarning.Should().BeFalse();
                log.Log.Count().Should().Equals(2);
                log.Log.ElementAt(1).ToString().Should().Contain("[MessageType:Message] [MessageVerbosityType:Verbose] Message 1 2");

                log.LogWarning("File", "Message {0} {1}", new object[] {"1", 2});
                log.HasError.Should().BeFalse();
                log.HasException.Should().BeFalse();
                log.HasWarning.Should().BeTrue();
                log.Log.Count().Should().Equals(3);
                log.Log.ElementAt(2).ToString().Should().Contain("[MessageType:Warning] [File:File] Message 1 2");

                log.LogWarning("File", 30, 40, "Message {0} {1}", new object[] {"1", 2});
                log.HasError.Should().BeFalse();
                log.HasException.Should().BeFalse();
                log.HasWarning.Should().BeTrue();
                log.Log.Count().Should().Equals(4);
                log.Log.ElementAt(3).ToString().Should().Contain("[MessageType:Warning] [File:File] [LineNumber:30] [LinePosition:40] Message 1 2");

                log.LogError("Message {0} {1}", new object[] {"1", 2});
                log.HasError.Should().BeTrue();
                log.HasException.Should().BeFalse();
                log.HasWarning.Should().BeTrue();
                log.Log.Count().Should().Equals(5);
                log.Log.ElementAt(4).ToString().Should().Contain("[MessageType:Error] Message 1 2");

                log.LogError("File", "Message {0} {1}", new object[] {"1", 2});
                log.HasError.Should().BeTrue();
                log.HasException.Should().BeFalse();
                log.HasWarning.Should().BeTrue();
                log.Log.Count().Should().Equals(6);
                log.Log.ElementAt(5).ToString().Should().Contain("[MessageType:Error] [File:File] Message 1 2");

                log.LogError("File", 30, 40, "Message {0} {1}", new object[] {"1", 2});
                log.HasError.Should().BeTrue();
                log.HasException.Should().BeFalse();
                log.HasWarning.Should().BeTrue();
                log.Log.Count().Should().Equals(7);
                log.Log.ElementAt(6).ToString().Should().Contain("[MessageType:Error] [File:File] [LineNumber:30] [LinePosition:40] Message 1 2");

                Exception exception = null;
                try {
                    throw new Exception("Exception was thrown");
                }
                catch (Exception ex) {
                    exception = ex;
                }

                log.LogErrorFromException(exception);
                log.HasError.Should().BeTrue();
                log.HasException.Should().BeTrue();
                log.HasWarning.Should().BeTrue();
                log.Log.Count().Should().Equals(8);
                log.Log.ElementAt(7).ToString().Should().Contain("[MessageType:Exception] Exception: System.Exception: Exception was thrown");

                log.LogErrorFromException(exception, "File");
                log.HasError.Should().BeTrue();
                log.HasException.Should().BeTrue();
                log.HasWarning.Should().BeTrue();
                log.Log.Count().Should().Equals(9);
                log.Log.ElementAt(8).ToString().Should().Contain("[MessageType:Exception] [File:File] Exception: System.Exception: Exception was thrown");

                log.LogErrorFromException(exception, "File", 30, 40);
                log.HasError.Should().BeTrue();
                log.HasException.Should().BeTrue();
                log.HasWarning.Should().BeTrue();
                log.Log.Count().Should().Equals(10);
                log.Log.ElementAt(9).ToString().Should().Contain("[MessageType:Exception] [File:File] [LineNumber:30] [LinePosition:40] Exception: System.Exception: Exception was thrown");

                log.StartSection("Message {0} {1}", new object[] {"1", 2});
                log.HasError.Should().BeTrue();
                log.HasException.Should().BeTrue();
                log.HasWarning.Should().BeTrue();
                log.Log.Count().Should().Equals(11);
                log.Log.ElementAt(10).ToString().Should().Contain("[MessageType:Section] Message 1 2");

                log.StartSection(MessageType.Verbose, "Message {0} {1}", new object[] {"1", 2});
                log.HasError.Should().BeTrue();
                log.HasException.Should().BeTrue();
                log.HasWarning.Should().BeTrue();
                log.Log.Count().Should().Equals(12);
                log.Log.ElementAt(11).ToString().Should().Contain("[MessageType:Section] [MessageVerbosityType:Verbose] Message 1 2");

                log.EndSection("Message {0} {1}", new object[] {"1", 2});
                log.HasError.Should().BeTrue();
                log.HasException.Should().BeTrue();
                log.HasWarning.Should().BeTrue();
                log.Log.Count().Should().Equals(13);
                log.Log.ElementAt(12).ToString().Should().Contain("[MessageType:Section] Message 1 2");

                log.EndSection(MessageType.Verbose, "Message {0} {1}", new object[] {"1", 2});
                log.HasError.Should().BeTrue();
                log.HasException.Should().BeTrue();
                log.HasWarning.Should().BeTrue();
                log.Log.Count().Should().Equals(14);
                log.Log.ElementAt(13).ToString().Should().Contain("[MessageType:Section] [MessageVerbosityType:Verbose] Message 1 2");
            });
        }
    }
}