using System;
using System.Collections.Generic;
using System.IO;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using System.Linq;
using NUnit.Framework;
using Xunit;
using GithubOrgStats.App;

namespace GitHubOrgStats.Tests
{
    public class PlotSpec : TestBase
    {

        [Fact]
        public void ShouldIssues()
        {
            var mainClass = new MainClass();
            var repos = mainClass.GetRepositories();

            var plot = new GithubPlot();
            var rs = plot.PlotIssues(repos);

            File.Copy(rs, "__Issues.png");
        }


        [Fact]
        public void ShouldPlotLongRun()
        {
            var mainClass = new MainClass();
            var repos = mainClass.GetRepositories();

            var plot = new GithubPlot();
            var rs = plot.PlotLongRun(repos);

            rs.ToList().Select((x, i) => new { Value = x, Index = i }).ToList().ForEach(x =>
             {
                 File.Copy(x.Value, $"__LongRun-{x.Index}.png", true);
             });
        }

        [Fact]
        public void ShouldPlotCommitByDay()
        {
            var mainClass = new MainClass();
            var repos = mainClass.GetRepositories();

            var plot = new GithubPlot();
            var rs = plot.PlotCommitDays(repos);

            File.Copy(rs, "__Days.png", true);
        }

        [Fact]
        public void ShouldPlotCommits()
        {
            var mainClass = new MainClass();
            var repos = mainClass.GetRepositories();

            var plot = new GithubPlot();
            var rs = plot.PlotTopCommits(repos);

            File.Copy(rs, "__Commits.png", true);
        }

        [Fact]
        public void ShouldPlotLanguage()
        {
            var mainClass = new MainClass();
            var repos = mainClass.GetRepositories();

            var plot = new GithubPlot();
            var rs = plot.PlotTopLanguages(repos);

            File.Copy(rs, "__Languages.png", true);
        }


    }
}
