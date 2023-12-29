using Confluent.Kafka;

namespace MinimumApi.Kafka.Core
{
    public class GenericConsumer<TKey, TValue>(GenericConsumerConfig config) : IGenericConsumer<TKey, TValue>
    {
        private IConsumer<TKey, TValue> _consumer;
        private IConsumer<TKey, TValue> KafkaConsumer => _consumer ??= CreateConsumer();

        protected virtual IConsumer<TKey, TValue> CreateConsumer()
        {
            return new ConsumerBuilder<TKey, TValue>(config.ConsumerConfig)
           .SetKeyDeserializer(new KeyDeserializer<TKey>())
           .SetValueDeserializer(new GenericDeserializer<TValue>())
           .Build();
        }

        protected virtual void ProcessResult(Action<ConsumeResult<TKey, TValue>> onConsume, ConsumeResult<TKey, TValue> result)
        {
            if (IsCompatibleVersion(config, result))
                onConsume?.Invoke(result);
        }

        protected virtual bool IsCompatibleVersion(GenericConsumerConfig config, ConsumeResult<TKey, TValue> result)
        {
            var schemaVersion = result.GetSchemaVersion();
            return schemaVersion == null || config.CompatibleSchemaVersions == null || config.CompatibleSchemaVersions.Contains(schemaVersion.Value);
        }

        public virtual ConsumeResult<TKey, TValue> Consume(int timeoutInMillisecnonds, Action<ConsumeResult<TKey, TValue>> onConsume)
        {
            var result = KafkaConsumer.Consume(timeoutInMillisecnonds);
            if (result == null)
                throw new Exception("Polling person topic timed out.");

            ProcessResult(onConsume, result);
            StoreOffset(result);
            return result;
        }

        public virtual ConsumeResult<TKey, TValue> Consume(CancellationToken cancellationToken, Action<ConsumeResult<TKey, TValue>> onConsume)
        {
            var result = KafkaConsumer.Consume(cancellationToken);
            ProcessResult(onConsume, result);
            StoreOffset(result);
            return result;
        }

        public virtual void StoreOffset(ConsumeResult<TKey, TValue> result)
        {
            if (result == null) return;

            _consumer.StoreOffset(result);
        }

        public virtual void Subscribe(string topic)
        {
            KafkaConsumer.Subscribe(topic);
        }

        public virtual void Unsubscribe()
        {
            KafkaConsumer.Unsubscribe();
        }

        public virtual void Dispose()
        {
            KafkaConsumer.Dispose();
        }
    }
}
