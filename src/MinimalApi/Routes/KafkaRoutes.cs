using Microsoft.Extensions.Logging;
using MinimumApi.Entities;
using MinimumApi.Kafka;
using System.Text.Json;

namespace MinimumApi.Routes
{
    public static class KafkaRoutes
    {
        
        public static void UseKafkaRoutes(this WebApplication app)
        {           
            var customerRoutes = app.MapGroup("kafka").WithTags("kafka");
            customerRoutes.MapPost("/produce", ProduceMessage);
            customerRoutes.MapPost("/consume", ConsumeMessage);          
        }       

        private async static Task<IResult> ProduceMessage(string topic, Person person, IProducer<Guid, Person> producer)
        {
            var result = await producer.PublishAsync(topic, new Confluent.Kafka.Message<Guid, Person> { Key = person.CompanyId, Value = person});
            return TypedResults.Ok(result);
        }
                
        private static IResult ConsumeMessage(string topic, IConsumer<Guid, Person> consumer, CancellationToken cancellationToken, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger("ConsumeMessageLogger");
            consumer.Subscribe(topic);
            
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(cancellationToken, result =>
                    {
                        logger.LogInformation(JsonSerializer.Serialize(result?.Message.Value));
                        //do message processing
                    });
                }
                catch (Exception ex)
                {
                    logger.LogError($"consumer.Consume failed: {ex.Message}.");
                }
            }

            if(cancellationToken.IsCancellationRequested)
                logger.LogInformation("Consume message cancelled.");

            consumer.Unsubscribe();
            return TypedResults.Ok();
        }      
    }
}
