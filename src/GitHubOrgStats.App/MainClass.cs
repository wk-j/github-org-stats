using System;
using System.Collections.Generic;
using GitHubOrgStats;
using LiteDB;
using System.Linq;
using GitHubOrgStats.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GithubOrgStats.App {
    public class MainClass {
        readonly ILogger<MainClass> logger;

        public MainClass(ILogger<MainClass> logger) {
            this.logger = logger;
        }

        public string DbPath { get; } = "GithubOrgStats.App.db";

        public bool IsDbExist() {
            return System.IO.File.Exists(DbPath);
        }

        public async Task<List<QRepository>> GetRepositories() {
            logger.LogInformation("get repositories");

            if (!IsDbExist()) {
                logger.LogInformation("db not exist");

                var user = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
                var pass = Environment.GetEnvironmentVariable("ghp") ?? user;

                var github = new Github(new LoginInfo {
                    User = user,
                    Token = pass,
                    // Organization = "bcircle"
                    Organization = "bcircle-intern"
                });

                using (var db = new LiteDatabase(DbPath)) {
                    logger.LogInformation("get github repositories ...");
                    var stats = await github.GetRepositories();

                    logger.LogInformation("get collections");
                    var col = db.GetCollection<QRepository>("Repositories");
                    stats.ToList().ForEach(x => {
                        logger.LogInformation("insert data - {0} ", x.Name);
                        col.Insert(x);
                    });
                    return stats.ToList();
                }
            } else {
                using (var db = new LiteDatabase(DbPath)) {
                    var col = db.GetCollection<QRepository>("Repositories");
                    return col.FindAll().ToList();
                }
            }
        }

    }
}
