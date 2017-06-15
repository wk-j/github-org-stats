
var sln = "GitHubOrgStats.sln";

Task("Restore").Does(() => {
    NuGetRestore(sln);
});

Task("Build").Does(() => {
    //DotNetBuild(sln);
    MSBuild(sln);
});

var target = Argument("target", "default");

RunTarget(target);