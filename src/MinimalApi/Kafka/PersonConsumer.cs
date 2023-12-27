using Confluent.Kafka;
using MinimumApi.Entities;

namespace MinimumApi.Kafka
{
    public class PersonConsumer(PersonConsumerConfig config) : IConsumer<Guid, Person>
    {
        private Confluent.Kafka.IConsumer<Guid, Person> _consumer;
        private Confluent.Kafka.IConsumer<Guid, Person> KafkaConsumer => _consumer ??= new ConsumerBuilder<Guid, Person>(config.ConsumerConfig)
            
             .SetKeyDeserializer(new GuidDeserializer())
             .SetValueDeserializer(new PersonDeserializer())
            .Build();

        public ConsumeResult<Guid, Person> Consume(int timeoutInMillisecnonds, Action<ConsumeResult<Guid, Person>> onConsume)
        {
            var result = KafkaConsumer.Consume(timeoutInMillisecnonds);
            ProcessResult(onConsume, result);
            return result;
        }

        public ConsumeResult<Guid, Person> Consume(CancellationToken cancellationToken, Action<ConsumeResult<Guid, Person>> onConsume)
        {
            var result = KafkaConsumer.Consume(cancellationToken);
            ProcessResult(onConsume, result);
            return result;
        }

        public void StoreOffset(ConsumeResult<Guid, Person> result)
        {
            if (result == null) return;

            _consumer.StoreOffset(result);
        }

        public void Subscribe(string topic)
        {
            KafkaConsumer.Subscribe(topic);
        }

        public void Unsubscribe()
        {
            KafkaConsumer.Unsubscribe();
        }

        public void Dispose()
        {
            KafkaConsumer.Dispose();
        }

        private void ProcessResult(Action<ConsumeResult<Guid, Person>> onConsume, ConsumeResult<Guid, Person> result)
        {
            var schemaVersion = result.GetSchemaVersion();
            if (schemaVersion == null || config.CompatibleSchemaVersions == null || config.CompatibleSchemaVersions.Contains(schemaVersion.Value))
                onConsume?.Invoke(result);
            StoreOffset(result);
        }

    }
}
