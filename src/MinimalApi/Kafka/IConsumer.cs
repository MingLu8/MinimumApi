﻿using Confluent.Kafka;
using MinimumApi.Entities;

namespace MinimumApi.Kafka
{
    public interface IConsumer<TKey, TValue>
    {
        void Subscribe(string topic);
        void Unsubscribe();
        ConsumeResult<TKey, TValue> Consume(int timeoutInMillisecnonds, Action<ConsumeResult<TKey, TValue>> onConsume);
        ConsumeResult<TKey, TValue> Consume(CancellationToken cancellationToken, Action<ConsumeResult<TKey, TValue>> onConsume);
        void StoreOffset(ConsumeResult<TKey, TValue> result);
    }
}
