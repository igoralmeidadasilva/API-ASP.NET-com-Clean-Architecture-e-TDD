namespace TDD.Tests.Domain.Services;

public sealed class TarefaServicesTest
{
    private readonly Mock<IBaseRepository<Tarefa>> _repoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DatabaseFixture _context;
    private readonly ILogger<TarefaService> _loggerService;
    private readonly ILogger<BaseRepository<Tarefa, DbContext>> _loggerRepo;

    public TarefaServicesTest()
    {
        _repoMock = new Mock<IBaseRepository<Tarefa>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _context = new DatabaseFixture();
        _loggerService = new LoggerFactory().CreateLogger<TarefaService>();
        _loggerRepo = new LoggerFactory().CreateLogger<BaseRepository<Tarefa, DbContext>>();
    }

    
    [Fact]
    public async Task AddTarefa_CasoTarefaSejaAdicionadaComSucesso_ReturnTrue()
    {
        // Arrange
        var tarefa = Tarefa.Create(1, "título-teste", "descrição-teste");
        _repoMock.Setup(x => x.InsertAsync(tarefa)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(x => x.CommitAsync()).ReturnsAsync(1);

        var service = new TarefaService(_repoMock.Object, _loggerService, _unitOfWorkMock.Object);

        // Act
        var result = await service.AdicionarTarefaAsync(tarefa);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task AddTarefa_CasoTarefaSejaAdicionadaSemSucesso_ReturnFalse()
    {
        // Arrange
        var tarefa = Tarefa.Create(1, "título-teste", "descrição-teste");
        _repoMock.Setup(x => x.InsertAsync(tarefa)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(x => x.CommitAsync()).ReturnsAsync(0);

        var service = new TarefaService(_repoMock.Object, _loggerService, _unitOfWorkMock.Object);

        // Act
        var result = await service.AdicionarTarefaAsync(tarefa);

        // Assert
        Assert.False(result);
    }


    [Theory]
    [InlineData(1)]
    public async Task ObterTarefaPorId_CasoATarefaExista_ReturnId_DaTarefa(int id)
    {
        // Arrange
        var loggerRepo = new LoggerFactory().CreateLogger<BaseRepository<Tarefa, DbContext>>();
        var repo = new BaseRepository<Tarefa, DbContext>(_context.CreateContext(), loggerRepo);
        var service = new TarefaService(repo, _loggerService, _unitOfWorkMock.Object);

        // Act
        var result = await service.ObterTarefaPorId(id);

        // Assert
        Assert.True(result.Id == id);
    }
    
    [Theory]
    [InlineData(99)]
    public async Task ObterTarefaPorId_CasoATarefaNaoSejaEncontrada_ReturnId_0(int id)
    {
        // Arrange
        var repo = new BaseRepository<Tarefa, DbContext>(_context.CreateContext(), _loggerRepo);
        var service = new TarefaService(repo, _loggerService, _unitOfWorkMock.Object);

        // Act
        var result = await service.ObterTarefaPorId(id);

        // Assert
        Assert.False(result.Id == id);
        Assert.Equal(0, result.Id);
    }

    [Theory]
    [InlineData("novo-titulo", "nova-descricao")]
    public async Task AtualizarTarefa_CasoTituloEDescricaoAlteradosComSucesso_ReturnTrue(string titulo, string descricao)
    {
        // Arrange
        var repo = new BaseRepository<Tarefa, DbContext>(_context.CreateContext(), _loggerRepo);
        _unitOfWorkMock.Setup(x => x.CommitAsync()).ReturnsAsync(1);
        var service = new TarefaService(repo, _loggerService, _unitOfWorkMock.Object);

        // Act
        var tarefa = await repo.GetByIdAsync(1);
        tarefa = tarefa.Update(Tarefa.Create(titulo, descricao));
        var result = await service.AtualizarTarefa(tarefa);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("novo-titulo", "nova-descricao")]
    public async Task AtualizarTarefa_CasoTituloEDescricaoAlteradosSemSucesso_ReturnFalse(string titulo, string descricao)
    {
        // Arrange
        var repo = new BaseRepository<Tarefa, DbContext>(_context.CreateContext(), _loggerRepo);
        _unitOfWorkMock.Setup(x => x.CommitAsync()).ReturnsAsync(0);
        var service = new TarefaService(repo, _loggerService, _unitOfWorkMock.Object);

        // Act
        var tarefa = await repo.GetByIdAsync(1);
        tarefa = tarefa.Update(Tarefa.Create(titulo, descricao));
        var result = await service.AtualizarTarefa(tarefa);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(1)]
    public async Task RemoverTarefa_CasoTarefaRemovidaComSucesso_ReturnTrue(int id)
    {
        // Arrange
        var context = _context.CreateContext();
        var repo = new BaseRepository<Tarefa, DbContext>(context, _loggerRepo);
        _unitOfWorkMock.Setup(x => x.CommitAsync()).ReturnsAsync(1);
        var service = new TarefaService(repo, _loggerService, _unitOfWorkMock.Object);

        // Act
        var result = await service.RemoverTarefa(id);
        await context.SaveChangesAsync();
        var tarefa = await service.ObterTarefaPorId(id);

        // Assert
        Assert.True(result);
        Assert.True(tarefa.Id == 0);
    }

    [Theory]
    [InlineData(1)]
    public async Task RemoverTarefa_CasoTarefaRemovidaSemSucesso_ReturnFalse(int id)
    {
        // Arrange
        var repo = new BaseRepository<Tarefa, DbContext>(_context.CreateContext(), _loggerRepo);
        _unitOfWorkMock.Setup(x => x.CommitAsync()).ReturnsAsync(0);
        var service = new TarefaService(repo, _loggerService, _unitOfWorkMock.Object);

        // Act
        var result = await service.RemoverTarefa(id);
        var tarefa = await service.ObterTarefaPorId(id);

        // Assert
        Assert.False(result);
        Assert.True(tarefa.Id != 0);
    }


    [Fact]
    public async Task ObterTodasAsTarefas_CasoExistaTarefasNoBancoParaSeremRecuperadas_ReturnEnumerableDeTarefas()
    {
        var repo = new BaseRepository<Tarefa, DbContext>(_context.CreateContext(), _loggerRepo);
        _unitOfWorkMock.Setup(x => x.CommitAsync()).ReturnsAsync(1);
        var service = new TarefaService(repo, _loggerService, _unitOfWorkMock.Object);

        // Act
        var result = await service.ObterTodasAsTarefas();

        // Assert
        Assert.True(result.Any());
    }


    [Fact]
    public async Task ObterTodasAsTarefas_CasoNaoExistamTarefasNoBancoParaSeremRecuperadas_ReturnEnumerableVazio()
    {
        // Arrange
        _repoMock.Setup(x => x.GetAllAsync()).ReturnsAsync(await Task.FromResult(Enumerable.Empty<Tarefa>()));
        var service = new TarefaService(_repoMock.Object, _loggerService, _unitOfWorkMock.Object);

        // Act
        var result = await service.ObterTodasAsTarefas();

        // Assert
        Assert.False(result.Any());
    }

}

