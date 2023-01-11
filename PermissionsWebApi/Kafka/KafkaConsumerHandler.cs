using Confluent.Kafka;

namespace PermissionsWebApi.Kafka
{
    public class KafkaConsumerHandler : IHostedService
    {
        private readonly string _Topic;
        private readonly string _GroupId;
        private readonly string _BootstrapServers;
        private readonly string _SaslUsername;
        private readonly string _SaslPassword;

        public KafkaConsumerHandler(string topic, string groupId, string bootstrapServers, string saslUsername, string saslPassword)
        {

            _Topic = topic;
            _GroupId = groupId;
            _BootstrapServers = bootstrapServers;
            _SaslUsername = saslUsername;
            _SaslPassword = saslPassword;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var conf = new ConsumerConfig
            {
                GroupId = _GroupId,
                BootstrapServers = _BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslUsername = _SaslUsername,
                SaslPassword = _SaslPassword
            };
            using (var builder = new ConsumerBuilder<Ignore,
                string>(conf).Build())
            {
                builder.Subscribe(_Topic);
                var cancelToken = new CancellationTokenSource();
                try
                {
                    while (!true)
                    {
                        var consumer = builder.Consume(cancelToken.Token);
                        Console.WriteLine($"Message: {consumer.Message.Value} received from {consumer.TopicPartitionOffset}");
                    }
                }
                catch (Exception)
                {
                    builder.Close();
                }
            }
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
