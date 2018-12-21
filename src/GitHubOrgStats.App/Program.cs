using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Threading.Tasks;

namespace GithubOrgStats.App {
    class Program {

        public static async Task Main(string[] args) {

            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

            var collection = new ServiceCollection();
            collection.AddLogging(config => {
                config.AddSerilog();
            });

            collection.AddSingleton<MainClass>();

            var provider = collection.BuildServiceProvider();
            var main = provider.GetService<MainClass>();
            await main.GetRepositories();
        }
    }
}
