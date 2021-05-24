using DotNet.Xdt;

namespace Cake.XdtTransform {
  /// <summary>
  /// Contains settings used by <see cref="XdtTransformation"/>.
  /// </summary>
  public sealed class XdtTransformationSettings {
    /// <summary>
    /// Gets or sets the <see cref="IXmlTransformationLogger"/> to use while performing transformations.
    /// </summary>
    /// <remarks>
    /// If set to <c>null</c>, the default implementation will throw any exceptions that are logged.
    /// </remarks>
    public IXmlTransformationLogger Logger { get; internal set; }

    /// <summary>
    /// Sets the logger to use the default implementation of <see cref="XdtTransformationLog"/>.
    /// </summary>
    /// <returns>The same <see cref="XdtTransformationSettings"/> instance so that multiple calls can be chained.</returns>
    public XdtTransformationSettings UseDefaultLogger() {
      this.Logger = new XdtTransformationLog();
      return this;
    }

    /// <summary>
    /// Sets the logger to use a specific instance of <see cref="IXmlTransformationLogger"/> or <c>null</c>.
    /// </summary>
    /// <param name="logger">The <see cref="IXmlTransformationLogger"/> to use, or <c>null</c> to specify that any exceptions should be thrown.</param>
    /// <returns>The same <see cref="XdtTransformationSettings"/> instance so that multiple calls can be chained.</returns>
    public XdtTransformationSettings UseLogger(IXmlTransformationLogger logger) {
      this.Logger = logger;
      return this;
    }
  }
}
