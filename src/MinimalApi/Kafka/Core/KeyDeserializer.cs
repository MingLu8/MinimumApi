using Confluent.Kafka;
using System.Text;

namespace MinimumApi.Kafka.Core
{
    public class KeyDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            Type type = typeof(T);
            if (type == typeof(string)) return (T)Convert.ChangeType(Encoding.UTF8.GetString(data), typeof(T));
            if (type == typeof(int)) return (T)Convert.ChangeType(BitConverter.ToInt32(data), typeof(T));
            if (type == typeof(long)) return (T)Convert.ChangeType(BitConverter.ToInt64(data), typeof(T));
            if (type == typeof(Guid)) return (T)Convert.ChangeType(Guid.Parse(Encoding.UTF8.GetString(data)), typeof(T));

            throw new Exception("Not supported key type, the supported types are: int, long, string and guid.");
        }
    }
}
