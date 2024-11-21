# About

This sample application demonstrates how Redis's **Streams** data-structure works w.r.t _consumer groups_.
When two applications / services with same consumer name connect to the same consumer group of a stream,
Redis performs load-balancing internally among the applications / services while delivering the messages
coming in the stream.

## Developed with:
- .NET 9 / C# 13

## External dependencies:
- Redis (assumed to be running on port `6379`)

## Components:
- `WriterService` --> Producer
- `ReaderService1` --> Consumer 1
- `ReaderService1` --> Consumer 2
