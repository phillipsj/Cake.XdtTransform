using System;
using System.IO;
using FluentAssertions;
using Xunit;

namespace Cake.XdtTransform.Tests {
    public sealed class XdtFragmentSourceTests {
        public sealed class TheCtor {
            [Theory]
            [InlineData("")]
            [InlineData("\t")]
            [InlineData(null)]
            public void ShouldThrowOnNullOrWhitespaceDocumentXml(string documentFragment) {
                var ex = Record.Exception(() => new XdtFragmentSource(documentFragment));
                ex.Should().BeOfType<ArgumentException>().Which.ParamName.Should().Be(nameof(documentFragment));
            }
        }

        public sealed class TheGetStreamMethod {
            const string ExpectedXml = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<configuration xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"">
  <appSettings>
    <add key=""key-name"" value=""key-value"" xdt:Locator=""Match(key)"" xdt:Transform=""SetAttributes"" />    
  </appSettings>
</configuration>";

            private const string DocumentFragment = @"<appSettings>
    <add key=""key-name"" value=""key-value"" xdt:Locator=""Match(key)"" xdt:Transform=""SetAttributes"" />    
  </appSettings>";

            [Fact]
            public void ShouldReturnStreamFromFragment() {
                using var stream = new XdtFragmentSource(DocumentFragment).GetXdtStream();
                using var reader = new StreamReader(stream);

                var streamXml = reader.ReadToEnd();

                streamXml.Should().Be(ExpectedXml, "because the document fragment should be expanded to be a valid, complete XDT document string.");
            }

            [Fact]
            public void ShouldReturnStreamFromDocument() {
                const string xmlDocument = @"<?xml version=""1.0"" encoding=""utf-8"" ?><foo xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform""/>";

                using var stream = new XdtFragmentSource(xmlDocument).GetXdtStream();
                using var reader = new StreamReader(stream);

                var streamXml = reader.ReadToEnd();

                streamXml.Should().Be(xmlDocument, "because the document string should validate as good XML and be returned as-is.");
            }

            [Fact]
            public void ShouldThrowIfDocumentIsNotValid() {
                // this should fail because the namespace is not defined correctly.
                // we should try the first check, attempt to wrap the fragment, and ultimately throw.
                const string badXml = @"<appSettings><add key=""key-name"" value=""key-value"" badns:Locator=""Match(key)"" badns:Transform=""SetAttributes"" /></appSettings>";

                var ex = Record.Exception(() => new XdtFragmentSource(badXml).GetXdtStream());
                ex.Should().BeOfType<InvalidOperationException>();
            }
        }
    }
}
