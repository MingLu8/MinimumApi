using Confluent.Kafka;

namespace MinimumApi.Kafka.Core
{
    public class GenericProducer<TKey, TValue>(GenericProducerConfig config) : IGenericProducer<TKey, TValue>, IDisposable
    {
        private IProducer<TKey, TValue> _producer;
        private byte[] _schemaVersion = BitConverter.GetBytes(config.SchemaVersion);
        private IProducer<TKey, TValue> KafkaProducer => _producer ??= CreateProcuder();
        protected virtual IProducer<TKey, TValue> CreateProcuder()
        {
            return new ProducerBuilder<TKey, TValue>(config.ProducerConfig)
                .SetKeySerializer(new KeySerializer<TKey>())
                .SetValueSerializer(new GenericSerializer<TValue>())
                .SetLogHandler((_, message) =>
                    Console.WriteLine($"Facility: {message.Facility}-{message.Level} Message: {message.Message}"))
                .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}. Is Fatal: {e.IsFatal}"))
                .Build();
        }

        public virtual Task<DeliveryResult<TKey, TValue>> PublishAsync(string topic, Message<TKey, TValue> message)
        {
            if (message.Headers == null)
                message.Headers = new Headers();

            if (!message.Headers.Any(a => string.Compare(a.Key, "SchemaVersion", true) == 0))
                message.Headers.Add("SchemaVersion", _schemaVersion);

            return KafkaProducer.ProduceAsync(topic, message);
        }

        public virtual void Dispose()
        {
            KafkaProducer.Dispose();
        }

    }
}
