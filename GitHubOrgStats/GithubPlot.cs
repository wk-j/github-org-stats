using GitHubOrgStats.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubOrgStats
{
    public class GithubPlot
    {
        private string ExportToPng(BarSeries bar, CategoryAxis axis, string title, int fontSize = 18)
        {
            var model = new PlotModel
            {
                Title = title,
                DefaultFontSize = fontSize
            };

            var max = bar.ItemsSource.Cast<BarItem>().Max(x => x.Value) + 10;

            bar.LabelPlacement = LabelPlacement.Inside;

            model.Series.Add(bar);
            model.Axes.Add(axis);


            var path = Path.Combine(Path.GetTempPath(), $"{ Guid.NewGuid().ToString("N")}.png");

            var pngExporter = new PngExporter { Width = 1400, Height = 600, Background = OxyColors.White };
            pngExporter.ExportToFile(model, path);
            return path;
        }

        public string PlotIssueClosers(IEnumerable<QRepository> repos)
        {
            var info = repos.SelectMany(x => x.Issues)
                .Where(x => x.IsClosed)
                .GroupBy(x => x.ClosedBy)
                .Select(x => new
                {
                    Closer = x.Key,
                    Total = x.Count()
                }).ToList();

            var bar = new BarSeries
            {
                ItemsSource = info.Select(x => new BarItem { Value = x.Total }),
                LabelFormatString = "{0} Issues"
            };

            var axis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                ItemsSource = info.Select(x => x.Closer)
            };

            return ExportToPng(bar, axis, "");

        }

        public string PlotCommiters(IEnumerable<QRepository> repos)
        {
            var info = repos.SelectMany(x => x.Commits).GroupBy(x => x.Commiter)
                .Select(x => new
                {
                    Commiter = x.Key,
                    Total = x.Count()
                });

            var bar = new BarSeries
            {
                ItemsSource = info.Select(x => new BarItem { Value = x.Total }),
                LabelFormatString = "{0} Commits"
            };

            var axis = new CategoryColorAxis
            {
                Position = AxisPosition.Left,
                ItemsSource = info.Select(x => x.Commiter)
            };

            return ExportToPng(bar, axis, "");
        }

        public string PlotIssueCreators(IEnumerable<QRepository> repos)
        {
            var info = repos.SelectMany(x => x.Issues).GroupBy(x => x.CreatedBy).Select(x => new
            {
                CreateBy = x.Key,
                Total = x.Count()
            }).OrderBy(x => x.CreateBy);

            var bar = new BarSeries
            {
                ItemsSource = info.Select(x => new BarItem { Value = x.Total }),
                LabelFormatString = "{0}"
            };

            var axis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                ItemsSource = info.Select(x => x.CreateBy)
            };

            return ExportToPng(bar, axis, "");
        }

        public string PlotClosingIssues(IEnumerable<QRepository> repos)
        {
            var info = repos.SelectMany(x => x.Issues)
                .Where(x => x.IsClosed)
                .GroupBy(x => x.CreatedAt.DayOfWeek).Select(x => new
                {
                    DayOfWeek = (int)x.First().CreatedAt.DayOfWeek,
                    Day = x.Key,
                    Total = x.Count()
                }).OrderByDescending(x => x.DayOfWeek).ToList();

            var bar = new BarSeries
            {
                ItemsSource = info.Select(x => new BarItem
                {
                    Value = x.Total
                }),
                LabelFormatString = "{0}"
            };

            var axis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                ItemsSource = info.Select(x => x.Day)
            };

            return ExportToPng(bar, axis, "", fontSize: 15);
        }

        public string PlotOpeningIssues(IEnumerable<QRepository> repos)
        {
            var info = repos.SelectMany(x => x.Issues)
                .GroupBy(x => x.CreatedAt.DayOfWeek).Select(x => new
                {
                    DayOfWeek = (int)x.First().CreatedAt.DayOfWeek,
                    Day = x.Key,
                    Total = x.Count()
                }).OrderByDescending(x => x.DayOfWeek).ToList();

            var bar = new BarSeries
            {
                ItemsSource = info.Select(x => new BarItem
                {
                    Value = x.Total
                }),
                LabelFormatString = "{0}"
            };

            var axis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                ItemsSource = info.Select(x => x.Day)
            };

            return ExportToPng(bar, axis, "", fontSize: 15);
        }

        public string PlotIssues(IEnumerable<QRepository> repos)
        {
            var day = repos.Select(x => new
            {
                Issues = x.Issues.Count(),
                Repository = x.Name,
            }).OrderByDescending(x => x.Issues).Where(x => x.Issues > 0).Take(15);

            var bar = new BarSeries
            {
                ItemsSource = day.Select(x => new BarItem
                {
                    Value = x.Issues
                }),
                LabelFormatString = "{0} Issues"
            };

            var axis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                ItemsSource = day.Select(x => x.Repository)
            };

            return ExportToPng(bar, axis, "", fontSize: 15);
        }

        public IEnumerable<string> PlotLongRun(IEnumerable<QRepository> repos)
        {
            var data = repos.Select(x =>
            {
                var commits = x.Commits.OrderBy(k => k.Date).ToList();
                var first = commits.First();
                var last = commits.Last();

                return new
                {
                    Repository = x.Name,
                    Long = (int)((last.Date - first.Date).TotalDays / 30)
                };
            }).OrderByDescending(x => x.Long);


            var size = 10;
            var skip = 0;
            var count = data.Count();

            var pages = Math.Round(1.0 * count / size);

            while (skip <= count)
            {
                var info = data.Skip(skip).Take(size);

                var bar = new BarSeries
                {
                    ItemsSource = info.Select(x => new BarItem
                    {
                        Value = x.Long
                    }),
                    LabelFormatString = "{0} Months"
                };

                var axis = new CategoryAxis
                {
                    Position = AxisPosition.Left,
                    ItemsSource = info.Select(x => x.Repository)
                };

                skip += size;

                yield return ExportToPng(bar, axis, "");
            }
        }

        public string PlotCommitDays(IEnumerable<QRepository> repos)
        {
            var day = repos.SelectMany(x => x.Commits).GroupBy(x => x.Date.DayOfWeek).Select(x => new
            {
                DayOfWeek = (int)x.First().Date.DayOfWeek,
                Day = x.Key,
                Total = x.Count()
            }).OrderByDescending(x => x.DayOfWeek);

            var bar = new BarSeries
            {
                ItemsSource = day.Select(x => new BarItem
                {
                    Value = x.Total
                }),
                LabelFormatString = "{0} Commits"
            };

            var axis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                ItemsSource = day.Select(x => x.Day)
            };

            return ExportToPng(bar, axis, "");
        }


        public string PlotTopCommits(IEnumerable<QRepository> repos)
        {
            var top10 = repos.OrderByDescending(x => x.Commits.Count()).Select(x => new
            {
                Repository = x.Name,
                Total = x.Commits.Count()
            }).Take(15);


            var bar = new BarSeries
            {
                ItemsSource = top10.Select(x => new BarItem
                {
                    Value = x.Total
                }),
                LabelFormatString = "{0} Commits",

            };

            var axis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                ItemsSource = top10.Select(x => x.Repository)
            };

            return ExportToPng(bar, axis, "");
        }


        public string PlotTopLanguages(IEnumerable<QRepository> repos)
        {
            var languages = repos.GroupBy(x => x.Language).Select(x => new
            {
                Langauge = x.Key != null ? x.Key : "Unknow",
                Total = x.Count()
            }).OrderBy(x => x.Total);


            var bar = new BarSeries
            {
                ItemsSource = languages.Select(x => new BarItem
                {
                    Value = x.Total
                }),
                LabelFormatString = "{0}"
            };

            var axis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                Key = "CakeAxis",
                ItemsSource = languages.Select(x => x.Langauge)
            };

            return ExportToPng(bar, axis, "");
        }
    }
}
