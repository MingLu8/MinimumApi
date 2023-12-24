using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using MinimumApi.Kafka;
using System.Threading;

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

        private async static Task<IResult> ProduceMessage(string topic, string message, IProducer producer)
        {
            var result = await producer.PublishAsync(topic, new Message<long, string> { Key = DateTime.Now.Ticks, Value = message});
            return TypedResults.Ok(result);
        }
                
        private static IResult ConsumeMessage(string topic, IConsumer consumer, CancellationToken cancellationToken, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger("ConsumeMessageLogger");
            consumer.Subscribe(topic);
            
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(cancellationToken);
                    logger.LogInformation(consumeResult?.Message.Value);
                    consumer.StoreOffset(consumeResult);  //at-least once delivery model
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
