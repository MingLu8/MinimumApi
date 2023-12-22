using Confluent.Kafka;

namespace MinimumApi.Kafka
{
    public class Consumer : IConsumer, IDisposable
    {
        private readonly ConsumerConfig _config;

        private IConsumer<Ignore, string> _consumer;
        private IConsumer<Ignore, string> KafkaConsumer => _consumer ??= new ConsumerBuilder<Ignore, string>(_config).Build();

        public Consumer(ConsumerConfig config)
        {
            _config = config;
        }

        public ConsumeResult<Ignore, string> Consume(int timeoutInMillisecnonds)
        {
            return KafkaConsumer.Consume(timeoutInMillisecnonds);
        }

        public void Dispose()
        {
            KafkaConsumer.Dispose();
        }

        public void Subscribe(string topic)
        {
            KafkaConsumer.Subscribe(topic);
        }

        public void Unsubscribe()
        {
            KafkaConsumer.Unsubscribe();
        }
    }
}
