namespace PermissionsWebApi.Kafka
{
    public interface IKafkaProducerHandler
    {
        void WriteMessage(string message);
    }
}
