using Confluent.Kafka;
using MinimumApi.Entities;
using MinimumApi.Kafka.Core;

namespace MinimumApi.Kafka
{
    public interface IPersonProducer : IGenericProducer<Guid, Person> { }

    public class PersonProducer(PersonProducerConfig config) : GenericProducer<Guid, Person>(config), IPersonProducer
    {
       
    }
}
