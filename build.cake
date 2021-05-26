#tool dotnet:?package=GitVersion.Tool&version=5.6.9

var target = Argument("target", "Package");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////
Task("Clean")
    .Does(() =>
{
    CleanDirectory($"./src/Cake.XdtTransform/bin/{configuration}");
    CleanDirectory($"./artifacts");
});

Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreBuild("./src/Cake.XdtTransform.sln", new DotNetCoreBuildSettings
    {
        Configuration = configuration,
    });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetCoreTest("./src/Cake.XdtTransform.sln", new DotNetCoreTestSettings
    {
        Configuration = configuration,
        NoBuild = true,
    });
});

Task("Package")
    .IsDependentOn("Test")
    .Does(() =>
{
    var result = GitVersion();
    Information($"The current version is: { result.SemVer }");
    var settings = new DotNetCorePackSettings
    {
        Configuration = "Release",
        OutputDirectory = "./artifacts/",
        ArgumentCustomization = args=>args.Append($"-p:PackageVersion={result.SemVer}")
    };
   
    DotNetCorePack("./src/Cake.XdtTransform.sln", settings);
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);