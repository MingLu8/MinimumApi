using Confluent.Kafka;
using System.Text;
using System.Text.Json;

namespace MinimumApi.Kafka.Core
{
    public class GenericSerializer<T> : ISerializer<T>
    {
        public byte[] Serialize(T data, SerializationContext context)
        {
            return Encoding.Unicode.GetBytes(JsonSerializer.Serialize(data));
        }
    }
}
