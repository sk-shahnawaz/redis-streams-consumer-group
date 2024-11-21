using StackExchange.Redis;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RedisStreamsConsumerGroup.Producer;

public sealed class WriterService(ILogger<WriterService> logger) : BackgroundService
{
    private readonly ILogger<WriterService> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting to write..");

        var connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(
            CommonResources.RedisConnectionString);
        
        var db = connectionMultiplexer.GetDatabase();

        if (!(await db.KeyExistsAsync(key: CommonResources.StreamName)) ||
             (await db.StreamGroupInfoAsync(key: CommonResources.StreamName))
             .All(x => x.Name != CommonResources.ConsumerGroupName))
        {
            await db.StreamCreateConsumerGroupAsync(
                key: CommonResources.StreamName, 
                groupName: CommonResources.ConsumerGroupName, 
                position: "0-0", 
                createStream: true);
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            await db.StreamAddAsync(
                key: CommonResources.StreamName, 
                streamField: CommonResources.StreamFieldName, 
                streamValue: DateTimeOffset.UtcNow.ToString());

            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }
}
