using Confluent.Kafka;

namespace MinimumApi.Kafka.Core
{
    public static class KafkaExtensions
    {
        public static int? GetSchemaVersion<TKey, TValue>(this ConsumeResult<TKey, TValue> consumeResult)
        {
            var valueBytes = consumeResult?.Message.Headers.FirstOrDefault(a => string.Compare(a.Key, "SchemaVersion", true) == 0)?.GetValueBytes();
            return valueBytes == null ? null : BitConverter.ToInt32(valueBytes);
        }

        public static void AddKafkaConfig<ConfigType>(this WebApplicationBuilder builder, string configSectionName) where ConfigType : class
        {
            var config = Activator.CreateInstance<ConfigType>();
            builder.Configuration.Bind(configSectionName, config);
            builder.Services.AddSingleton(config);
        }

        public static WebApplicationBuilder ConfigKafka(this WebApplicationBuilder builder)
        {
            builder.AddKafkaConfig<PersonConsumerConfig>("PersonConsumerConfig");
            builder.AddKafkaConfig<PersonProducerConfig>("PersonProducerConfig");
            builder.AddKafkaConfig<RoutePointConsumerConfig>("RoutePointConsumerConfig");
            builder.AddKafkaConfig<RoutePointProducerConfig>("RoutePointProducerConfig");

            builder.Services.AddSingleton<IPersonConsumer, PersonConsumer>();
            builder.Services.AddSingleton<IPersonProducer, PersonProducer>();
            builder.Services.AddSingleton<IRoutePointConsumer, RoutePointConsumer>();
            builder.Services.AddSingleton<IRoutePointProducer, RoutePointProducer>();

            return builder;
        }
    }
}
