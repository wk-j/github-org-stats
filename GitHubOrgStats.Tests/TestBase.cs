using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubOrgStats.Tests
{
    public class TestBase
    {
        public LoginInfo LoginInfo()
        {
            var pass = Environment.GetEnvironmentVariable("ghp");

            var info = new LoginInfo
            {
                User = "wk-j",
                Token = pass,
                Organization = "bcircle"
            };

            return info;
        }
    }
}
