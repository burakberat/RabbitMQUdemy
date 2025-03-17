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
            //channel.QueueDeclare("hello-queue", true, false, false); //publisher tarafından kuyruk oluşturulduğu için burada tekrardan oluşturmaya gerek yok.

            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume("hello-queue", false, consumer);

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
