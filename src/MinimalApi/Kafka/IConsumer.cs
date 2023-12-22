using Confluent.Kafka;

namespace MinimumApi.Kafka
{
    public interface IConsumer
    {
        void Subscribe(string topic);
        void Unsubscribe();
        ConsumeResult<Ignore, string> Consume(int timeoutInMillisecnonds);
    }
}
