using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubOrgStats.Models
{
    public class QIssue
    {
        public bool IsClosed { set; get; }
        public List<string> Labels { set; get; } = new List<string>();
    }
}
