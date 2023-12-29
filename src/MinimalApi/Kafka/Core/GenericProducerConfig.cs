using Confluent.Kafka;

namespace MinimumApi.Kafka.Core
{
    public class GenericProducerConfig
    {
        public ProducerConfig ProducerConfig { get; set; }
        public int SchemaVersion { get; set; }
        public string Topic { get; set; }

    }
}
