using System;
using Octokit;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;

namespace GitHubOrgStats.Tests
{
    public class ApiSpec : TestBase
    {

        [Test]
        public void ShouldGetRepository()
        {
            var info = LoginInfo();
            var github = new Github(info);
            var repo = github.GetRepositories().Result.ToList();
        }

        [Test]
        public async void ShouldConnectToGithub()
        {

            var pass = Environment.GetEnvironmentVariable("ghp");

            var auth = new Credentials("wk-j", pass);

            var app = "MyApp";
            var github = new GitHubClient(new ProductHeaderValue(app));
            github.Credentials = auth;

            var org = await github.Organization.Get("bcircle");
            var repo = org.TotalPrivateRepos;


            var e = await github.Repository.Get("bcircle", "easy-capture");
            var issues = e.OpenIssuesCount;


            var repos = await github.Repository.GetAllForOrg("bcircle");

            repo.Should().BeGreaterThan(10);
        }
    }
}
