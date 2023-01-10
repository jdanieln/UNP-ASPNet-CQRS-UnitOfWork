using Confluent.Kafka;

namespace PermissionsWebApi.Kafka
{
    public class KafkaConsumerHandler : IHostedService
    {
        private readonly string _topic;
        private readonly string _groupId;
        private readonly string _bootstrapServers;

        public KafkaConsumerHandler(string topic, string groupId, string bootstrapServers)
        {

            _topic = topic;
            _groupId = groupId;
            _bootstrapServers = bootstrapServers;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var conf = new ConsumerConfig
            {
                GroupId = _groupId,
                BootstrapServers = _bootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            using (var builder = new ConsumerBuilder<Ignore,
                string>(conf).Build())
            {
                builder.Subscribe(_topic);
                var cancelToken = new CancellationTokenSource();
                try
                {
                    while (true)
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
