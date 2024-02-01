using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tdd.Project.Domain.Interfaces.Services;
using Tdd.Project.Presentation.Controllers;

namespace Tdd.Project.Tests.Presentation.Controllers;

public sealed class TarefaControllerTest
{
    private readonly ILogger<TarefaController> _loggerController;
    private readonly ILogger<TarefaService> _loggerService;
    private readonly DatabaseFixture _context;
    private readonly Mock<ITarefaService> _tarefaServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IBaseRepository<Tarefa>> _repoMock;

    public TarefaControllerTest()
    {
        _loggerController = new LoggerFactory().CreateLogger<TarefaController>();
        _tarefaServiceMock = new Mock<ITarefaService>();
        _context = new DatabaseFixture();
        _loggerService = new LoggerFactory().CreateLogger<TarefaService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _repoMock = new Mock<IBaseRepository<Tarefa>>();
    }

    [Fact]
    public async Task GetTodasAsTarefas_QuandoOBancoTemRegistros_ReturnOk()
    {
        // Arrange
        var context = _context.CreateContext();
        var loggerRepo = new LoggerFactory().CreateLogger<BaseRepository<Tarefa, DbContext>>();
        var repo = new BaseRepository<Tarefa, DbContext>(context, loggerRepo);
        var service = new TarefaService(repo, _loggerService, _unitOfWorkMock.Object);
        var controller = new TarefaController(_loggerController, service);

        // Act
        var actionResult = await controller.GetAllTarefas();
        var okResult = actionResult.Result as OkObjectResult;
        var tarefas = okResult!.Value as IEnumerable<Tarefa>;

        // Assert
        Assert.NotNull(okResult);
        Assert.NotNull(tarefas);

        var tarefasNoBanco = await context.Tarefas.AsNoTracking().ToListAsync();
        Assert.Equal(tarefasNoBanco.Count, tarefas.Count());
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }

    [Fact]
    public async Task GetTodasAsTarefas_QuandoOBancoEstaVazio_ReturnNotFound()
    {
        // Arrange
        var context = _context.CreateContext();
        
        _tarefaServiceMock.Setup(x => x.ObterTodasAsTarefas()).ReturnsAsync(await Task.FromResult(Enumerable.Empty<Tarefa>()));
        var controller = new TarefaController(_loggerController, _tarefaServiceMock.Object);

        // Act
        var actionResult = await controller.GetAllTarefas();

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(actionResult.Result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task GetTodasAsTarefas_QuandoCapturamosAlgumaExcessao_ReturnInternalServerError()
    {
        // Arrange
        _tarefaServiceMock.Setup(x => x.ObterTodasAsTarefas()).Throws<Exception>();
        var controller = new TarefaController(_loggerController, _tarefaServiceMock.Object);

        // Act
        var actionResult = await controller.GetAllTarefas();

        // Assert
        var internalServerErrorRequestResult = actionResult.Result as ObjectResult;
        Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorRequestResult!.StatusCode);
    }

    [Fact]
    public async Task GetTodasAsTarefas_QuandoCapturamosAlgumaExcessaoEmNiveisMaisBaixo_ReturnInternalServerError()
    {
        // Arrange
        _repoMock.Setup(x => x.GetAllAsync()).Throws<Exception>();
        var service = new TarefaService(_repoMock.Object, _loggerService, _unitOfWorkMock.Object);
        var controller = new TarefaController(_loggerController, service);

        // Act
        var actionResult = await controller.GetAllTarefas();

        // Assert
        var internalServerErrorRequestResult = actionResult.Result as ObjectResult;
        Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorRequestResult!.StatusCode);
    }

