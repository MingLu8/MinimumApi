using Confluent.Kafka;

namespace MinimumApi.Kafka
{
    public class PersonConsumerConfig
    {
        public ConsumerConfig ConsumerConfig { get; set; }
        public List<int>? CompatibleSchemaVersions { get; set; }
        public string Topic {  get; set; }
    }
}
