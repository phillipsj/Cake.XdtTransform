#tool dotnet:?package=GitVersion.Tool&version=5.8.1

var target = Argument("target", "CI");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////
Task("Clean")
    .Does(() =>
{
    Information("Cleaning build directory...");
    CleanDirectory($"./src/Cake.XdtTransform/bin/{configuration}");
    
    Information("Cleaning artifacts directory...");
    CleanDirectory($"./artifacts");
});

Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetBuild("./src/Cake.XdtTransform.sln", new DotNetCoreBuildSettings
    {
        Configuration = configuration,
    });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetTest("./src/Cake.XdtTransform.sln", new DotNetCoreTestSettings
    {
        Configuration = configuration,
        NoBuild = true,
    });
});

Task("CI").IsDependentOn("Test");

Task("Package")
    .IsDependentOn("CI")
    .Does(() =>
{
    var result = GitVersion();
    Information($"The current version is: { result.SemVer }");
    var settings = new DotNetPackSettings
    {
        Configuration = "Release",
        OutputDirectory = "./artifacts/",
        ArgumentCustomization = args=>args.Append($"-p:PackageVersion={result.SemVer}")
    };
   
    DotNetPack("./src/Cake.XdtTransform.sln", settings);
});

Task("Publish")
    .WithCriteria(GitHubActions.IsRunningOnGitHubActions)
    .IsDependentOn("Package")
    .Does(() =>
{    
     var source = EnvironmentVariable("NUGET_SOURCE");
     var apiKey = EnvironmentVariable("NUGET_API_KEY");
     NuGetPush("./artifacts/*.nupkg", new NuGetPushSettings {
         Source = source,
         ApiKey = apiKey
     });

});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);