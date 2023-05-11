using ItemService.EventProcessor;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ItemService.RabbitMqClient
{
    public class RabbitMqSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly string _nomeDaFila;
        private readonly IConnection _connection;
        private IModel _chanel;
        private IProcessaEvento _processaEvento;

        public RabbitMqSubscriber(IConfiguration configuration, IProcessaEvento processaEvento)
        {
            _configuration = configuration;

            _connection = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMqHost"],
                Port = Int32.Parse(_configuration["RabbitMqPort"])
            }.CreateConnection();

            _chanel = _connection.CreateModel();
            _chanel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _nomeDaFila = _chanel.QueueDeclare().QueueName;
            _chanel.QueueBind(queue: _nomeDaFila, exchange: "trigger", routingKey: "");
            _processaEvento = processaEvento;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumidor = new EventingBasicConsumer(_chanel);

            consumidor.Received += (ModuleHandle, ea) =>
            {
                var body = ea.Body;
                var mensagem = Encoding.UTF8.GetString(body.ToArray());
                _processaEvento.Processa(mensagem);
            };

            _chanel.BasicConsume(queue: _nomeDaFila, autoAck: true, consumer: consumidor);

            return Task.CompletedTask;
        }
    }
}
