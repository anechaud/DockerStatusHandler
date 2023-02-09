using System;
using Docker.DotNet;
using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;
using Docker.DotNet.Models;

namespace DockerStatusHandler.Core
{
	public class DockerManager : IDockerManager, IAsyncDisposable
    {
        private static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        private static readonly bool IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        private static readonly bool IsMac = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        private readonly IConfiguration Configuration;
        private readonly DockerClient _dockerClient;
        public DockerManager(IConfiguration configuration)
		{
            _dockerClient = new DockerClientConfiguration(new Uri(DockerApiUri())).CreateClient();
            Configuration = configuration;
        }

        public ValueTask DisposeAsync()
        {
            _dockerClient.Dispose();
            return new ValueTask();
        }
        public async Task RemoveContainer(string containerId)
        {
            await _dockerClient.Containers.RemoveContainerAsync(
                                                    containerId,
                                                    new ContainerRemoveParameters(),
                                                    CancellationToken.None);
        }

        /// <summary>
        /// List all containers in the docker daemon
        /// </summary>
        /// <returns></returns>
        public async Task<IList<ContainerListResponse>> ListContainers()
        {
            IList<ContainerListResponse> containers = await _dockerClient.Containers.ListContainersAsync(
                                                     new ContainersListParameters()
                                                     {
                                                         All = true,
                                                     });
            return containers;
        }

        private static string DockerApiUri()
        {
            if (IsWindows)
                return "npipe://./pipe/docker_engine";

            if (IsLinux || IsMac)
                return "unix:///var/run/docker.sock";

            throw new Exception(
                "Was unable to determine what OS this is running on, does not appear to be Windows or Linux!?");
        }

        public async Task MonitorContainerEvents(ContainerEventsParameters containerEventsParameters, Progress<Message> progress)
        {
            await _dockerClient.System.MonitorEventsAsync(new ContainerEventsParameters(), progress: progress);
        }

        public async Task<bool> CheckContainerExistance(string containerId)
        {
            var containers = await ListContainers();
            return containers.Any(x => x.ID == containerId);
        }
    }
}

