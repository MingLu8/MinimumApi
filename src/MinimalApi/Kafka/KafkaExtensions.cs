using Confluent.Kafka;
using HotChocolate.Types.Relay;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using MinimumApi.Entities;

namespace MinimumApi.Kafka
{
    public static class KafkaExtensions
    {
        public static WebApplicationBuilder AddKafka(this WebApplicationBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            var personConsumerConfig = new PersonConsumerConfig();
            builder.Configuration.Bind("PersonConsumerConfig", personConsumerConfig);
            builder.Services.AddSingleton(_ => personConsumerConfig);
            builder.Services.AddSingleton<IConsumer<Guid, Person>, PersonConsumer>();


            var personProducerConfig = new PersonProducerConfig();
            builder.Configuration.Bind("PersonProducerConfig", personProducerConfig);
            builder.Services.AddSingleton(_ => personProducerConfig);
            builder.Services.AddSingleton<IProducer<Guid, Person>, PersonProducer>();
            return builder;
        }

        public static int? GetSchemaVersion<TKey, TValue>(this ConsumeResult<TKey, TValue> consumeResult)
        {          
            var valueBytes = consumeResult?.Message.Headers.FirstOrDefault(a => string.Compare(a.Key, "SchemaVersion", true) == 0)?.GetValueBytes();
            return valueBytes == null ? null : BitConverter.ToInt32(valueBytes);            
        }
    }
}
