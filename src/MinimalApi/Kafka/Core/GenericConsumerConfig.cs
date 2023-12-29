using Confluent.Kafka;

namespace MinimumApi.Kafka.Core
{
    public class GenericConsumerConfig
    {
        public ConsumerConfig ConsumerConfig { get; set; }
        public List<int>? CompatibleSchemaVersions { get; set; }
        public string Topic { get; set; }
    }
}
