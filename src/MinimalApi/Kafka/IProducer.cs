using Confluent.Kafka;

namespace MinimumApi.Kafka
{
    public interface IProducer<TKey, TValue>
    {
        Task<DeliveryResult<TKey, TValue>> PublishAsync(string topic, Message<TKey, TValue> message);
    }
}
