using System;
using System.IO;
using Cake.Core.IO;
using Cake.XdtTransform.Tests.Fixtures;
using Cake.XdtTransform.Tests.Properties;
using FluentAssertions;
using Xunit;

namespace Cake.XdtTransform.Tests {
    public sealed class XdtTransformationTests {
        public sealed class TheCtor {
            [Fact]
            public void ShouldThrowIfFileSystemIsNull() {
                var ex = Record.Exception(() => new XdtTransformation(null));

                ex.Should().BeOfType<ArgumentNullException>().Subject.ParamName.Should().Be("fileSystem");
            }
        }

        public sealed class TheStaticMethods {
            [Fact]
            public void ShouldErrorIfSourceFileIsNull() {
                // Given
                var fixture = new XdtTransformationFixture {SourceFile = null};

                // When
                var result = Record.Exception(() => fixture.TransformConfig());

                // Then
                result.Should().BeOfType<ArgumentNullException>().Subject.ParamName.Should().Be("sourceFile");
            }

            [Fact]
            public void ShouldErrorIfTransformFileIsNull() {
                // Given
                var fixture = new XdtTransformationFixture {TransformFile = null};

                // When
                var result = Record.Exception(() => fixture.TransformConfig());

                // Then
                result.Should().BeOfType<ArgumentNullException>().Subject.ParamName.Should().Be("transformFile");
            }

            [Fact]
            public void ShouldErrorIfTargetFileIsNull() {
                // Given
                var fixture = new XdtTransformationFixture {TargetFile = null};

                // When
                var result = Record.Exception(() => fixture.TransformConfig());

                // Then
                result.Should().BeOfType<ArgumentNullException>().Subject.ParamName.Should().Be("targetFile");
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
                log.Log.Count.Should().Be(15);
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

        public sealed class TheTransformConfigUsingFileMethod {
            [Fact]
            public void ShouldThrowWhenSourceFileIsNull() {
                var fixture = new XdtTransformationFixture();

                var ex = Record.Exception(() => fixture.TransformConfig(null, fixture.TransformFile, fixture.TargetFile, new XdtTransformationSettings()));

                ex.Should().BeOfType<ArgumentNullException>().Subject.ParamName.Should().Be("sourceFile");
            }

            [Fact]
            public void ShouldThrowWhenTransformFileIsNull() {
                var fixture = new XdtTransformationFixture();

                var ex = Record.Exception(() => fixture.TransformConfig(fixture.SourceFile, null, fixture.TargetFile, new XdtTransformationSettings()));

                ex.Should().BeOfType<ArgumentNullException>().Subject.ParamName.Should().Be("transformFile");
            }

            [Fact]
            public void ShouldThrowWhenTargetFileIsNull() {
                var fixture = new XdtTransformationFixture();

                var ex = Record.Exception(() => fixture.TransformConfig(fixture.SourceFile, fixture.TransformFile, (FilePath) null, new XdtTransformationSettings()));

                ex.Should().BeOfType<ArgumentNullException>().Subject.ParamName.Should().Be("targetFile");
            }

            [Fact]
            public void ShouldThrowWhenSettingsIsNull() {
                var fixture = new XdtTransformationFixture();

                var ex = Record.Exception(() => fixture.TransformConfig(fixture.SourceFile, fixture.TransformFile, fixture.TargetFile, null));

                ex.Should().BeOfType<ArgumentNullException>().Subject.ParamName.Should().Be("settings");
            }

            [Fact]
            public void ShouldTransformFile() {
                var fixture = new XdtTransformationFixture {TargetFile = "/Working/transformed.config"};

                fixture.TransformConfig(fixture.SourceFile, fixture.TransformFile, fixture.TargetFile, new XdtTransformationSettings());

                var transformedString = fixture.GetTargetFileContent();
                transformedString.Should().Contain("<add key=\"transformed\" value=\"false\"/>");
            }
        }

        public sealed class TheTransformConfigUsingXdtSourceMethod {
            [Fact]
            public void ShouldThrowWhenSourceFileIsNull() {
                var fixture = new XdtTransformationFixture();

                var ex = Record.Exception(() => fixture.TransformConfig(null, fixture.TargetFile, fixture.TransformFileSource, new XdtTransformationSettings()));

                ex.Should().BeOfType<ArgumentNullException>().Subject.ParamName.Should().Be("sourceFile");
            }

            [Fact]
            public void ShouldThrowWhenTargetFileIsNull() {
                var fixture = new XdtTransformationFixture();

                var ex = Record.Exception(() => fixture.TransformConfig(fixture.SourceFile, null, fixture.TransformFileSource, new XdtTransformationSettings()));

                ex.Should().BeOfType<ArgumentNullException>().Subject.ParamName.Should().Be("targetFile");
            }

            [Fact]
            public void ShouldThrowWhenXdtTransformIsNull() {
                var fixture = new XdtTransformationFixture();

                var ex = Record.Exception(() => fixture.TransformConfig(fixture.SourceFile, fixture.TargetFile, (XdtSource) null, new XdtTransformationSettings()));

                ex.Should().BeOfType<ArgumentNullException>().Subject.ParamName.Should().Be("transformation");
            }

            [Fact]
            public void ShouldThrowWhenSettingsIsNull() {
                var fixture = new XdtTransformationFixture();

                var ex = Record.Exception(() => fixture.TransformConfig(fixture.SourceFile, fixture.TransformFile, fixture.TransformFileSource, null));

                ex.Should().BeOfType<ArgumentNullException>().Subject.ParamName.Should().Be("settings");
            }

            [Fact]
            public void ShouldTransformFileUsingXdtFileTransform() {
                var fixture = new XdtTransformationFixture {TargetFile = "/Working/transformed.config"};

                fixture.TransformConfig(fixture.SourceFile, fixture.TargetFile, fixture.TransformFileSource, new XdtTransformationSettings());

                var transformedString = fixture.GetTargetFileContent();
                transformedString.Should().Contain("<add key=\"transformed\" value=\"false\"/>");
            }

            [Fact]
            public void ShouldTransformFileUsingXdtDocumentTransform() {
                var fixture = new XdtTransformationFixture {TargetFile = "/Working/transformed.config"};

                var transform = new XdtDocumentSource(Resources.XdtTransformation_TransformFile);
                fixture.TransformConfig(fixture.SourceFile, fixture.TargetFile, transform, new XdtTransformationSettings());

                var transformedString = fixture.GetTargetFileContent();
                transformedString.Should().Contain("<add key=\"transformed\" value=\"false\"/>");
            }

            [Fact]
            public void ShouldTransformFileUsingXdtFragmentTransform() {
                var fixture = new XdtTransformationFixture {TargetFile = "/Working/transformed.config"};

                var transform = new XdtFragmentSource(Resources.XdtTransformation_DocumentFragment);
                fixture.TransformConfig(fixture.SourceFile, fixture.TargetFile, transform, new XdtTransformationSettings());

                var transformedString = fixture.GetTargetFileContent();
                transformedString.Should().Contain("<add key=\"transformed\" value=\"false\"/>");
            }
        }
    }
}
