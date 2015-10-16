using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.XdtTransform
{
    [CakeAliasCategory("XDT")]
    public static class XdtTransformationAlias
    {
        [CakeMethodAlias]
        public static void XdtTransformConfig(this ICakeContext context, FilePath sourceFile, FilePath transformFile, FilePath targetFile)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            XdtTransformation.TransformConfig(sourceFile, transformFile, targetFile);
        }
    }
}
