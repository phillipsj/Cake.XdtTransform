using System;
using System.IO;
using Cake.Core.IO;

namespace Cake.XdtTransform {
  /// <summary>
  /// Represents a XDT transformation XML document on the file system.
  /// </summary>
  internal sealed class XdtFileSource : XdtSource {
    private readonly IFile _file;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:XdtFileSource" /> class.
    /// </summary>
    /// <param name="file">The file containing the transform document.</param>
    public XdtFileSource(IFile file) {
      _file = file ?? throw new ArgumentNullException(nameof(file));
    }

    /// <inheritdoc />
    public override Stream GetXdtStream() {
      return _file.OpenRead();
    }
  }
}