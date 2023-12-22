// See https://aka.ms/new-console-template for more information
using Confluent.Kafka;

Console.WriteLine("Hello, World!");
var config = new ConsumerConfig
{
    BootstrapServers = "localhost:22181,localhost:29092",
    AutoOffsetReset = AutoOffsetReset.Earliest,
    GroupId = "console consumer"
};

using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

consumer.Subscribe("xxx-topic");
while (true)
{
    try
    {
        var consumeResult = consumer.Consume(5000);
        Console.WriteLine($"Received message: {consumeResult?.Message?.Value}");

        // Process the message here
        if (consumeResult != null)
            consumer.Commit(consumeResult);
    }
    catch (ConsumeException e)
    {
        Console.WriteLine($"Error consuming message: {e.Error.Reason}");
    }
}
