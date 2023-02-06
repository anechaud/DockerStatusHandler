using System;
using DockerStatusHandler;
using DockerStatusHandler.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GenericSiteCrawler.Client
{
    public class Startup
    {
        public static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            services.AddScoped<IDockerManager, DockerManager>();
            services.AddHostedService<MonitorContainer>();
        }
    }
}