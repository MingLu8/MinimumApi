using Confluent.Kafka;
using System.Text;

namespace MinimumApi.Kafka.Core
{
    public class KeySerializer<T> : ISerializer<T>
    {
        public byte[] Serialize(T data, SerializationContext context)
        {
            Type type = typeof(T);
            if (type == typeof(int)) return BitConverter.GetBytes((int)Convert.ChangeType(data, typeof(int)));
            if (type == typeof(long)) return BitConverter.GetBytes((long)Convert.ChangeType(data, typeof(long)));
            if (type == typeof(string)) return Encoding.UTF8.GetBytes((string)Convert.ChangeType(data, typeof(string)));
            if (type == typeof(Guid)) return Encoding.UTF8.GetBytes(((Guid)Convert.ChangeType(data, typeof(Guid))).ToString("D"));

            throw new Exception("Not supported key type, the supported types are: int, long, string and guid.");
        }
    }
}
