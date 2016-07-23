using System;
using System.IO;
using Cake.Core;
using Cake.Core.IO;
using Microsoft.Web.XmlTransform;

namespace Cake.XdtTransform {
    public static class XdtTransformation {
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

        public static void TransformConfig(IFileSystem fileSystem, FilePath sourceFile, FilePath transformFile, FilePath targetFile) {
            if (fileSystem == null) {
                throw new ArgumentNullException(nameof(fileSystem), "File system is null.");
            }
            if (sourceFile == null)
            {
                throw new ArgumentNullException(nameof(sourceFile), "Source file path is null.");
            }
            if (transformFile == null)
            {
                throw new ArgumentNullException(nameof(transformFile), "Transform file path is null.");
            }
            if (targetFile == null)
            {
                throw new ArgumentNullException(nameof(targetFile), "Target file path is null.");
            }

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
    }
}