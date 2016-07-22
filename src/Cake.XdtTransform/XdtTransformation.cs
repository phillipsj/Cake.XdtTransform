using System;
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
    }
}