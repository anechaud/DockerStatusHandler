using Docker.DotNet.Models;
using DockerStatusHandler;

class Program
{
    static async Task Main(string[] args)
    {
        var cl = new Client().Connect();
        Console.WriteLine("Start");
        Progress<Message> progress = new Progress<Message>();
        progress.ProgressChanged += (sender, message) =>
        {

            Console.WriteLine($"{message.Actor.ID} -- {message.Status}");
        };

        await cl.System.MonitorEventsAsync(new ContainerEventsParameters(),progress: progress);
    }
}