using Confluent.Kafka;
using MinimumApi.Entities;
using MinimumApi.Kafka;
using MinimumApi.Kafka.Core;
using System.Text.Json;

namespace MinimumApi.Routes
{
    public static class KafkaStreamingRoutes
    {
        private static CancellationTokenSource _cancellationtoken;

        public static void UseRoutePointsRoutes(this WebApplication app)
        {           
            var customerRoutes = app.MapGroup("RoutePoints").WithTags("RoutePoints");
            customerRoutes.MapPost("/simulate-route-point-stream", SimulateRoutePointStreaming);
            customerRoutes.MapPost("/consume", ConsumeMessage);
            customerRoutes.MapPost("/stop-consume", StopConsume);
        }

        private async static Task SimulateRoutePointStreaming(int simulateCount, IRoutePointProducer producer, RoutePointConsumerConfig config)
        {
            var routeId = Guid.NewGuid().ToString();
            var driverId = Guid.NewGuid().ToString();
            var latitude = new Random().Next(100000, 1000000);
            var longitude = new Random().Next(100000, 1000000);
            while (simulateCount > 0)
            {
                simulateCount--;
                latitude += new Random().Next(0, 10);
                longitude += new Random().Next(0, 10);
                var routePoint = new RoutePoint { RouteId = routeId, DriverId = driverId, Latitude = latitude, Longitude = longitude };
                var message = new Message<string, RoutePoint> { Key = routeId, Value = routePoint };
                await producer.PublishAsync(config.Topic, message);
            }
        }

        private static Task StopConsume()
        {
            _cancellationtoken.Cancel();
            return Task.CompletedTask;
        }

        private async static Task<IResult> ProduceMessage(int? schemaVersion, RoutePoint routePoint, IRoutePointProducer producer, RoutePointConsumerConfig config)
        {
            var message = new Message<string, RoutePoint> { Key = routePoint.RouteId, Value = routePoint };
            if (schemaVersion.HasValue)
            {
                if (message.Headers == null)
                {
                    message.Headers = new Headers();
                }
                message.Headers.Add("SchemaVersion", BitConverter.GetBytes(schemaVersion.Value));
            }
            var result = await producer.PublishAsync(config.Topic, message);
            return TypedResults.Ok(result);
        }
                
        private static IResult ConsumeMessage(IRoutePointConsumer consumer, ILoggerFactory loggerFactory, RoutePointConsumerConfig config)
        {
            _cancellationtoken = new CancellationTokenSource();
            consumer.Subscribe(config.Topic);
            var logger = loggerFactory.CreateLogger("ConsumeMessageLogger");
            var results = new List<Tuple<bool, ConsumeResult<string, RoutePoint>>>();
           
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
                    results.Add(new Tuple<bool, ConsumeResult<string, RoutePoint>>(processed, consumeResult));
                }
                catch (Exception ex)
                {
                    logger.LogError($"consumer.Consume failed: {ex.Message}.");
                }
            }

            consumer.Unsubscribe();

            return TypedResults.Ok(results.Select(a=>new { schemaVersion = a.Item2.GetSchemaVersion(), processed = a.Item1, routePoint = a.Item2.Message.Value }));
        }      
    }
}
