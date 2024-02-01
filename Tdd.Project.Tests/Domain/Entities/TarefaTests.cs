namespace TDD.Tests.Domain.Entities;

public class TarefaTests
{
    private static Tarefa CriarTarefaPadrao()
    {
        var id = 1;
        var titulo = "título-teste";
        var descricao = "";

        return Tarefa.Create(id, titulo, descricao);
    }

    [Fact]
    public void CreatTarefa_CasoTituloEDescricaoSejamValidos_ReturnTrue()
    {
        // Arrange
        var id = 1;
        var titulo = "título-teste";
        var descricao = "descrição-teste";

        // Act
        var tarefa = Tarefa.Create(id, titulo, descricao);

        // Asserts
        Assert.Equal(id, tarefa.Id);
        Assert.Equal(titulo, tarefa.Titulo);
        Assert.Equal(descricao, tarefa.Descricao);
    }

    [Fact]
    public void UpdateDescricao_CasoNovaDescricaoSejaValida_ReturnTrue()
    {
        // Arrange
        var tarefa = CriarTarefaPadrao();
        var descricao = tarefa.Descricao;

        var novaDescricao = "nova-descrição-teste";

        // Act
        var tarefaUpdateDescricao = tarefa.UpdateDescricao(novaDescricao);

        // Asserts
        Assert.False(descricao!.Equals(tarefaUpdateDescricao.Descricao), "A descrição não deveria ser igual após a atualização.");
        Assert.Equal(novaDescricao, tarefaUpdateDescricao.Descricao);
        Assert.False(descricao.Equals(tarefa.Descricao), "A descrição não deveria ser igual após a atualização.");
        Assert.Equal(novaDescricao, tarefa.Descricao);
    }

    [Fact]
    public void UpdateTitulo_CasoNovoTituloSejaValido_ReturnTarefa()
    {
        // Arrange
        var tarefa = CriarTarefaPadrao();
        var titulo = tarefa.Titulo;

        var novoTitulo = "novo-titulo-teste";

        // Act
        var tarefaUpdateTitulo = tarefa.UpdateTitulo(novoTitulo);

        // Asserts
        Assert.False(titulo!.Equals(tarefaUpdateTitulo.Titulo), "O titulo não deveria ser igual após a atualização.");
        Assert.Equal(novoTitulo, tarefaUpdateTitulo.Titulo);
        Assert.False(titulo.Equals(tarefa.Titulo), "O titulo não deveria ser igual após a atualização.");
        Assert.Equal(novoTitulo, tarefa.Titulo);
    }
    
    [Fact]
    public void UpdateTarefa_CasoNovosTituloEDescricaoSejamValidos_ReturnTarefa()
    {
        // Arrange
        var tarefa = CriarTarefaPadrao();
        var titulo = tarefa.Titulo;
        var descricao = tarefa.Descricao;

        var novoTitulo = "novo-titulo-teste";
        var novaDescricao = "nova-descrição-teste";
        var novaTarefa = Tarefa.Create(tarefa.Id, novoTitulo, novaDescricao);

        // Act
        var tarefaUpdate = tarefa.Update(novaTarefa);

        // Asserts
        Assert.False(
            titulo!.Equals(tarefaUpdate.Titulo) &&
            descricao!.Equals(tarefaUpdate.Descricao),
            "O titulo e a Descrição não deveria ser igual após a atualização utlizando o método tarefaUpdate."
            );

        Assert.Equal(novoTitulo, tarefaUpdate.Titulo);
        Assert.Equal(novoTitulo, tarefa.Titulo);

        Assert.Equal(novaDescricao, tarefaUpdate.Descricao);
        Assert.Equal(novaDescricao, tarefa.Descricao);
    }

    [Fact]
    public void UpdateDescricao_CasoADescricaoSejaNulaOuIgualAAnterior_ThrowsArgumentException()
    {
        // Arrange
        var tarefa = CriarTarefaPadrao();
        var descricao = tarefa.Descricao;

        // Act & Asserts
        Assert.Throws<ArgumentException>(() => tarefa = tarefa.UpdateDescricao(null!));
        Assert.Throws<ArgumentException>(() => tarefa = tarefa.UpdateDescricao(""));

    }

    [Fact]
    public void UpdateTitulo_CasoOTituloSejaNulaOuIgualAoAnterior_ThrowsArgumentException()
    {
        // Arrange
        var tarefa = CriarTarefaPadrao();
        var titulo = tarefa.Titulo;

        // Act & Asserts
        Assert.Throws<ArgumentException>(() => tarefa = tarefa.UpdateTitulo(null!));
        Assert.Throws<ArgumentException>(() => tarefa = tarefa.UpdateTitulo(""));
    }

}

