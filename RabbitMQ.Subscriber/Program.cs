using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;
using System.Text;
using System.Text.Json;

namespace RabbitMQ.Subscriber
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region ortak
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://laaknfjq:lHyWIu6ZfyhuDjcEFI4CWZl0VjDXcrrg@seal.lmq.cloudamqp.com/laaknfjq");
            using var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            #endregion

            #region Fanout Exchange
            var randomQueueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(randomQueueName, "logs-fanout", "", null);
            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(randomQueueName, false, consumer);
            Console.WriteLine("Logları dinleniyor...");

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Thread.Sleep(1500);
                Console.WriteLine("Gelen Mesaj:" + message);
                channel.BasicAck(e.DeliveryTag, false);
            };
            Console.ReadLine(); 
            #endregion

            #region Topic Exchange
            //channel.BasicQos(0, 1, false);
            //var consumer = new EventingBasicConsumer(channel);
            //var queueName = channel.QueueDeclare().QueueName;
            //var routekey = "Info.#";
            //channel.QueueBind(queueName, "logs-topic", routekey);
            //channel.BasicConsume(queueName, false, consumer);

            //Console.WriteLine("Logları dinleniyor...");

            //consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            //{
            //    var message = Encoding.UTF8.GetString(e.Body.ToArray());
            //    Thread.Sleep(1500);
            //    Console.WriteLine("Gelen Mesaj:" + message);
            //    // File.AppendAllText("log-critical.txt", message+ "\n");
            //    channel.BasicAck(e.DeliveryTag, false);
            //};
            //Console.ReadLine(); 
            #endregion

            #region Direct Exchange
            //channel.BasicQos(0, 1, false);
            //var consumer = new EventingBasicConsumer(channel);
            //var queueName = "direct-queue-Critical";
            //channel.BasicConsume(queueName, false, consumer);
            //Console.WriteLine("Logları dinleniyor...");

            //consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            //{
            //    var message = Encoding.UTF8.GetString(e.Body.ToArray());
            //    Thread.Sleep(1500);
            //    Console.WriteLine("Gelen Mesaj:" + message);
            //    // File.AppendAllText("log-critical.txt", message+ "\n");
            //    channel.BasicAck(e.DeliveryTag, false);
            //};
            //Console.ReadLine(); 
            #endregion

            #region HeaderExchange ve Text Message
            //channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);
            //channel.BasicQos(0, 1, false);
            //var consumer = new EventingBasicConsumer(channel);
            //var queueName = channel.QueueDeclare().QueueName;

            //Dictionary<string, object> headers = new Dictionary<string, object>();

            //headers.Add("format", "pdf");
            //headers.Add("shape", "a4");
            //headers.Add("x-match", "any");
            //channel.QueueBind(queueName, "header-exchange", String.Empty, headers);
            //channel.BasicConsume(queueName, false, consumer);
            //Console.WriteLine("Logları dinleniyor...");

            //consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            //{
            //    var message = Encoding.UTF8.GetString(e.Body.ToArray());
            //    Product product = JsonSerializer.Deserialize<Product>(message);
            //    Thread.Sleep(1500);
            //    Console.WriteLine($"Gelen Mesaj: {product.Id}-{product.Name}-{product.Price}-{product.Stock}");
            //    channel.BasicAck(e.DeliveryTag, false);
            //};
            //Console.ReadLine();
            #endregion
        }
    }
}