    [Theory]
    [InlineData(1)]
    public async Task GetTarefaPorId_QuandoOBancoTemRegistro_ReturnOk(int id)
    {
        // Arrange
        var context = _context.CreateContext();
        var loggerRepo = new LoggerFactory().CreateLogger<BaseRepository<Tarefa, DbContext>>();
        var repo = new BaseRepository<Tarefa, DbContext>(context, loggerRepo);
        var service = new TarefaService(repo, _loggerService, _unitOfWorkMock.Object);
        var controller = new TarefaController(_loggerController, service);

        // Act
        var actionResult = await controller.GetTarefaPorId(id);
        var okResult = actionResult.Result as OkObjectResult;
        var tarefa = (Tarefa) okResult!.Value!;

        // Assert
        Assert.NotNull(okResult);
        Assert.NotNull(tarefa);

        var tarefasNoBanco = await context.Tarefas.AsNoTracking().FirstOrDefaultAsync(x => x.Id!.Equals(id));
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.Equal(tarefa.Id, tarefasNoBanco!.Id);
    }

    [Theory]
    [InlineData(99)]
    public async Task GetTarefaPorId_QuandoOBancoNaoTemORegistro_ReturnNotFound(int id)
    {
        // Arrange
        var context = _context.CreateContext();
        var loggerRepo = new LoggerFactory().CreateLogger<BaseRepository<Tarefa, DbContext>>();
        var repo = new BaseRepository<Tarefa, DbContext>(context, loggerRepo);
        var service = new TarefaService(repo, _loggerService, _unitOfWorkMock.Object);
        var controller = new TarefaController(_loggerController, service);

        // Act
        var actionResult = await controller.GetTarefaPorId(id);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(actionResult.Result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }

    [Theory]
    [InlineData(1)]
    public async Task GetTarefaPorId_QuandoCapturamosAlgumaExcessao_ReturnInternalServerError(int id)
    {
        // Arrange
        _tarefaServiceMock.Setup(x => x.ObterTarefaPorId(id)).Throws<Exception>();
        var controller = new TarefaController(_loggerController, _tarefaServiceMock.Object);

        // Act
        var actionResult = await controller.GetTarefaPorId(id);

        // Assert
        var internalServerErrorRequestResult = actionResult.Result as ObjectResult;
        Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorRequestResult!.StatusCode);
    }

