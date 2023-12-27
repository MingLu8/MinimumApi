//using Confluent.Kafka;
//using System.Text.Json.Serialization;

//namespace MinimumApi.Kafka
//{

//    public class Producer : IProducer, IDisposable
//    {
//        private IProducer<long, string> _producer;
//        private PersonProducerConfig _config;
//        private byte[] _schemaVersion = BitConverter.GetBytes(1);
//        private IProducer<long, string> KafkaProducer => _producer ??= new ProducerBuilder<long, string>(_config.ProducerConfig)
//                .SetKeySerializer(Serializers.Int64)
//                .SetValueSerializer(Serializers.Utf8)
//                .SetLogHandler((_, message) =>
//                    Console.WriteLine($"Facility: {message.Facility}-{message.Level} Message: {message.Message}"))
//                .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}. Is Fatal: {e.IsFatal}"))
//                .Build();

//        public Producer(PersonProducerConfig config)
//        {
//            _config = config;
//        }

//        public Task<DeliveryResult<long, string>> PublishAsync(string topic, Message<long, string> message)
//        {
//            message.Headers.Add("SchemaVersion", _schemaVersion);
//            return KafkaProducer.ProduceAsync(topic, message);
//        }

//        public void Dispose()
//        {
//            KafkaProducer.Dispose();
//        }

//    }
//}
