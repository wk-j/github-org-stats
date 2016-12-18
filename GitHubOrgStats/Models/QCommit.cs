using System;
namespace GitHubOrgStats.Models {
	public class QCommit {
		public string Commiter { set; get; }
		public string Message { set; get; }
		public DateTime Date { set; get; }
	}
}
