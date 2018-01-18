using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Baibaocp.LotteryOrdering.WebApi
{
    /// <inheritdoc/>
    public class Program
    {
        /// <inheritdoc/>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        /// <inheritdoc/>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
