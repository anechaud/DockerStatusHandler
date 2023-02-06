using GenericSiteCrawler.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
public class Program
{
    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args).ConfigureServices(Startup.ConfigureServices);

    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }
}