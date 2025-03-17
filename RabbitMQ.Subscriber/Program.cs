using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQ.Subscriber
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://laaknfjq:lHyWIu6ZfyhuDjcEFI4CWZl0VjDXcrrg@seal.lmq.cloudamqp.com/laaknfjq");

            using var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var randomQueueName = channel.QueueDeclare().QueueName;

            channel.QueueDeclare(randomQueueName, true, false, false);
            channel.QueueBind(randomQueueName, "logs-fanout", "", null);

            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(randomQueueName, false, consumer);

            Console.WriteLine("Loglar Dinleniyor.");

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Thread.Sleep(1500);
                Console.WriteLine("Gelen Mesaj:" + message);

                channel.BasicAck(e.DeliveryTag, false);
            };

            Console.ReadLine();
        }
    }
}
