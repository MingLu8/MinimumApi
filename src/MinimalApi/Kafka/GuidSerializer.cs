using Confluent.Kafka;
using System.Text;

namespace MinimumApi.Kafka
{
    public class GuidSerializer : ISerializer<Guid>
    {
        public byte[] Serialize(Guid data, SerializationContext context)
        {
            return Encoding.UTF8.GetBytes(data.ToString("D"));
        }
    }
}
