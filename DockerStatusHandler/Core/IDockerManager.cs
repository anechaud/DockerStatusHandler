using System;
using Docker.DotNet.Models;

namespace DockerStatusHandler.Core
{
	public interface IDockerManager
	{
        public Task RemoveContainer(string containerId);
        public Task MonitorContainerEvents(ContainerEventsParameters containerEventsParameters, Progress<Message> progress);
        public Task<IList<ContainerListResponse>> ListContainers();
        public Task<bool> CheckContainerExistance(string containerId);
    }
}

