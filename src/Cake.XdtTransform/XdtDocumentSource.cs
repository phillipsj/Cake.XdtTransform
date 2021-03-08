using System;
using System.IO;
using System.Text;

namespace Cake.XdtTransform {
  /// <summary>
  /// Represents a literal XDT transformation document.
  /// </summary>
  public sealed class XdtDocumentSource : XdtSource {
    private readonly string _documentXml;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:XdtDocumentSource" /> class.
    /// </summary>
    /// <param name="documentXml">The XML document content.</param>
    public XdtDocumentSource(string documentXml) {
      if (string.IsNullOrWhiteSpace(documentXml)) {
        throw new ArgumentException("Value cannot be null or whitespace.", nameof(documentXml));
      }
      _documentXml = documentXml;
    }

    /// <inheritdoc />
    public override Stream GetXdtStream() {
      var transformStream = new MemoryStream();
      using (var writer = new StreamWriter(transformStream, new UTF8Encoding(false, true), 1024, true)) {
        writer.Write(_documentXml);
        writer.Flush();
        transformStream.Position = 0;
      }

      return transformStream;
    }
  }
}