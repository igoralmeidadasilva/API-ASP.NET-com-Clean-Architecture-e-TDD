using Microsoft.Extensions.Logging;
using Tdd.Project.Domain.Interfaces.Repositories;
using Tdd.Project.Domain.Interfaces.Services;
using Tdd.Project.Domain.Interfaces.UnitOfWork;
using TDD.Domain.Entities;

namespace Tdd.Project.Application.Services;

public sealed class TarefaService : ITarefaService
{
    private readonly IBaseRepository<Tarefa> _repository;
    private readonly ILogger<TarefaService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public TarefaService(IBaseRepository<Tarefa> repository, ILogger<TarefaService> logger, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> AdicionarTarefaAsync(Tarefa tarefa)
    {
        try
        {
            _logger.LogInformation("Start AdicionarTarefaAsync.");

            await _repository.InsertAsync(tarefa);
            var commit = await _unitOfWork.CommitAsync();

            _logger.LogInformation("End AdicionarTarefaAsync.");

            if(commit == 0)
            {
                _logger.LogInformation("Erro ao alterar o banco.");
                return false;
            }

            return true;
        }
        catch(Exception ex)
        {
            _logger.LogError("Error in AdicionarTarefaAsync. {ex}", ex);
            throw;
        }
    }

    public async Task<Tarefa> ObterTarefaPorId(int id)
    {
        try
        {
            _logger.LogInformation("Start ObterTarefaPorId.");

            var response = await _repository.GetByIdAsync(id);

            _logger.LogInformation("End ObterTarefaPorId.");
            
            return response ?? Tarefa.Create(0, "", "");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in ObterTarefaPorId: {ex}", ex);
            throw;
        }

    }

    public async Task<bool> AtualizarTarefa(Tarefa tarefa)
    {
        try
        {
            _logger.LogInformation("Start AtualizarTarefa.");

            await _repository.Update(tarefa);
            var commit = await _unitOfWork.CommitAsync();

            _logger.LogInformation("End AtualizarTarefa.");

            if(commit == 0)
            {
                _logger.LogInformation("Erro ao alterar tarefa.");
                return false;
            }

            return true;
        }
        catch(ArgumentException aex)
        {
            _logger.LogError("Error in AtualizarTarefa: {ex}", aex);
            return false;;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in AtualizarTarefa: {ex}", ex);
            throw;
        }

    }

    public async Task<IEnumerable<Tarefa>> ObterTodasAsTarefas()
    {
        try
        {
            _logger.LogInformation("Start ObterTodasAsTarefas.");
            var response = await _repository.GetAllAsync();
            _logger.LogInformation("End ObterTodasAsTarefas.");

            return response ?? Enumerable.Empty<Tarefa>();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in ObterTodasAsTarefas: {ex}", ex);
            throw;
        }
    }


    public async Task<bool> RemoverTarefa(int id)
    {
        try
        {
            _logger.LogInformation("Start RemoverTarefa.");

            await _repository.Delete(id);
            var commit = await _unitOfWork.CommitAsync();

            _logger.LogInformation("End RemoverTarefa.");

            if(commit == 0)
            {
                _logger.LogInformation("Erro ao Remover do banco.");
                return false;
            }

            return true;
        }
        catch(Exception ex)
        {
            _logger.LogError("Error in RemoverTarefa: {ex}", ex);
            throw;
        }
    }

}
