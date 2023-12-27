using Confluent.Kafka;
using MinimumApi.Entities;

namespace MinimumApi.Kafka
{
    public class PersonProducer(PersonProducerConfig config) : IGenericProducer<Guid, Person>, IDisposable
    {
        private IProducer<Guid, Person> _producer;
        private byte[] _schemaVersion = BitConverter.GetBytes(config.SchemaVersion);
        private IProducer<Guid, Person> KafkaProducer => _producer ??= new ProducerBuilder<Guid, Person>(config.ProducerConfig)
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

            if(!message.Headers.Any(a=> string.Compare(a.Key, "SchemaVersion", true) == 0))
                message.Headers.Add("SchemaVersion", _schemaVersion);

            return KafkaProducer.ProduceAsync(topic, message);
        }

        public void Dispose()
        {
            KafkaProducer.Dispose();
        }

    }
}