    [Theory]
    [InlineData(1)]
    public async Task GetTarefaPorId_QuandoCapturamosAlgumaExcessaoEmNiveisMaisBaixo_ReturnInternalServerError(int id)
    {
        // Arrange
        _repoMock.Setup(x => x.GetByIdAsync(id)).Throws<Exception>();
        var service = new TarefaService(_repoMock.Object, _loggerService, _unitOfWorkMock.Object);
        var controller = new TarefaController(_loggerController, service);

        // Act
        var actionResult = await controller.GetTarefaPorId(id);

        // Assert
        var internalServerErrorRequestResult = actionResult.Result as ObjectResult;
        Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorRequestResult!.StatusCode);
    }
    
    public static IEnumerable<object[]> TarefaComoParametroParaInserir()
    {
        yield return new object[]
        {
            Tarefa.Create(6, "teste-6", "")
        };        
    }

    [Theory]
    [MemberData(nameof(TarefaComoParametroParaInserir))]
    public async Task AddTarefa_QuandoATarefaEAdicionadaComSucesso_ReturnCreated(Tarefa tarefa)
    {
        // Arrange
        var context = _context.CreateContext();
        var loggerRepo = new LoggerFactory().CreateLogger<BaseRepository<Tarefa, DbContext>>();
        var repo = new BaseRepository<Tarefa, DbContext>(context, loggerRepo);
        _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
        var service = new TarefaService(repo, _loggerService, _unitOfWorkMock.Object);
        var controller = new TarefaController(_loggerController, service);

        // Act
        var actionResult = await controller.AdicionarTarefa(tarefa);
        var createdResult = actionResult.Result as CreatedResult;
        var result = (bool) createdResult!.Value!;

        // Assert
        Assert.Equal(StatusCodes.Status201Created, createdResult!.StatusCode);
        Assert.True(result);

    } 

    [Theory]
    [MemberData(nameof(TarefaComoParametroParaInserir))]
    public async Task AddTarefa_QuandoATarefaEAdicionadaSemSucesso_ReturnBadRequest(Tarefa tarefa)
    {
        // Arrange
        var context = _context.CreateContext();
        var loggerRepo = new LoggerFactory().CreateLogger<BaseRepository<Tarefa, DbContext>>();
        var repo = new BaseRepository<Tarefa, DbContext>(context, loggerRepo);
        _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.FromResult(0));
        var service = new TarefaService(repo, _loggerService, _unitOfWorkMock.Object);
        var controller = new TarefaController(_loggerController, service);

        // Act
        var actionResult = await controller.AdicionarTarefa(tarefa);
        var badRequestResult = actionResult.Result as BadRequestObjectResult;
        
        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult!.StatusCode);
    } 

    [Theory]
    [MemberData(nameof(TarefaComoParametroParaInserir))]
    public async Task AddTarefa_QuandoCapturamosAlgumaExcessao_ReturnInternalServerError(Tarefa tarefa)
    {
        // Arrange
        _tarefaServiceMock.Setup(x => x.AdicionarTarefaAsync(tarefa)).Throws<Exception>();
        var controller = new TarefaController(_loggerController, _tarefaServiceMock.Object);

        // Act
        var actionResult = await controller.AdicionarTarefa(tarefa);

        // Assert
        var internalServerErrorRequestResult = actionResult.Result as ObjectResult;
        Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorRequestResult!.StatusCode);
    } 

    public static IEnumerable<object[]> TarefaComoParametroParaAlterarComSucesso()
    {
        yield return new object[]
        {
            Tarefa.Create(1, "novo-teste", "nova-descricao")
        };        
    }

    [Theory]
    [MemberData(nameof(TarefaComoParametroParaAlterarComSucesso))]
    public async Task AlterarTarefa_QuandoATarefaEAlteradaComSucesso_ReturnOk(Tarefa tarefa)
    {
        // Arrange
        var context = _context.CreateContext();
        var loggerRepo = new LoggerFactory().CreateLogger<BaseRepository<Tarefa, DbContext>>();
        var repo = new BaseRepository<Tarefa, DbContext>(context, loggerRepo);
        _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
        var service = new TarefaService(repo, _loggerService, _unitOfWorkMock.Object);
        var controller = new TarefaController(_loggerController, service);

        // Act
        var actionResult = await controller.AlterarTarefa(tarefa);
        var createdResult = actionResult.Result as OkObjectResult;
        var result = (bool) createdResult!.Value!;

        // Assert
        Assert.Equal(StatusCodes.Status200OK, createdResult!.StatusCode);
        Assert.True(result);
    } 

    public static IEnumerable<object[]> TarefaComoParametroParaAlterarSemSucesso()
    {
        yield return new object[]
        {
            Tarefa.Create(6, "novo-teste", "nova-descricao")
        };        
    }

    [Theory]
    [MemberData(nameof(TarefaComoParametroParaAlterarSemSucesso))]
    public async Task AlterarTarefa_QuandoATarefaEAlteradaSemSucesso_ReturnBadRequest(Tarefa tarefa)
    {
        // Arrange
        var context = _context.CreateContext();
        var loggerRepo = new LoggerFactory().CreateLogger<BaseRepository<Tarefa, DbContext>>();
        var repo = new BaseRepository<Tarefa, DbContext>(context, loggerRepo);
        _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.FromResult(0));
        var service = new TarefaService(repo, _loggerService, _unitOfWorkMock.Object);
        var controller = new TarefaController(_loggerController, service);

        // Act
        var actionResult = await controller.AlterarTarefa(tarefa);
        var badRequestResult = actionResult.Result as BadRequestObjectResult;
        var result = (bool) badRequestResult!.Value!;

        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult!.StatusCode);
    } 

    [Theory]
    [MemberData(nameof(TarefaComoParametroParaAlterarSemSucesso))]
    public async Task AlterarTarefa_QuandoATarefaNaoExisteNoBanco_ReturnBadRequest(Tarefa tarefa)
    {
        // Arrange
        var context = _context.CreateContext();
        var loggerRepo = new LoggerFactory().CreateLogger<BaseRepository<Tarefa, DbContext>>();
        var repo = new BaseRepository<Tarefa, DbContext>(context, loggerRepo);
        _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
        var service = new TarefaService(repo, _loggerService, _unitOfWorkMock.Object);
        var controller = new TarefaController(_loggerController, service);

        // Act
        var actionResult = await controller.AlterarTarefa(tarefa);
        var badRequestResult = actionResult.Result as BadRequestObjectResult;
        var result = (bool) badRequestResult!.Value!;

        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult!.StatusCode);
        Assert.False(result);
    } 

    [Theory]
    [MemberData(nameof(TarefaComoParametroParaAlterarSemSucesso))]
    public async Task AlterarTarefa_QuandoCapturamosAlgumaExcessao_ReturnInternalServerError(Tarefa tarefa)
    {
        // Arrange
        _tarefaServiceMock.Setup(x => x.AtualizarTarefa(tarefa)).Throws<Exception>();
        var controller = new TarefaController(_loggerController, _tarefaServiceMock.Object);

        // Act
        var actionResult = await controller.AlterarTarefa(tarefa);

        var internalServerErrorRequestResult = actionResult.Result as ObjectResult;
        Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorRequestResult!.StatusCode);
    } 

    [Theory]
    [InlineData(1)]
    public async Task RemoverTarefa_QuandoARemocaoEBemSucedida_ReturnNoContent(int id)
    {
        // Arrange
        var context = _context.CreateContext();
        var loggerRepo = new LoggerFactory().CreateLogger<BaseRepository<Tarefa, DbContext>>();
        var repo = new BaseRepository<Tarefa, DbContext>(context, loggerRepo);
        _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
        var service = new TarefaService(repo, _loggerService, _unitOfWorkMock.Object);
        var controller = new TarefaController(_loggerController, service);

        // Act
        var actionResult = await controller.RemoverTarefa(id);
        var createdResult = actionResult.Result as NoContentResult;

        // Assert
        Assert.Equal(StatusCodes.Status204NoContent, createdResult!.StatusCode);
    }

    [Theory]
    [InlineData(99)]
    public async Task RemoverTarefa_QuandoARemocaoESemSucesso_ReturnBadRequest(int id)
    {
        // Arrange
        var context = _context.CreateContext();
        var loggerRepo = new LoggerFactory().CreateLogger<BaseRepository<Tarefa, DbContext>>();
        var repo = new BaseRepository<Tarefa, DbContext>(context, loggerRepo);
        _unitOfWorkMock.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
        var service = new TarefaService(repo, _loggerService, _unitOfWorkMock.Object);
        var controller = new TarefaController(_loggerController, service);

        // Act
        var actionResult = await controller.RemoverTarefa(id);
        var badRequestResult = actionResult.Result as BadRequestObjectResult;

        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult!.StatusCode);
    }

    [Theory]
    [InlineData(99)]
    public async Task RemoverTarefa_QuandoCapturamosAlgumaExcessao_ReturnInternalServerError(int id)
    {
        // Arrangr
        _tarefaServiceMock.Setup(x => x.RemoverTarefa(id)).Throws<Exception>();
        var controller = new TarefaController(_loggerController, _tarefaServiceMock.Object);

        // Act
        var actionResult = await controller.RemoverTarefa(id);

        // Asserts
        var internalServerErrorRequestResult = actionResult.Result as ObjectResult;
        Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorRequestResult!.StatusCode);
    }

    [Theory]
    [InlineData(1)]
    public async Task RemoverTarefa_QuandoCapturamosAlgumaExcessaoEmNiveisMaisBaixos_ReturnInternalServerError(int id)
    {
        // Arrange
        _repoMock.Setup(x => x.Delete(id)).Throws<Exception>();
        var service = new TarefaService(_repoMock.Object, _loggerService, _unitOfWorkMock.Object);
        var controller = new TarefaController(_loggerController, service);

        // Act
        var actionResult = await controller.RemoverTarefa(id);

        // Assert
        var internalServerErrorRequestResult = actionResult.Result as ObjectResult;
        Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorRequestResult!.StatusCode);
    }
}
