using System;
using DotNet.Xdt;
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
        ///     DotNet.Xdt.IXmlTransformationLogger logger = GetLogger();
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
        ///     var log = XdtTransformConfigWithDefaultLogger(sourceFile, transformFile, targetFile);
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

        /// <summary>
        /// Transforms configuration files using XDT Transform library.
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
        ///     var settings = new XdtTransformationSettings().UseDefaultLogger();
        ///
        ///     XdtTransformConfig(sourceFile, transformFile, settings);
        ///     
        ///     if(settings.Logger.HasWarning)
        ///     {
        ///         var warnings = settings.Logger.Log
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
        /// <param name="sourceFile">Source file to be transformed.  This is also the target file, indicating an in-place transform.</param>
        /// <param name="transformFile">The transformation to apply.</param>
        /// <param name="settings">The settings to use during transformation.</param>
        /// <exception cref="ArgumentNullException"></exception>
        [CakeMethodAlias]
        public static void XdtTransformConfig(this ICakeContext context, FilePath sourceFile, FilePath transformFile, XdtTransformationSettings settings) {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            new XdtTransformation(context.FileSystem).TransformConfig(sourceFile, transformFile, sourceFile, settings);
        }

        /// <summary>
        /// Transforms configuration files using XDT Transform library.
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
        ///     var settings = new XdtTransformationSettings().UseDefaultLogger();
        ///
        ///     XdtTransformConfig(sourceFile, transformFile, targetFile, settings);
        ///     
        ///     if(settings.Logger.HasWarning)
        ///     {
        ///         var warnings = settings.Logger.Log
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
        /// <param name="transformFile">The transformation to apply.</param>
        /// <param name="targetFile">Output file name for the transformed file.</param>
        /// <param name="settings">The settings to use during transformation.</param>
        /// <exception cref="ArgumentNullException"></exception>
        [CakeMethodAlias]
        public static void XdtTransformConfig(this ICakeContext context, FilePath sourceFile, FilePath transformFile, FilePath targetFile, XdtTransformationSettings settings) {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            new XdtTransformation(context.FileSystem).TransformConfig(sourceFile, transformFile, targetFile, settings);
        }

        /// <summary>
        /// Transforms configuration files using XDT Transform library using the specified transformation source.  The transformation source can be a file, a transform document, or a document fragment.
        /// </summary>
        /// <example>
        /// <code>
        /// var target = Argument("target", "Default");
        ///
        /// Task("TransformConfig")
        ///   .Does(() => {
        ///
        ///     var transformFragment = "&lt;appSettings&gt;&lt;add key="key-name" value="key-value" xdt:Locator="Match(key)" xdt:Transform="SetAttributes" /&gt;&lt;/appSettings&gt;";
        ///     var sourceFile = File("web.config");
        ///     var settings = new XdtTransformationSettings().UseDefaultLogger();
        ///
        ///     XdtTransformConfig(sourceFile, new XdtFragmentSource(transformFragment), settings);
        ///     
        ///     if(settings.Logger.HasWarning)
        ///     {
        ///         var warnings = settings.Logger.Log
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
        /// <param name="sourceFile">Source file to be transformed.  This is also the target file, indicating an in-place transform.</param>
        /// <param name="transformation">The transformation to apply.</param>
        /// <param name="settings">The settings to use during transformation.  Specifying <c>null</c> will throw any errors encountered.</param>
        [CakeMethodAlias]
        public static void XdtTransformConfig(this ICakeContext context, FilePath sourceFile, XdtSource transformation, XdtTransformationSettings settings = null) {
            XdtTransformConfig(context, sourceFile, sourceFile, transformation, settings);
        }

        /// <summary>
        /// Transforms configuration files using XDT Transform library using the specified transformation source.  The transformation source can be a file, a transform document, or a document fragment.
        /// </summary>
        /// <example>
        /// <code>
        /// var target = Argument("target", "Default");
        ///
        /// Task("TransformConfig")
        ///   .Does(() => {
        ///
        ///     // specifying a fragment like this will infer and create a full document XML structure.
        ///     // use XdtDocumentSource to pass a fully qualified XML document structure
        ///     var transformFragment = "&lt;appSettings&gt;&lt;add key="key-name" value="key-value" xdt:Locator="Match(key)" xdt:Transform="SetAttributes" /&gt;&lt;/appSettings&gt;";
        ///     var sourceFile = File("web.config");
        ///     var targetFile = File("web.target.config");
        ///     var settings = new XdtTransformationSettings().UseDefaultLogger();
        ///
        ///     XdtTransformConfig(sourceFile, targetFile, new XdtFragmentSource(transformFragment), settings);
        ///     
        ///     if(settings.Logger.HasWarning)
        ///     {
        ///         var warnings = settings.Logger.Log
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
        /// <param name="targetFile">Output file name for the transformed file.</param>
        /// <param name="transformation">The transformation to apply.</param>
        /// <param name="settings">The settings to use during transformation.  Specifying <c>null</c> will throw any errors encountered.</param>
        [CakeMethodAlias]
        public static void XdtTransformConfig(this ICakeContext context, FilePath sourceFile, FilePath targetFile, XdtSource transformation, XdtTransformationSettings settings = null) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context));
            }
            new XdtTransformation(context.FileSystem).TransformConfig(sourceFile, targetFile, transformation, settings ?? new XdtTransformationSettings());
        }
    }
}
