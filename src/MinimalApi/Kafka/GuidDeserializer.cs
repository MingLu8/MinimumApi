using Confluent.Kafka;
using HotChocolate.Types.Relay;
using System.Text;

namespace MinimumApi.Kafka
{
    public class GuidDeserializer : IDeserializer<Guid>
    {
        public Guid Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {            
            return Guid.Parse(Encoding.UTF8.GetString(data));
        }
    }
}
