using Confluent.Kafka;

namespace MinimumApi.Kafka.Core
{
    public interface IGenericProducer<TKey, TValue>
    {
        Task<DeliveryResult<TKey, TValue>> PublishAsync(string topic, Message<TKey, TValue> message);
    }
}
