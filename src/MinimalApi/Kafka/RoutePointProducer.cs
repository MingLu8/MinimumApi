using Confluent.Kafka;
using MinimumApi.Entities;
using MinimumApi.Kafka.Core;

namespace MinimumApi.Kafka
{
    public interface IRoutePointProducer : IGenericProducer<string, RoutePoint> { }
    public class RoutePointProducer(RoutePointProducerConfig config) : GenericProducer<string, RoutePoint>(config), IRoutePointProducer
    {

    }
}
