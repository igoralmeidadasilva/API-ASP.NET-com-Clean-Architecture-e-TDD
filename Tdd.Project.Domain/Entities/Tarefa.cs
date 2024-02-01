using Tdd.Project.Domain.Entities.Base;

namespace TDD.Domain.Entities;

public sealed class Tarefa : EntityBase<Tarefa>
{
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;

    public Tarefa() : base()
    { }

    public static Tarefa Create(int id, string titulo, string descricao)
    {
        return new Tarefa
        {
            Id = id,
            Titulo = titulo,
            Descricao = descricao,
        };
    }

    public static Tarefa Create(string titulo, string descricao)
    {
        if (string.IsNullOrWhiteSpace(titulo))
        {
            throw new ArgumentException("O título não pode ser nulo ou vazio.", nameof(titulo));
        }

        if (string.IsNullOrWhiteSpace(descricao))
        {
            throw new ArgumentException("A descrição não pode ser nula ou vazia.", nameof(descricao));
        }
        
        return new Tarefa
        {
            Titulo = titulo,
            Descricao = descricao,
        };
    }

    public Tarefa UpdateTitulo(string? titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
        {
            throw new ArgumentException("O título não pode ser nulo ou vazio.", nameof(titulo));
        }

        if (titulo.Equals(Titulo))
        {
            throw new ArgumentException("O novo título é igual ao título atual.");
        }

        Titulo = titulo;
        return this;
    }

    public Tarefa UpdateDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            {
                throw new ArgumentException("A descrição não pode ser nula ou vazia.", nameof(descricao));
            }

        if (descricao.Equals(Descricao))
        {
            throw new ArgumentException("A nova descrição é igual à descrição atual.");
        }

        Descricao = descricao;
        return this;
    }

    public override Tarefa Update(Tarefa tarefa)
    {
        UpdateTitulo(tarefa.Titulo);
        UpdateDescricao(tarefa.Descricao);
        
        return this;
    }
    
}


