using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Docker.DotNet;

namespace DockerStatusHandler
{
	public class Client
    {
        private static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        private static readonly bool IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        private static readonly bool IsMac = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        public DockerClient Connect()
        {
            var client = new DockerClientConfiguration(new Uri(DockerApiUri())).CreateClient();
            return client;
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
    }
}

