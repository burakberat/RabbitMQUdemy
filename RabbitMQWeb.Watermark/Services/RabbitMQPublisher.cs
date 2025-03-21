using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace RabbitMQWeb.Watermark.Services
{
    public class RabbitMQPublisher
    {
        private readonly RabbitMQClientService _rabbitMQClientService;

        public RabbitMQPublisher(RabbitMQClientService rabbitMQClientService)
        {
            _rabbitMQClientService = rabbitMQClientService;
        }

        public void Publish(ProductImageCreatedEvent productImageCreatedEvent)
        {
            var channel = _rabbitMQClientService.Connect();
            var body = JsonSerializer.Serialize(productImageCreatedEvent);
            var bodyByte = Encoding.UTF8.GetBytes(body);
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            channel.BasicPublish(RabbitMQClientService.ExchangeName, RabbitMQClientService.RoutingWatermark, properties, bodyByte);
        }
    }
}
