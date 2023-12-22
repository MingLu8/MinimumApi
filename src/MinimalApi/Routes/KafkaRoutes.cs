using Confluent.Kafka;
using MinimumApi.Kafka;

namespace MinimumApi.Routes
{
    public static class KafkaRoutes
    {
        
        public static void AddKafkaRoutes(this WebApplication app)
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
                
        private static IResult ConsumeMessage(string topic, IConsumer consumer)
        {
            consumer.Subscribe(topic);
            var result = consumer.Consume(30000);
            consumer.Unsubscribe();
            return TypedResults.Ok(result?.Message.Value);
        }      
    }
}
