using Confluent.Kafka;
using GreenDonut;
using Microsoft.Extensions.Logging;
using MinimumApi.Entities;
using MinimumApi.Kafka;
using System.Data.SqlTypes;
using System.Text.Json;
using static Confluent.Kafka.ConfigPropertyNames;

namespace MinimumApi.Routes
{
    public static class KafkaRoutes
    {
        private static CancellationTokenSource _cancellationtoken;

        public static void UseKafkaRoutes(this WebApplication app)
        {           
            var customerRoutes = app.MapGroup("kafka").WithTags("kafka");
            customerRoutes.MapPost("/produce", ProduceMessage);
            customerRoutes.MapPost("/consume", ConsumeMessage);
            customerRoutes.MapPost("/stop-consume", StopConsume);
        }
  
        private static Task StopConsume()
        {
            _cancellationtoken.Cancel();
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
                
        private static IResult ConsumeMessage(IGenericConsumer<Guid, Person> consumer, ILoggerFactory loggerFactory, PersonConsumerConfig config)
        {
            _cancellationtoken = new CancellationTokenSource();
            consumer.Subscribe(config.Topic);
            var logger = loggerFactory.CreateLogger("ConsumeMessageLogger");
            var results = new List<Tuple<bool, ConsumeResult<Guid, Person>>>();
           
            while (!_cancellationtoken.IsCancellationRequested)
            {
                try
                {
                    var processed = false;
                    var consumeResult = consumer.Consume(10000, result =>
                    {
                        logger.LogInformation(JsonSerializer.Serialize(result?.Message.Value));
                        processed = true;
                        //do message processing
                    });
                    results.Add(new Tuple<bool, ConsumeResult<Guid, Person>>(processed, consumeResult));
                }
                catch (Exception ex)
                {
                    logger.LogError($"consumer.Consume failed: {ex.Message}.");
                }
            }

            consumer.Unsubscribe();

            return TypedResults.Ok(results.Select(a=>new { schemaVersion = a.Item2.GetSchemaVersion(), processed = a.Item1, person = a.Item2.Message.Value }));
        }      
    }
}
