using System;
using System.IO;
using System.Text;
using Cake.Core.IO;
using Cake.XdtTransform.Tests.Fixtures;
using Should;
using Should.Core.Assertions;

namespace Cake.XdtTransform.Tests {
    public sealed class XdtTransformationTests {
        public void ShouldErrorIfSourceFileIsNull() {
            // Given
            var fixture = new XdtTransformationFixture {SourceFile = null};

            // When
            var result = Record.Exception(() => fixture.TransformConfig());

            // Then
            result.ShouldBeType<ArgumentNullException>().ParamName.ShouldEqual("sourceFile");
        }

        public void ShouldErrorIfTransformFileIsNull() {
            // Given
            var fixture = new XdtTransformationFixture {TransformFile = null};

            // When
            var result = Record.Exception(() => fixture.TransformConfig());

            // Then
            result.ShouldBeType<ArgumentNullException>().ParamName.ShouldEqual("transformFile");
        }

        public void ShouldErrorIfTargetFileIsNull() {
            // Given
            var fixture = new XdtTransformationFixture {TargetFile = null};

            // When
            var result = Record.Exception(() => fixture.TransformConfig());

            // Then
            result.ShouldBeType<ArgumentNullException>().ParamName.ShouldEqual("targetFile");
        }

        public void ShouldErrorIfSourceFileNotExists() {
            // Given
            var fixture = new XdtTransformationFixture(sourceFileExists: false) {
                SourceFile = "/Working/non-existing.config"
            };

            // When
            var result = Record.Exception(() => fixture.TransformConfig());

            // Then
            result.ShouldBeType<FileNotFoundException>().Message.ShouldContain("Unable to find the specified file.");
        }

        public void ShouldErrorIfTransformFileNotExists() {
            // Given
            var fixture = new XdtTransformationFixture(transformFileExists: false) {
                TransformFile = "/Working/non-existing-transform.config"
            };

            // When
            var result = Record.Exception(() => fixture.TransformConfig());

            // Then
            result.ShouldBeType<FileNotFoundException>().Message.ShouldContain("Unable to find the specified file.");
        }

        public void ShouldTransformFile() {
            // Given
            var fixture = new XdtTransformationFixture {
                TargetFile = "/Working/transformed.config"
            };

            // When
            fixture.TransformConfig();

            // Then
            var transformedFile = fixture.FileSystem.GetFile(fixture.TargetFile);
            transformedFile.Exists.ShouldEqual(true);
            string transformedString;
            using (var transformedStream = transformedFile.OpenRead()) {
                using (var streamReader = new StreamReader(transformedStream, Encoding.UTF8)) {
                    transformedString = streamReader.ReadToEnd();
                }
            }
            transformedString.ShouldContain("<add key=\"transformed\" value=\"false\"/>");
        }
    }
}
