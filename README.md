# Cake.XdtTransform

This was adapted from an [open issue](https://github.com/cake-build/cake/issues/321) on Github that shows
how to perform an XDT config transform. The issue is still open, but I didn't there would need to be an
extra dependency pulled in unless you really needed it and that is why it is still open.

## Support

If you would like to support this project, there are several opportunities. Pull Requests, bug reports, documentation, promotion, and encouragement are all great ways. If you would like to contribute monetarily, you can [Buy Me a Coffee](https://www.buymeacoffee.com/aQPnJ73O8).

## Contributions

This repository uses GitHub Flow, please create a feature branch then submit your PRs to main.

## Usage

```csharp
#addin nuget:?package=Cake.XdtTransform&version=0.18.0&loaddependencies=true

var target = Argument("target", "Default");

Task("TransformConfig")
  .Does(() =>
{
  var sourceFile      = File("web.config");
  var transformFile   = File("web.release.config");
  var targetFile      = File("web.target.config");
  XdtTransformConfig(sourceFile, transformFile, targetFile);
});

RunTarget(target);
```

Here is an example of applying multiple configs and showing encountered problems in console.
Transformations usually do not notify user about potential problems like ‘transformation target element not found’. This may lead to silent problems, for example, someone renaming a config key, but forgetting about transformations – keep an eye on the log.  

```csharp
// Find and apply all pairs like Web.base.config + Web.ENV.config => Web.config
var environment = Argument("environmentName", "DEV");
Task("Transform-all-configs")
    .Does(() =>
    {

        (string, string, string)[] trasfromations = GetPahts(environment, "./");

        foreach ((var baseConfigPath, var transfromationConfigPath, var resultingConfigPath) in trasfromations)
        {
            WrtieLine("");

            Console.ForegroundColor = ConsoleColor.Green;
            WriteLine($"Running transform for: \r\n{baseConfigPath}\r\n{transfromationConfigPath}");
            Console.ForegroundColor = ConsoleColor.White;
            var transformationLog = XdtTransformConfigWithDefaultLogger(
                                                            File(baseConfigPath),
                                                            File(transfromationConfigPath),
                                                            File(resultingConfigPath));

            var hasProblems = (transformationLog.HasError
                                || transformationLog.HasException
                                || transformationLog.HasWarning);

            if(hasProblems == false)
            {
                continue;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            WriteLine($"Tranfomration log has problems.");
            Console.ForegroundColor = ConsoleColor.White;

            var problemEntries = transformationLog.Log
                                            .Where(x => x.MessageType == "Error"
                                                        || x.MessageType == "Exception"
                                                        || x.MessageType == "Warning");

            foreach (var entry in problemEntries)
            {
                WriteLine(entry);
            }

            Console.ForegroundColor = Console.Gray;
        }
    });
```

## Discussion

For questions and to discuss ideas & feature requests, use the [GitHub discussions on the Cake GitHub repository](https://github.com/cake-build/cake/discussions), under the [Extension Q&A](https://github.com/cake-build/cake/discussions/categories/extension-q-a) category.

[![Join in the discussion on the Cake repository](https://img.shields.io/badge/GitHub-Discussions-green?logo=github)](https://github.com/cake-build/cake/discussions)
