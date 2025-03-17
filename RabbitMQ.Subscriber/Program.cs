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

            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);
            var queueName = "direct-queue-Critical";
            channel.BasicConsume(queueName, false, consumer);

            Console.WriteLine("Loglar Dinleniyor.");

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Thread.Sleep(1500);
                Console.WriteLine("Gelen Mesaj:" + message);
                //File.AppendAllText("logs-critical.txt", message + "\n");

                channel.BasicAck(e.DeliveryTag, false);
            };

            Console.ReadLine();
        }
    }
}
