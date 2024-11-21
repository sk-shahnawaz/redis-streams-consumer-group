using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

using RedisStreamsConsumerGroup.Producer;
using RedisStreamsConsumerGroup.Consumer;

namespace RedisStreamsConsumerGroup;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args);

        host.ConfigureLogging(options =>
        {
            options.SetMinimumLevel(LogLevel.Debug);
        });

        host.ConfigureHostOptions(options =>
        {
            options.ServicesStartConcurrently = true;
            options.ServicesStopConcurrently = true;
        });

        host.ConfigureServices(options =>
        {
            options.AddHostedService<WriterService>();
            options.AddHostedService<ReaderService1>();
            options.AddHostedService<ReaderService2>();
        });

        await host.RunConsoleAsync();
    }
}
