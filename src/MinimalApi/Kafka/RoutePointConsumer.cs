using Confluent.Kafka;
using MinimumApi.Entities;
using MinimumApi.Kafka.Core;

namespace MinimumApi.Kafka
{
    public interface IRoutePointConsumer : IGenericConsumer<string, RoutePoint> { }
    public class RoutePointConsumer(RoutePointConsumerConfig config) : GenericConsumer<string, RoutePoint>(config), IRoutePointConsumer
    {

    }
}
