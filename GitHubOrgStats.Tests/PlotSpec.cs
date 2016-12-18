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
        public void ShoudSum()
        {
            var repos = GetRepositories();
            var issues = repos.SelectMany(x => x.Issues).Count();
            var commits = repos.SelectMany(x => x.Commits).Count();
        }

        [Fact]
        public void ShouldPlotCommter()
        {
            var plot = new GithubPlot();
            var repos = GetRepositories();
            var rs = plot.PlotCommiters(repos);
            File.Copy(rs, "__Commiters.png", true);

        }


        [Fact]
        public void ShouldPlotIssueCloser()
        {
            var plot = new GithubPlot();
            var repos = GetRepositories();
            var rs = plot.PlotIssueClosers(repos);
            File.Copy(rs, "__IssueCloser.png", true);
        }

        [Fact]
        public void ShouldPlotIssueCreator()
        {
            var plot = new GithubPlot();
            var repos = GetRepositories();
            var rs = plot.PlotIssueCreators(repos);
            File.Copy(rs, "__IssueCreators.png", true);
        }

        [Fact]
        public void ShouldPlotCloseIssue()
        {
            var plot = new GithubPlot();
            var repos = GetRepositories();
            var rs = plot.PlotClosingIssues(repos);

            File.Copy(rs, "__CloseIssues.png", true);
        }


        [Fact]
        public void ShouldPlotOpenIssues()
        {
            var repos = GetRepositories();
            var plot = new GithubPlot();
            var rs = plot.PlotOpeningIssues(repos);

            File.Copy(rs, "__OpenIssues.png", true);
        }



        [Fact]
        public void ShouldPlotIssues()
        {
            var repos = GetRepositories();
            var plot = new GithubPlot();
            var rs = plot.PlotIssues(repos);

            File.Copy(rs, "__Issues.png", true);
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
