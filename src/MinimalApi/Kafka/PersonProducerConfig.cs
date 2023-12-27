using Confluent.Kafka;

namespace MinimumApi.Kafka
{
    public class PersonProducerConfig
    {
        public ProducerConfig ProducerConfig { get; set; }
        public int SchemaVersion { get; set; }
    }
}
