using Confluent.Kafka;

namespace PermissionsWebApi.Kafka
{
    public class KafkaConsumerHandler : IHostedService
    {
        private readonly string _topic;
        private readonly string _groupId;
        private readonly string _bootstrapServers;
        private readonly string _SaslUsername;
        private readonly string _SaslPassword;

        public KafkaConsumerHandler(string topic, string groupId, string bootstrapServers, string saslUsername, string saslPassword)
        {

            _topic = topic;
            _groupId = groupId;
            _bootstrapServers = bootstrapServers;
            _SaslUsername = saslUsername;
            _SaslPassword = saslPassword;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var conf = new ConsumerConfig
            {
                GroupId = _groupId,
                BootstrapServers = _bootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslUsername = _SaslUsername,
                SaslPassword = _SaslPassword
            };
            using (var builder = new ConsumerBuilder<Ignore,
                string>(conf).Build())
            {
                builder.Subscribe(_topic);
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
