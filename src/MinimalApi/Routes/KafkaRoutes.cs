using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using MinimumApi.Entities;
using MinimumApi.Kafka;
using System.Text.Json;
using static Confluent.Kafka.ConfigPropertyNames;

namespace MinimumApi.Routes
{
    public static class KafkaRoutes
    {
        private static CancellationToken _cancellationtoken;

        public static void UseKafkaRoutes(this WebApplication app)
        {           
            var customerRoutes = app.MapGroup("kafka").WithTags("kafka");
            customerRoutes.MapPost("/produce", ProduceMessage);
            customerRoutes.MapPost("/consume", ConsumeMessage);
            customerRoutes.MapPost("/subscribe", SubscribeToTopic);
            customerRoutes.MapPost("/unsubscribe", UnsubscribeToTopic);          
        }

        private static Task UnsubscribeToTopic(IGenericConsumer<Guid, Person> consumer)
        {
            consumer.Unsubscribe();
            return Task.CompletedTask;
        }

        private static Task SubscribeToTopic(IGenericConsumer<Guid, Person> consumer, PersonConsumerConfig config)
        {
            consumer.Subscribe(config.Topic);
            return Task.CompletedTask;
        }

        private async static Task<IResult> ProduceMessage(int? schemaVersion,  Person person, IGenericProducer<Guid, Person> producer, PersonConsumerConfig config)
        {
            var message = new Message<Guid, Person> { Key = person.CompanyId, Value = person };
            if(schemaVersion.HasValue)
            {
                if(message.Headers == null)
                {
                    message.Headers = new Headers();
                }
                message.Headers.Add("SchemaVersion", BitConverter.GetBytes(schemaVersion.Value));
            }
            var result = await producer.PublishAsync(config.Topic, message);
            return TypedResults.Ok(result);
        }
                
        private static IResult ConsumeMessage(IGenericConsumer<Guid, Person> consumer, CancellationToken cancellationToken, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger("ConsumeMessageLogger");
            ConsumeResult<Guid, Person> consumeResult = null;
            var processed = false;
            //while (!cancellationToken.IsCancellationRequested)
            //{
                try
                {
                    //var consumeResult = consumer.Consume(cancellationToken, result =>
                    consumeResult = consumer.Consume(10000, result =>
                    {
                        logger.LogInformation(JsonSerializer.Serialize(result?.Message.Value));
                        processed = true;
                        //do message processing
                    });
                }
                catch (Exception ex)
                {
                    logger.LogError($"consumer.Consume failed: {ex.Message}.");
                }
            //}

            //if(cancellationToken.IsCancellationRequested)
            //    logger.LogInformation("Consume message cancelled.");

            return TypedResults.Ok(new { schemaVersion = consumeResult.GetSchemaVersion(), processed, person = consumeResult.Message.Value });
        }      
    }
}
