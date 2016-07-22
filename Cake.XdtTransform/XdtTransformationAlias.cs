using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.XdtTransform {
    /// <summary>
    /// Contains functionality for working with XDT transformations.
    /// </summary>
    [CakeAliasCategory("XDT")]
    public static class XdtTransformationAlias {
        /// <summary>
        /// Transforms configuration files using XDT Transform library.
        /// </summary>
        /// <example>
        /// <code>
        /// var target = Argument("target", "Default");
        ///
        /// Task("TransformConfig")
        ///   .Does(() => {
        ///     var sourceFile = File("web.config");
        ///     var transformFile = File("web.release.config");
        ///     var targetFile = File("web.target.config");
        ///     XdtTransformConfig(sourceFile, transformFile, targetFile);
        /// });
        ///
        /// RunTarget(target);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="sourceFile">Source file to be transformed.</param>
        /// <param name="transformFile">Transformation file.</param>
        /// <param name="targetFile">Output file name for the transformed file.</param>
        [CakeMethodAlias]
        public static void XdtTransformConfig(this ICakeContext context, FilePath sourceFile, FilePath transformFile,
            FilePath targetFile) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context));
            }
            XdtTransformation.TransformConfig(sourceFile, transformFile, targetFile);
        }
    }
}
