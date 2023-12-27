using Confluent.Kafka;
using MinimumApi.Entities;
using System.Text;
using System.Text.Json;

namespace MinimumApi.Kafka
{
    public class PersonDeserializer : IDeserializer<Person>
    {
        public Person Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            var json = Encoding.Unicode.GetString(data.ToArray());
            return JsonSerializer.Deserialize<Person>(json);
        }
    }
}
