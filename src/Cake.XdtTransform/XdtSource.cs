using System.IO;

namespace Cake.XdtTransform
{
  /// <summary>
  /// Represents the source of an XDT transformation XML document.
  /// </summary>
  public abstract class XdtSource {
    /// <summary>
    /// Gets the <see cref="Stream"/> representing the transformation document.
    /// </summary>
    /// <returns>The <see cref="Stream"/> representing the transformation document.</returns>
    public abstract Stream GetXdtStream();
  }
}
