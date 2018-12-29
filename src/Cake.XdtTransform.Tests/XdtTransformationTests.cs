using System;
using System.IO;
using Cake.XdtTransform.Tests.Fixtures;
using FluentAssertions;
using Xunit;

namespace Cake.XdtTransform.Tests {
    public sealed class XdtTransformationTests {
        [Fact]
        public void ShouldErrorIfSourceFileIsNull() {
            // Given
            var fixture = new XdtTransformationFixture {SourceFile = null};

            // When
            var result = Record.Exception(() => fixture.TransformConfig());

            // Then
            result.Should().BeOfType<ArgumentNullException>().Subject.ParamName.Should().Equals("sourceFile");
        }

        [Fact]
        public void ShouldErrorIfTransformFileIsNull() {
            // Given
            var fixture = new XdtTransformationFixture {TransformFile = null};

            // When
            var result = Record.Exception(() => fixture.TransformConfig());

            // Then
            result.Should().BeOfType<ArgumentNullException>().Subject.ParamName.Should().Equals("transformFile");
        }

        [Fact]
        public void ShouldErrorIfTargetFileIsNull() {
            // Given
            var fixture = new XdtTransformationFixture {TargetFile = null};

            // When
            var result = Record.Exception(() => fixture.TransformConfig());

            // Then
            result.Should().BeOfType<ArgumentNullException>().Subject.ParamName.Should().Equals("targetFile");
        }

        [Fact]
        public void ShouldErrorIfSourceFileNotExists() {
            // Given
            var fixture = new XdtTransformationFixture(sourceFileExists: false) {
                SourceFile = "/Working/non-existing.config"
            };

            // When
            var result = Record.Exception(() => fixture.TransformConfig());

            // Then
            result.Should().BeOfType<FileNotFoundException>().Subject.Message.Should().Contain("Unable to find the specified file.");
        }

        [Fact]
        public void ShouldErrorIfTransformFileNotExists() {
            // Given
            var fixture = new XdtTransformationFixture(transformFileExists: false) {
                TransformFile = "/Working/non-existing-transform.config"
            };

            // When
            var result = Record.Exception(() => fixture.TransformConfig());

            // Then
            result.Should().BeOfType<FileNotFoundException>().Subject.Message.Should().Contain("Unable to find the specified file.");
        }

        [Fact]
        public void ShouldTransformFile() {
            // Given
            var fixture = new XdtTransformationFixture {
                TargetFile = "/Working/transformed.config"
            };

            // When
            fixture.TransformConfig();

            // Then
            var transformedString = fixture.GetTargetFileContent();
            transformedString.Should().Contain("<add key=\"transformed\" value=\"false\"/>");
        }

        [Fact]
        public void ShouldTransformFileWithDefaultLogger() {
            // Given
            var fixture = new XdtTransformationFixture {
                TargetFile = "/Working/transformed.config"
            };

            // When
            var log = fixture.TransformConfigWithDefaultLogger();

            // Then
            var transformedString = fixture.GetTargetFileContent();
            transformedString.Should().Contain("<add key=\"transformed\" value=\"false\"/>");
            transformedString.Should().NotContain("this-is-missing");

            log.HasError.Should().BeFalse();
            log.HasException.Should().BeFalse();
            log.HasWarning.Should().BeTrue();
            log.Log.Count.Should().Equals(15);
        }

        [Fact]
        public void ShouldTransformFileWithDefaultLoggerIfSameSourceAndTarget() {
            // Given
            var fixture = new XdtTransformationFixture();
            fixture.TargetFile = fixture.SourceFile;

            // When
            fixture.TransformConfigWithDefaultLogger();

            // Then
            var transformedString = fixture.GetTargetFileContent();
            transformedString.Should().Contain("<add key=\"transformed\" value=\"false\"/>");
        }
    }
}
