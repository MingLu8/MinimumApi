using MinimumApi.Entities;
using MinimumApi.Kafka.Core;

namespace MinimumApi.Kafka
{
    public interface IPersonConsumer : IGenericConsumer<Guid, Person> { }
    public class PersonConsumer(PersonConsumerConfig config) : GenericConsumer<Guid, Person>(config), IPersonConsumer
    {
       
    }
}
