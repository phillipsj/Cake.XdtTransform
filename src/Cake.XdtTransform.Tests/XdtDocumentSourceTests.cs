using System;
using System.IO;
using FluentAssertions;
using Xunit;

namespace Cake.XdtTransform.Tests {
  public sealed class XdtDocumentSourceTests {
    public sealed class TheCtor {
      [Theory]
      [InlineData("")]
      [InlineData("\t")]
      [InlineData(null)]
      public void ShouldThrowOnNullOrWhitespaceDocumentXml(string documentXml) {
        var ex = Record.Exception(() => new XdtDocumentSource(documentXml));
        ex.Should().BeOfType<ArgumentException>().Which.ParamName.Should().Be(nameof(documentXml));
      }
    }

    public sealed class TheGetStreamMethod {
      const string sourceTransformationXml = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<configuration xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"">
  <appSettings>
    <add key=""key-name"" value=""key-value"" xdt:Locator=""Match(key)"" xdt:Transform=""SetAttributes"" />    
  </appSettings>
</configuration>";

      [Fact]
      public void ShouldReturnStreamContainingXmlDocument() {
        using var stream = new XdtDocumentSource(sourceTransformationXml).GetXdtStream();
        using var reader = new StreamReader(stream);

        var streamXml = reader.ReadToEnd();

        streamXml.Should().Be(sourceTransformationXml, "because the xml string should roundtrip successfully");
      }
    }
  }
}