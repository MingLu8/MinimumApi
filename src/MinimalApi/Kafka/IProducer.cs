using Confluent.Kafka;

namespace MinimumApi.Kafka
{
    public interface IProducer
    {
        Task<DeliveryResult<long, string>> PublishAsync(string topic, Message<long, string> message);
    }
}
