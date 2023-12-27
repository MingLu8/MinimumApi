using Confluent.Kafka;
using MinimumApi.Entities;

namespace MinimumApi.Kafka
{
    public class PersonProducer(PersonProducerConfig config) : IProducer<Guid, Person>, IDisposable
    {
        private Confluent.Kafka.IProducer<Guid, Person> _producer;
        private byte[] _schemaVersion = BitConverter.GetBytes(1);
        private Confluent.Kafka.IProducer<Guid, Person> KafkaProducer => _producer ??= new ProducerBuilder<Guid, Person>(config.ProducerConfig)
                .SetKeySerializer(new GuidSerializer())
                .SetValueSerializer(new PersonSerializer())
                .SetLogHandler((_, message) =>
                    Console.WriteLine($"Facility: {message.Facility}-{message.Level} Message: {message.Message}"))
                .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}. Is Fatal: {e.IsFatal}"))
                .Build();

        public Task<DeliveryResult<Guid, Person>> PublishAsync(string topic, Message<Guid, Person> message)
        {
            if(message.Headers == null)
                message.Headers = new Headers();

            message.Headers.Add("SchemaVersion", _schemaVersion);
            return KafkaProducer.ProduceAsync(topic, message);
        }

        public void Dispose()
        {
            KafkaProducer.Dispose();
        }

    }
}
