namespace RedisStreamsConsumerGroup;

internal static class CommonResources
{
    internal const string RedisConnectionString = "localhost:6379";
    
    internal const string StreamName = "tick";
    internal const string ConsumerGroupName = "c-group";

    internal const string StreamFieldName = "current-utc-time";
}
