using Confluent.Kafka;

namespace MinimumApi.Kafka
{
    public static class KafkaExtensions
    {
        public static WebApplicationBuilder AddKafka(this WebApplicationBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            var consumerConfig = new ConsumerConfig();
            builder.Configuration.Bind("ConsumerConfig", consumerConfig);
            builder.Services.AddSingleton(_ => consumerConfig);
            builder.Services.AddSingleton<IConsumer, Consumer>();


            var producerConfig = new ProducerConfig();
            builder.Configuration.Bind("ProducerConfig", producerConfig);
            builder.Services.AddSingleton(_ => producerConfig);
            builder.Services.AddSingleton<IProducer, Producer>();
            return builder;
        }
    }
}
