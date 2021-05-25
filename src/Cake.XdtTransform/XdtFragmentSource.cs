using System;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Cake.XdtTransform {

  /// <summary>
  /// Represents a fragment of an XDT transformation document.
  /// <para>
  /// This is typically going to be a document fragment like a single &lt;appSettings&gt; value as opposed to a full document.
  /// </para>
  /// <para>
  /// The implementation will attempt to infer the correct document structure using the default .NET &lt;configuration&gt; root element.
  /// For more complex use cases, it's probably most appropriate to use <see cref="XdtDocumentSource"/> or derive a class from <see cref="XdtSource"/>.
  /// </para>
  /// </summary>
  /// <example>
  /// <code>
  /// var transform = "&lt;appSettings&gt;&lt;add key="key-name" value="key-value" xdt:Locator="Match(key)" xdt:Transform="SetAttributes" /&gt;&lt;/appSettings&gt;";
  /// var fragmentSource = new XdtFragmentSource(transform);
  /// </code>
  /// </example>
  public sealed class XdtFragmentSource : XdtSource {
    
    private const string DocumentXmlFormat = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<configuration xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"">
  {0}
</configuration>";
    private readonly string _documentFragment;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="T:XdtFileSource" /> class.
    /// </summary>
    /// <param name="documentFragment">The transformation XML fragment.</param>
    public XdtFragmentSource(string documentFragment) {
      if (string.IsNullOrWhiteSpace(documentFragment)) {
        throw new ArgumentException("Value cannot be null or whitespace.", nameof(documentFragment));
      }
      _documentFragment = documentFragment;
    }

    /// <summary>
    /// Gets the <see cref="Stream"/> representing the transformation document.
    /// </summary>
    /// <returns>The <see cref="Stream"/> representing the transformation document.</returns>
    public override Stream GetXdtStream() {
      // first check the fragment.  if it's valid, process it as a document.
      if (this.IsValidXml(this._documentFragment)) {
        return new XdtDocumentSource(this._documentFragment).GetXdtStream();
      }

      // now try formatting it.
      var xmlDocument = string.Format(CultureInfo.InvariantCulture, DocumentXmlFormat, this._documentFragment);
      if (this.IsValidXml(xmlDocument)) {
        return new XdtDocumentSource(xmlDocument).GetXdtStream();
      }

      throw new InvalidOperationException("The document fragment cannot be converted to a valid XML document.  Unable to create a valid Xdt stream.");
    }

    private bool IsValidXml(string xmlDocument) {
      // quick check - is it valid xml?  we need to check to avoid wrapping perfectly valid XML.
      // i.e., if someone passed an entire document to the fragment constructor.
      var document = new XmlDocument();
      try {
        document.LoadXml(xmlDocument);
        return true;
      }
      catch {
        return false;
      }
    }
  }
}