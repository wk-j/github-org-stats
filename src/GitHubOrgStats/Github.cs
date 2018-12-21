using System;
using System.Collections.Generic;
using Octokit;
using System.Linq;
using System.Threading.Tasks;
using GitHubOrgStats;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.WindowsForms;
using OxyPlot.Series;
using System.IO;
using GitHubOrgStats.Models;

namespace GitHubOrgStats {
    public class Github {
        private readonly LoginInfo Login;
        public Github(LoginInfo login) {
            Login = login;
        }

        public GitHubClient CreateClient() {
            var auth = new Credentials(Login.User, Login.Token);
            var app = "MyApp";
            var github = new GitHubClient(new ProductHeaderValue(app));
            github.Credentials = auth;
            return github;
        }

        public async Task<IEnumerable<QRepository>> GetRepositories() {
            var github = CreateClient();
            var repos = await github.Repository.GetAllForOrg(Login.Organization);

            return repos.ToList().Select(x => {

                var commits = github.Repository.Commit.GetAll(x.Id).Result.Select(k => new QCommit {
                    Commiter = Utility<string>.Default(() => k.Committer.Login),
                    Date = Utility<DateTime>.Default(() => k.Commit.Author.Date.DateTime),
                    Message = k.Commit.Message,
                }).ToList();


                var req = new RepositoryIssueRequest {
                    State = ItemStateFilter.All
                };

                var issues = github.Issue.GetAllForRepository(x.Id, req).Result.Select(k => new QIssue {
                    Title = k.Title,
                    IsClosed = k.State == ItemState.Closed,
                    Labels = k.Labels.Select(y => y.Name).ToList(),
                    CreatedAt = k.CreatedAt.Date,
                    ClosedAt = k.CreatedAt.Date,
                    CreatedBy = k.User.Login,
                    ClosedBy = Utility<string>.Default(() => k.ClosedBy.Login)
                }).ToList();

                return new QRepository {
                    Name = x.Name,
                    Language = x.Language,
                    Commits = commits,
                    Issues = issues
                };
            });
        }


    }
}
