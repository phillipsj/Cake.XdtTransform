using System;
using System.IO;
using Cake.Core.IO;
using FluentAssertions;
using Moq;
using Xunit;

namespace Cake.XdtTransform.Tests {
  public sealed class XdtFileSourceTests {
    public sealed class TheCtor {
      [Fact]
      public void ShouldThrowOnNullFile() {
        var ex = Record.Exception(() => new XdtFileSource(null));
        ex.Should().BeOfType<ArgumentNullException>();
      }
    }

    public sealed class TheGetStreamMethod {
      [Fact]
      public void ShouldReturnFileStream() {
        var mockFile = new Mock<IFile>();
        mockFile.Setup(x => x.Open(FileMode.Open, FileAccess.Read, FileShare.Read)).Returns(new MemoryStream(new byte[] {1, 2, 3}));

        using var stream = new XdtFileSource(mockFile.Object).GetXdtStream();
        
        var bytes = new byte[3];
        stream.Read(bytes, 0, (int)stream.Length);

        stream.Length.Should().Be(3);
        bytes.Should().BeEquivalentTo(new byte[] {1, 2, 3}, "because the file should have returned our exact mocked stream");
      }
    }
  }
}
