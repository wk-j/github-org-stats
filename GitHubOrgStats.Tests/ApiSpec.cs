using System;
using Octokit;
using FluentAssertions;
using NUnit.Framework;

namespace GitHubOrgStats.Tests {
	public class ApiSpec {
		public ApiSpec() {
		}

		[Test]
		public async void ShouldConnectToGithub() {
			var app = "MyApp";
			var github = new GitHubClient(new ProductHeaderValue(app));

			var user = await github.User.Get("wk-j");
			var repo = user.PublicRepos;

			repo.Should().BeGreaterThan(10);
		}
	}
}
