using Confluent.Kafka;
using MinimumApi.Entities;
using System.Text;
using System.Text.Json;

namespace MinimumApi.Kafka
{
    public class PersonSerializer : ISerializer<Person>
    {
        public byte[] Serialize(Person data, SerializationContext context)
        {
            return Encoding.Unicode.GetBytes(JsonSerializer.Serialize(data));
        }
    }
}
