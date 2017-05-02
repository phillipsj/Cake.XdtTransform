using System;
using System.IO;
using Cake.Core;
using Cake.Core.IO;
using Microsoft.Web.XmlTransform;

namespace Cake.XdtTransform {
    /// <summary>
    /// The XDT Transformatin class.
    /// </summary>
    public static class XdtTransformation {
        /// <summary>
        /// Transforms config file.
        /// </summary>
        /// <param name="sourceFile">Source config file.</param>
        /// <param name="transformFile">Tranformation to apply.</param>
        /// <param name="targetFile">Target config file.</param>
        public static void TransformConfig(FilePath sourceFile, FilePath transformFile, FilePath targetFile) {
            CheckNulls(sourceFile, transformFile, targetFile);
            using (var document = new XmlTransformableDocument {PreserveWhitespace = true})
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
        /// Transforms config file.
        /// </summary>
        /// <param name="fileSystem">The filesystem.</param>
        /// <param name="sourceFile">Source config file.</param>
        /// <param name="transformFile">Tranformation to apply.</param>
        /// <param name="targetFile">Target config file.</param>
        public static void TransformConfig(IFileSystem fileSystem, FilePath sourceFile, FilePath transformFile, FilePath targetFile) {
            if (fileSystem == null) {
                throw new ArgumentNullException(nameof(fileSystem), "File system is null.");
            }
            CheckNulls(sourceFile, transformFile, targetFile);

            IFile
               sourceConfigFile = fileSystem.GetFile(sourceFile),
               transformConfigFile = fileSystem.GetFile(transformFile),
               targetConfigFile = fileSystem.GetFile(targetFile);
            
            using (Stream
                sourceStream = sourceConfigFile.OpenRead(),
                transformStream = transformConfigFile.OpenRead(),
                targetStream = targetConfigFile.OpenWrite())
            using (var document = new XmlTransformableDocument { PreserveWhitespace = true })
            using (var transform = new XmlTransformation(transformStream, null))
            {
                document.Load(sourceStream);

                if (!transform.Apply(document))
                {
                    throw new CakeException(
                        $"Failed to transform \"{sourceFile}\" using \"{transformFile}\" to \"{targetFile}\""
                        );
                }

                document.Save(targetStream);
            }
        }

        private static void CheckNulls(FilePath sourceFile, FilePath transformFile, FilePath targetFile) {
            if (sourceFile == null) {
                throw new ArgumentNullException(nameof(sourceFile), "Source file path is null.");
            }
            if (transformFile == null) {
                throw new ArgumentNullException(nameof(transformFile), "Transform file path is null.");
            }
            if (targetFile == null) {
                throw new ArgumentNullException(nameof(targetFile), "Target file path is null.");
            }
        }
    }
}