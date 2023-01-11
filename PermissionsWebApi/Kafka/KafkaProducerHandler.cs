using Confluent.Kafka;
using Nest;

namespace PermissionsWebApi.Kafka
{
    public class OperationRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
    public class KafkaProducerHandler: IKafkaProducerHandler
    {
        private readonly string _Topic;
        private readonly string _GroupId;
        private readonly string _BootstrapServers;
        private readonly string _SaslUsername;
        private readonly string _SaslPassword;
        private readonly ProducerConfig _config;

        public KafkaProducerHandler(string topic, string groupId, string bootstrapServers, string saslUsername, string saslPassword)
        {
            _Topic = topic;
            _GroupId = groupId;
            _BootstrapServers = bootstrapServers;
            _SaslUsername = saslUsername;
            _SaslPassword = saslPassword;
            _config = new ProducerConfig {
                SaslUsername = _SaslUsername,
                SaslPassword = _SaslPassword,
                BootstrapServers = _BootstrapServers,
                SecurityProtocol = SecurityProtocol.SaslSsl
            };
        }

        public void WriteMessage(string message)
        {
            var uuid = Guid.NewGuid();

            using (var producer = new ProducerBuilder<Null, string>(_config).Build())
            {
                producer.Produce(_Topic, new Message<Null, string>{
                    Value = $"Id = { uuid.ToString() },Name={ message }"
                });
            }   
        }
    }
}
