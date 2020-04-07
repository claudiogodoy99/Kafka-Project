
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Produtor
{
    public class Produtor<Entidade> where Entidade : EntidadeCliente
    {

        private readonly ILogger<EntidadeCliente> _logger;

        public Produtor(ILogger<EntidadeCliente>  logger)
        {
            _logger = logger;
        }

        public async Task Produzir(Entidade entidade) 
        {
            var cofiguracao = new ProducerConfig()
            {
                BootstrapServers = "127.0.0.1:9092",
                Acks = Acks.All
            };

            var produdor = new ProducerBuilder<string, string>(cofiguracao).Build();

            produdor.Produce("cliente", new Message<string, string>
            {
                Key = entidade.cpf,
                Value = JsonConvert.SerializeObject(entidade)
            },r => TratarCallBack(r));

            produdor.Flush();
        }

        public async void TratarCallBack(DeliveryReport<string, string> report) 
        {
            if (report.Error.Code != ErrorCode.NoError)
                _logger.LogInformation("deu bom");
            else
                _logger.LogInformation("deu ruim"); ;
        }
    }
}
