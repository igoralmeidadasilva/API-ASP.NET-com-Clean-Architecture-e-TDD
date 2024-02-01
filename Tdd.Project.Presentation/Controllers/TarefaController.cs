using Microsoft.AspNetCore.Mvc;
using Tdd.Project.Domain.Interfaces.Services;
using TDD.Domain.Entities;

namespace Tdd.Project.Presentation.Controllers;

[Route("/api/tarefas/v1")]
[ApiController]
public sealed class TarefaController : ControllerBase
{
    private readonly ILogger<TarefaController> _logger;
    private readonly ITarefaService _service;

    public TarefaController(ILogger<TarefaController> logger, ITarefaService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet]
    [Route(nameof(GetAllTarefas))]
    public async Task<ActionResult<IEnumerable<Tarefa>>> GetAllTarefas()
    {
        try
        {
            _logger.LogInformation("Start {name} Async.", nameof(GetAllTarefas));
            var response = await _service.ObterTodasAsTarefas();
            _logger.LogInformation("End {name} Async", nameof(GetAllTarefas));

            if(!response.Any())
            {
                _logger.LogInformation("Nenhum resultado encontrado");
                return NotFound();
            }

            _logger.LogInformation("Resultados encontrados: {response}", response);
            return Ok(response);
        }
        catch(Exception ex)
        {
            _logger.LogError("Error em {name}: {ex}", nameof(GetAllTarefas), ex);
            var InternalServerError = StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            return InternalServerError;
        }
    }

    [HttpGet]
    [Route(nameof(GetTarefaPorId))]
    public async Task<ActionResult<IEnumerable<Tarefa>>> GetTarefaPorId(int id)
    {
        try
        {
            _logger.LogInformation("Start {name} Async.", nameof(GetTarefaPorId));
            var response = await _service.ObterTarefaPorId(id);
            _logger.LogInformation("End {name} Async", nameof(GetTarefaPorId));

            if(response.Id == 0)
            {
                _logger.LogInformation("Nenhum resultado encontrado");
                return NotFound();
            }

            _logger.LogInformation("Resultados encontrados: {response}", response);
            return Ok(response);
        }
        catch(Exception ex)
        {
            _logger.LogError("Error em {name}: {ex}", nameof(GetTarefaPorId), ex);
            var InternalServerError = StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            return InternalServerError;
        }
    }

    [HttpPost]
    [Route(nameof(AdicionarTarefa))]
    public async Task<ActionResult<bool>> AdicionarTarefa([FromBody] Tarefa tarefa)
    {
        try
        {
            _logger.LogInformation("Start {name} Async.", nameof(AdicionarTarefa));
            var response = await _service.AdicionarTarefaAsync(tarefa);
            _logger.LogInformation("End {name} Async", nameof(AdicionarTarefa));

            if(!response)
            {
                _logger.LogError("Error ao adicionar tarefa.");
                return BadRequest(response);
            }

            return Created("Successfully created", response);
        }
        catch(Exception ex)
        {
            _logger.LogError("Error em {name}: {ex}", nameof(AdicionarTarefa), ex);
            var InternalServerError = StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            return InternalServerError;
        }
    }

    [HttpPut]
    [Route(nameof(AlterarTarefa))]
    public async Task<ActionResult<bool>> AlterarTarefa(Tarefa tarefa)
    {
        try
        {
            _logger.LogInformation("Star {name} Async.", nameof(AlterarTarefa));
            var response = await _service.AtualizarTarefa(tarefa);
            _logger.LogInformation("End {name} Async", nameof(AlterarTarefa));

            if(!response)
            {
                _logger.LogError("Error ao alterar a tarefa.");
                return BadRequest(response);
            }

            return Ok(response);

        }
        catch(Exception ex)
        {
            _logger.LogError("Error em {name}: {ex}", nameof(AlterarTarefa), ex);
            var InternalServerError = StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            return InternalServerError;
        }
    }

    [HttpDelete]
    [Route(nameof(RemoverTarefa))]
    public async Task<ActionResult<bool>> RemoverTarefa(int id)
    {
        try
        {
            _logger.LogInformation("Start {name} Async.", nameof(RemoverTarefa));
            var response = await _service.RemoverTarefa(id);
            _logger.LogInformation("End {name} Async.", nameof(RemoverTarefa));

            if(!response)
            {
                _logger.LogError("Erro ao remover a tarefa");
                return BadRequest(response);
            }
            return NoContent();
        }
        catch(Exception ex)
        {
            _logger.LogError("Error em {name}: {ex}", nameof(RemoverTarefa), ex);
            var InternalServerError = StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            return InternalServerError;
        }
    }
}

