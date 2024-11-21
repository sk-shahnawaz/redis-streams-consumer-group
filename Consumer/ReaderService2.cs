using StackExchange.Redis;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace RedisStreamsConsumerGroup.Consumer;
public class ReaderService2(ILogger<ReaderService2> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Either check & create stream with consumer group or wait till those
        // get created by the producer.
        await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        
        logger.LogInformation("Starting to read (Reader 2)..");

        var connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(
            CommonResources.RedisConnectionString);
        
        var db = connectionMultiplexer.GetDatabase();

        while (!stoppingToken.IsCancellationRequested)
        {
            var result = await db.StreamReadGroupAsync(
                key: CommonResources.StreamName,
                groupName: CommonResources.ConsumerGroupName,
                
                // Consumer names should match within the consumer group so that
                // Redis performs load-balancing.
                consumerName: "reader",
                position: StreamPosition.NewMessages,
                count: 1);

            if (result.Length == 1)
            {
                logger.LogDebug("Reader 2 read message {messageId}, value: {streamFieldValue}",
                    result[0].Id, result[0].Values[0].Value);

                await db.StreamAcknowledgeAsync(
                    key: CommonResources.StreamName,
                    groupName: CommonResources.ConsumerGroupName,
                    messageId: result[0].Id);
            }

            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }

        await Task.CompletedTask;
    }
}
