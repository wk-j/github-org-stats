using System;
using System.Collections.Generic;
using GitHubOrgStats;
using LiteDB;
using System.Linq;
using GitHubOrgStats.Models;

namespace GithubOrgStats.App {
	public class MainClass {

		public string DbPath { get; } = "GithubOrgStats.App.db";

		public bool IsDbExist() {
			return System.IO.File.Exists(DbPath);
		}

		public List<QRepository> GetRepositories() {

			if (!IsDbExist()) {
				var user = Environment.GetEnvironmentVariable("ghu");
				var pass = Environment.GetEnvironmentVariable("ghp");

				var github = new Github(new LoginInfo { 
					User = user,
					Token = pass,
					Organization = "bcircle"
				});

				using (var db = new LiteDatabase(DbPath)) {
					var stats = github.GetRepositories().Result;
					var col = db.GetCollection<QRepository>("Repositories");
					stats.ToList().ForEach(x => {
						col.Insert(x);
					});
					return stats.ToList();
				}
			} else {


				using (var db = new LiteDatabase(DbPath)){
					var col = db.GetCollection<QRepository>("Repositories");
					return col.FindAll().ToList();
				}
			}
		}

		public static void Main(string[] args) {

			var main = new MainClass();
			var repo = main.GetRepositories();
		}
	}
}
