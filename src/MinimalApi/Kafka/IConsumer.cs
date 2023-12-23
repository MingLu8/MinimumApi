using Confluent.Kafka;
using System.Threading;

namespace MinimumApi.Kafka
{
    public interface IConsumer
    {
        void Subscribe(string topic);
        void Unsubscribe();
        ConsumeResult<Ignore, string> Consume(int timeoutInMillisecnonds);
        ConsumeResult<Ignore, string> Consume(CancellationToken cancellationToken);
        void StoreOffset(ConsumeResult<Ignore, string> result);
    }
}
