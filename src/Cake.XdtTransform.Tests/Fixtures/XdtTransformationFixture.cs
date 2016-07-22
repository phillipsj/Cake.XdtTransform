using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core.IO;

namespace Cake.XdtTransform.Tests.Fixtures
{
    internal sealed class XdtTransformationFixture
    {
        public FilePath SourceFile { get; set; } 
        public FilePath TransformFile { get; set; } 
        public FilePath TargetFile { get; set; }

        public void TransformConfig() {
        }
    }
}
