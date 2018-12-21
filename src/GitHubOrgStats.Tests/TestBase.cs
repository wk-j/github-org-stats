using GithubOrgStats.App;
using GitHubOrgStats.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubOrgStats.Tests {
    public class TestBase {
        public IEnumerable<QRepository> GetRepositories() {
            var mainClass = new MainClass();
            var repos = mainClass.GetRepositories();
            return repos;
        }
        public LoginInfo LoginInfo() {
            var pass = Environment.GetEnvironmentVariable("GITHUB_TOKEN");

            var info = new LoginInfo {
                User = "wk-j",
                Token = pass,
                Organization = "bcircle"
            };

            return info;
        }
    }
}
