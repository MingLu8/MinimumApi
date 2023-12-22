// See https://aka.ms/new-console-template for more information
using Confluent.Kafka;
using System.Net;

var c = new ProducerConfig
{
    BootstrapServers = "localhost:29092",
    EnableDeliveryReports = true,
    ClientId = Dns.GetHostName(),

    // Emit debug logs for message writer process, remove this setting in production
    Debug = "msg",

    // retry settings:
    // Receive acknowledgement from all sync replicas
    Acks = Acks.All,
    // Number of times to retry before giving up
    MessageSendMaxRetries = 3,
    // Duration to retry before next attempt
    RetryBackoffMs = 1000,
    // Set to true if you don't want to reorder messages on retry
    EnableIdempotence = true
};
using var producer = new ProducerBuilder<long, string>(c)
.SetKeySerializer(Serializers.Int64)
.SetValueSerializer(Serializers.Utf8)
.SetLogHandler((_, message) =>
    Console.WriteLine($"Facility: {message.Facility}-{message.Level} Message: {message.Message}"))
.SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}. Is Fatal: {e.IsFatal}"))
.Build();
try
{
    Console.WriteLine("\nProducer loop started...\n\n");
    for (var character = 0; character <= 200; character++)
    {
        var message = $"Character #{character} sent at {DateTime.Now:yyyy-MM-dd_HH:mm:ss}";

        var deliveryReport = await producer.ProduceAsync("xxx-topic",
            new Message<long, string>
            {
                Key = DateTime.UtcNow.Ticks,
                Value = message
            });

        Console.WriteLine($"Message sent (value: '{message}'). Delivery status: {deliveryReport.Status}");
        if (deliveryReport.Status != PersistenceStatus.Persisted)
        {
            // delivery might have failed after retries. This message requires manual processing.
            Console.WriteLine(
                $"ERROR: Message not ack'd by all brokers (value: '{message}'). Delivery status: {deliveryReport.Status}");
        }

        Thread.Sleep(TimeSpan.FromSeconds(2));
    }
}
catch (ProduceException<long, string> e)
{
    Console.WriteLine($"Permanent error: {e.Message} for message (value: '{e.DeliveryResult.Value}')");
    Console.WriteLine("Exiting producer...");
}