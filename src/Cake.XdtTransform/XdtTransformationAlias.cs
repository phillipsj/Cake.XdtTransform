using System;
using Microsoft.Web.XmlTransform;
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

        /// <summary>
        /// Transforms configuration files using XDT Transform library, allows you to pass a custom logger
        /// </summary>
        /// <example>
        /// <code>
        /// var target = Argument("target", "Default");
        ///
        /// Task("TransformConfig")
        ///   .Does(() => {
        /// 
        ///     Microsoft.Web.XmlTransform.IXmlTransformationLogger logger = GetLogger();
        /// 
        ///     var sourceFile = File("web.config");
        ///     var transformFile = File("web.release.config");
        ///     var targetFile = File("web.target.config");
        ///     XdtTransformConfigWithLogger(sourceFile, transformFile, targetFile, logger);
        ///     
        ///     AnalyzeLog(logger);
        /// });
        ///
        /// RunTarget(target);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="sourceFile">Source file to be transformed.</param>
        /// <param name="transformFile">Transformation file.</param>
        /// <param name="targetFile">Output file name for the transformed file.</param>
        /// <param name="logger">Logger for the transformation process</param>
        [CakeMethodAlias]
        public static void XdtTransformConfigWithLogger(this ICakeContext context, FilePath sourceFile, FilePath transformFile,
    FilePath targetFile, IXmlTransformationLogger logger)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            XdtTransformation.TransformConfig(context.FileSystem, sourceFile, transformFile, targetFile, logger);
        }

        /// <summary>
        /// Transforms configuration files using XDT Transform library and returns transformation log
        /// </summary>
        /// <example>
        /// <code>
        /// var target = Argument("target", "Default");
        ///
        /// Task("TransformConfig")
        ///   .Does(() => {
        /// 
        ///     var sourceFile = File("web.config");
        ///     var transformFile = File("web.release.config");
        ///     var targetFile = File("web.target.config");
        ///     var log = XdtTransformConfigWithDefaultLogger(sourceFile, transformFile, targetFile, logger);
        ///     
        ///     if(log.HasWarning)
        ///     {
        ///         var warnings = log.Log
        ///                           .Where(entry => entry.MessageType == XdtTransformationLog.Warning)
        ///                           .Select(entry => entry.ToString());
        ///                           
        ///         var concatWarnings = string.Join("\r\n", warnings);
        ///         
        ///         throw new Exception("Transformation has warnings:\r\n" + concatWarnings);
        ///     }
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
        public static XdtTransformationLog XdtTransformConfigWithDefaultLogger(this ICakeContext context, FilePath sourceFile, FilePath transformFile,
    FilePath targetFile)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            return XdtTransformation.TransformConfigWithDefaultLogger(context.FileSystem, sourceFile, transformFile, targetFile);
        }
    }
}
