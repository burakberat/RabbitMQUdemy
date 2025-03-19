using RabbitMQ.Client;
using Shared;
using System.Text;
using System.Text.Json;

namespace RabbitMQ.Publisher
{
    public enum LogNames
    {
        Critical = 1,
        Error = 2,
        Warning = 3,
        Info = 4
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Ortak
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://laaknfjq:lHyWIu6ZfyhuDjcEFI4CWZl0VjDXcrrg@seal.lmq.cloudamqp.com/laaknfjq");
            using var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            #endregion


            #region Fanout Exchange
            channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout);

            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                string message = $"log {x}";
                var messageBody = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("logs-fanout", "", null, messageBody);
                Console.WriteLine($"Mesaj gönderilmiştir : {message}");
            });
            Console.ReadLine();
            #endregion

            #region Topic Exchange
            //channel.ExchangeDeclare("logs-topic", durable: true, type: ExchangeType.Topic);
            //Random rnd = new Random();
            //Enumerable.Range(1, 50).ToList().ForEach(x =>
            //{
            //    LogNames log1 = (LogNames)rnd.Next(1, 5);
            //    LogNames log2 = (LogNames)rnd.Next(1, 5);
            //    LogNames log3 = (LogNames)rnd.Next(1, 5);

            //    var routeKey = $"{log1}.{log2}.{log3}";
            //    string message = $"log-type: {log1}-{log2}-{log3}";
            //    var messageBody = Encoding.UTF8.GetBytes(message);
            //    channel.BasicPublish("logs-topic", routeKey, null, messageBody);
            //    Console.WriteLine($"Log gönderilmiştir : {message}");
            //});
            //Console.ReadLine(); 
            #endregion

            #region Direct Exchange
            //channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct);

            //Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
            //{
            //    var routeKey = $"route-{x}";
            //    var queueName = $"direct-queue-{x}";
            //    channel.QueueDeclare(queueName, true, false, false);
            //    channel.QueueBind(queueName, "logs-direct", routeKey, null);
            //});

            //Enumerable.Range(1, 50).ToList().ForEach(x =>
            //{
            //    LogNames log = (LogNames)new Random().Next(1, 5);
            //    string message = $"log-type: {log}";
            //    var messageBody = Encoding.UTF8.GetBytes(message);
            //    var routeKey = $"route-{log}";
            //    channel.BasicPublish("logs-direct", routeKey, null, messageBody);
            //    Console.WriteLine($"Log gönderilmiştir : {message}");
            //});
            //Console.ReadLine(); 
            #endregion

            #region HeaderExchange ve Text Message
            //channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);
            //Dictionary<string, object> headers = new Dictionary<string, object>();
            //headers.Add("format", "pdf");
            //headers.Add("shape2", "a4");

            //var properties = channel.CreateBasicProperties();
            //properties.Headers = headers;
            //properties.Persistent = true;

            //var product = new Product { Id = 1, Name = "Kalem", Price = 100, Stock = 10 };
            //var productJsonString = JsonSerializer.Serialize(product);

            //channel.BasicPublish("header-exchange", string.Empty, properties, Encoding.UTF8.GetBytes(productJsonString));
            //Console.WriteLine("mesaj gönderilmiştir");
            //Console.ReadLine(); 
            #endregion
        }
    }
}
