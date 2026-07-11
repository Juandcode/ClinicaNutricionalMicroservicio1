using GestionClinicaNutricional.Application.Paciente;
using GestionClinicaNutricional.Application.PlanAlimenticio;
using GestionClinicaNutricional.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionClinicaNutricionalService.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PacienteController(IMediator mediator) : ControllerBase
    {
        [HttpPost(nameof(CreatePaciente))]
        public async Task<IActionResult> CreatePaciente([FromBody] CreatePacienteCommand request)
        {
            var result = await mediator.Send(request);

            return Ok(result);
        }

        [HttpPost("{pacienteId}/" + nameof(CreateConsulta))]
        public async Task<IActionResult> CreateConsulta(
            [FromRoute] Guid pacienteId,
            [FromBody] CreateConsultaCommand request)
        {
            var requestConId = request with { PacienteId = pacienteId };
            var result = await mediator.Send(requestConId);

            return Ok(result);
        }

        [HttpPost("{planAlimenticioId}/" + nameof(CreateEvaluacion))]
        public async Task<IActionResult> CreateEvaluacion(
            [FromRoute] Guid planAlimenticioId,
            [FromBody] CreateEvaluacionCommand request)
        {
            var requestConId = request with { PlanAlimenticioId = planAlimenticioId };
            var result = await mediator.Send(requestConId);

            return Ok(result);
        }

        [HttpGet("{planAlimenticioId}/" + nameof(Evaluaciones))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Evaluacion>))]
        public async Task<IActionResult> Evaluaciones([FromRoute] Guid planAlimenticioId)
        {
            var request = new GetEvaluacionesCommand { PlanAlimenticioId = planAlimenticioId };
            var result = await mediator.Send(request);

            return Ok(result);
        }
    }
}