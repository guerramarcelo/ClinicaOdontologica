using Clinica.API.Inputs;
using Clinica.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Clinica.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConsultaController : Controller
    {

        private readonly ConsultaRepository _consultaRepository;

        public ConsultaController(ConsultaRepository consultaRepository)
        {
            _consultaRepository = consultaRepository;
        }

        [HttpPost()]
        public ActionResult MarcarConsulta(ConsultaInput input)
        {
            var consulta = _consultaRepository.MarcarConsulta(input);
            return Ok(consulta);
        }

        [HttpGet()]
        public ActionResult GetConsultas()
        {
            var consultas = _consultaRepository.GetConsultas();
            return Ok(consultas);
        }

        [HttpGet("{id}")]
        public ActionResult GetConsultaPorId(Guid id)
        {
            var consultas = _consultaRepository.GetConsultaPorId(id);
            return Ok(consultas);
        }

        [HttpPut("remarcar/{id}")]
        public ActionResult RemarcarConsulta(Guid id, [FromBody] ConsultaInput input)
        {
            var consulta = _consultaRepository.RemarcarConsulta(id, input.Data);
            return Ok(consulta);
        }

        [HttpPut("cancelar/{id}")]
        public ActionResult CancelarConsulta(Guid id)
        {
            var consulta = _consultaRepository.CancelarConsulta(id);
            return Ok(consulta);
        }
    }
}
