using System;
using Docker.DotNet.Models;
using DockerStatusHandler.Core;
using Microsoft.Extensions.Hosting;
namespace DockerStatusHandler
{
	public class MonitorContainer : BackgroundService
	{
        private readonly IDockerManager _dockerManager;
        public MonitorContainer(IDockerManager dockerManager)
		{
            _dockerManager = dockerManager;
		}

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                Progress<Message> progress = new Progress<Message>();
                progress.ProgressChanged += async (sender, message) =>
                {
                    MonitorComputation(message);
                    MonitorContainers(message);
                    await DeleteContainerAsync(message);
                };

                await _dockerManager.MonitorContainerEvents(new ContainerEventsParameters(), progress: progress);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong while monitorning containers", ex.Message);
            }
        }

        private void MonitorComputation(Message message)
        {
            string val = string.Empty;
            var exitCode = message.Actor?.Attributes.TryGetValue("exitCode", out val);
            Console.WriteLine(val);
        }

        private void MonitorContainers(Message message)
        {
            Console.WriteLine($"{message.Actor.ID} -- {message.Status}");
        }

        private async Task DeleteContainerAsync(Message message)
        {
            if (!string.IsNullOrEmpty(message.Status) && message.Status.Equals("stop", StringComparison.InvariantCultureIgnoreCase))
            {
                await _dockerManager.RemoveContainer(message.ID);
                Console.WriteLine($"Container {message.ID} is removed");
            }
        }
    }
}