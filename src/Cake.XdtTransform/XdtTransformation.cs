using System;
using Cake.Core;
using Cake.Core.IO;
using DotNet.Xdt;

namespace Cake.XdtTransform {
    /// <summary>
    /// The XDT Transformation class.
    /// </summary>
    public sealed class XdtTransformation {
    private readonly IFileSystem _fileSystem;

    /// <summary>Initializes a new instance of the <see cref="XdtTransformation"></see> class.</summary>
    public XdtTransformation(IFileSystem fileSystem) {
      _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    /// <summary>
    /// Transforms config file.
    /// </summary>
    /// <param name="sourceFile">Source config file.</param>
    /// <param name="transformFile">Transformation to apply.</param>
    /// <param name="targetFile">Target config file.</param>
    public static void TransformConfig(FilePath sourceFile, FilePath transformFile, FilePath targetFile) {
      if (sourceFile == null) {
        throw new ArgumentNullException(nameof(sourceFile), "Source file path is null.");
      }
      if (transformFile == null) {
        throw new ArgumentNullException(nameof(transformFile), "Transform file path is null.");
      }
      if (targetFile == null) {
        throw new ArgumentNullException(nameof(targetFile), "Target file path is null.");
      }

      using (var document = new XmlTransformableDocument { PreserveWhitespace = true })
      using (var transform = new XmlTransformation(transformFile.ToString())) {
        document.Load(sourceFile.ToString());

        if (!transform.Apply(document)) {
          throw new CakeException(
            $"Failed to transform \"{sourceFile}\" using \"{transformFile}\" to \"{targetFile}\""
            );
        }

        document.Save(targetFile.ToString());
      }
    }

    /// <summary>
    /// Transforms config file and returns transformation log.
    /// </summary>
    /// <param name="fileSystem">The filesystem.</param>
    /// <param name="sourceFile">Source config file.</param>
    /// <param name="transformFile">Transformation to apply.</param>
    /// <param name="targetFile">Target config file.</param>
    public static XdtTransformationLog TransformConfigWithDefaultLogger(IFileSystem fileSystem, FilePath sourceFile, FilePath transformFile, FilePath targetFile) {
      var log = new XdtTransformationLog();

      TransformConfig(fileSystem, sourceFile, transformFile, targetFile, log);

      return log;
    }

    /// <summary>
    /// Transforms config file.
    /// </summary>
    /// <param name="fileSystem">The filesystem.</param>
    /// <param name="sourceFile">Source config file.</param>
    /// <param name="transformFile">Transformation to apply.</param>
    /// <param name="targetFile">Target config file.</param>
    /// <param name="logger">Logger for the transformation process.</param>
    public static void TransformConfig(IFileSystem fileSystem, FilePath sourceFile, FilePath transformFile, FilePath targetFile, IXmlTransformationLogger logger = null) {
      // pass whatever logger we've received.
      var settings = new XdtTransformationSettings().UseLogger(logger);
      new XdtTransformation(fileSystem).TransformConfig(sourceFile, transformFile, targetFile, settings);
    }

    /// <summary>
    /// Transforms config file.
    /// </summary>
    /// <param name="sourceFile">Source config file.</param>
    /// <param name="transformFile">Transformation to apply.</param>
    /// <param name="targetFile">Target config file.</param>
    /// <param name="settings">The settings to use for the transformation.</param>
    internal void TransformConfig(FilePath sourceFile, FilePath transformFile, FilePath targetFile, XdtTransformationSettings settings) {
      if (sourceFile == null) {
        throw new ArgumentNullException(nameof(sourceFile));
      }
      if (transformFile == null) {
        throw new ArgumentNullException(nameof(transformFile));
      }
      if (targetFile == null) {
        throw new ArgumentNullException(nameof(targetFile));
      }
      if (settings == null) {
        throw new ArgumentNullException(nameof(settings));
      }

      void TransformUsingFile(XmlTransformableDocument document) {
        var transformConfigFile = this._fileSystem.GetFile(transformFile);

        using (var transformStream = transformConfigFile.OpenRead())
        using (var transform = new XmlTransformation(transformStream, settings.Logger)) {
          if (!transform.Apply(document)) {
            throw new CakeException(
              $"Failed to transform \"{sourceFile}\" using \"{transformFile}\" to \"{targetFile}\""
            );
          }
        }
      }

      TransformConfig(sourceFile, targetFile, TransformUsingFile);
    }

    internal void TransformConfig(FilePath sourceFile, FilePath targetFile, XdtSource transformation, XdtTransformationSettings settings) {
      if (sourceFile == null) {
        throw new ArgumentNullException(nameof(sourceFile));
      }
      if (targetFile == null) {
        throw new ArgumentNullException(nameof(targetFile));
      }
      if (transformation == null) {
        throw new ArgumentNullException(nameof(transformation));
      }
      if (settings == null) {
        throw new ArgumentNullException(nameof(settings));
      }

      void TransformUsingStream(XmlTransformableDocument document) {
        using (var transform = new XmlTransformation(transformation.GetXdtStream(), settings.Logger)) {
          if (!transform.Apply(document)) {
            throw new CakeException($"Failed to transform \"{sourceFile}\" to \"{targetFile}\"");
          }
        }
      }

      TransformConfig(sourceFile, targetFile, TransformUsingStream);
    }

    private void TransformConfig(FilePath sourceFile, FilePath targetFile, Action<XmlTransformableDocument> transformation) {
      IFile
        sourceConfigFile = this._fileSystem.GetFile(sourceFile),
        targetConfigFile = this._fileSystem.GetFile(targetFile);

      using (var document = new XmlTransformableDocument { PreserveWhitespace = true }) {
        using (var sourceStream = sourceConfigFile.OpenRead()) {
          document.Load(sourceStream);
        }

        transformation(document);

        using (var targetStream = targetConfigFile.OpenWrite()) {
          document.Save(targetStream);
        }
      }
    }
  }
}
