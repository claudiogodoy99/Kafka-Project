
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Produtor;

namespace Demostrativa
{
    [Route("api/[controller]")]
    public class ClienteController : Controller
    {
        private readonly ILogger<EntidadeCliente> _logger;

        public ClienteController(ILogger<EntidadeCliente> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]EntidadeCliente entidade)
        {
            try
            {
                Logar(entidade);

                Produtor<EntidadeCliente> produtor = new Produtor<EntidadeCliente>(_logger);
                var taskPorduto = produtor.Produzir(entidade);
              

                await taskPorduto;

                return Created("api/cliente",entidade.cpf) ;
            }
            catch 
            {
                return Problem();

            }
        }

        public async void Logar(EntidadeCliente cliente) 
        {
            _logger.LogInformation($"criar salvar cliente: {JsonConvert.SerializeObject(cliente)}");
        }
    }
}
