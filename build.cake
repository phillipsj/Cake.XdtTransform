#tool "nuget:?package=Fixie"
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("./src/Cake.XdtTransform/bin") + Directory(configuration);
var solution = "./src/Cake.XdtTransform.sln";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>{
        CleanDirectory(buildDir);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() => {
        NuGetRestore(solution);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>{
      // Use MSBuild
      MSBuild(solution, settings =>
        settings.SetConfiguration(configuration));  
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>{
        Fixie("./src/**/bin/" + configuration + "/*.Tests.dll");
});

Task("Package")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>{
        NuGetPack("./src/Cake.XdtTransform/Cake.XdtTransform.nuspec", new NuGetPackSettings());
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Run-Unit-Tests");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);



