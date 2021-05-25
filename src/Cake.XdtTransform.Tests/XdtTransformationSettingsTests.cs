using DotNet.Xdt;
using FluentAssertions;
using Moq;
using Xunit;

namespace Cake.XdtTransform.Tests {
  public sealed class XdtTransformationSettingsTests {
    public sealed class TheCtor {
      [Fact]
      public void ShouldInitializeLoggerToNull() {
        new XdtTransformationSettings().Logger.Should().BeNull("because the logger should be initialized to null");
      }
    }

    public sealed class TheUseDefaultLoggerMethod {
      [Fact]
      public void ShouldUseTheCorrectLoggerType() {
        var subject = new XdtTransformationSettings().UseDefaultLogger();
        subject.Logger.Should().BeOfType<XdtTransformationLog>($"because the logger should be set as the default {nameof(XdtTransformationLog)} type.");
      }
    }

    public sealed class TheUseLoggerMethod {
      [Fact]
      public void ShouldAcceptNull() {
        var subject = new XdtTransformationSettings();
        var ex = Record.Exception(() => subject.UseLogger(null));

        ex.Should().BeNull($"because no exception should be thrown when using a null {nameof(IXmlTransformationLogger)}");
        subject.Logger.Should().BeNull();
      }

      [Fact]
      public void ShouldSetTheLoggerCorrectly() {
        var mockLogger = Mock.Of<IXmlTransformationLogger>();
        var subject = new XdtTransformationSettings();

        subject.UseLogger(mockLogger);

        subject.Logger.Should().BeSameAs(mockLogger, "because the logger should have been set correctly");
      }
    }
  }
}
