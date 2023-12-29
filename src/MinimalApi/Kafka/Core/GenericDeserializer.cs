using Confluent.Kafka;
using System.Text;
using System.Text.Json;

namespace MinimumApi.Kafka.Core
{
    public class GenericDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            var json = Encoding.Unicode.GetString(data.ToArray());
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
