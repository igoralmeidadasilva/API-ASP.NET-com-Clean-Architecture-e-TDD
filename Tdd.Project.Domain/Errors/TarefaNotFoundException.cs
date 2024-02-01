using System;

namespace Tdd.Project.Domain.Errors;

public class TarefaNotFoundException : Exception
{
    public TarefaNotFoundException(string? message = "Tarefa não encontrada") : base(message)
    { }
}
