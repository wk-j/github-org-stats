using System;
using System.Collections.Generic;

namespace GitHubOrgStats.Models {
    public class QRepository {
        public Guid Id { get; set; }
        public string Name { set; get; }
        public string Language { set; get; }
        public List<QCommit> Commits { set; get; }
        public List<QIssue> Issues { set; get; }
    }
}
