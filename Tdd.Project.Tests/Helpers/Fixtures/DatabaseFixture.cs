namespace Tdd.Project.Tests.Helpers.Fixtures;

public class DatabaseFixture
{
    private const string CONNECTION_STRING = "Data Source=D:/workspace/Estudos/dotnet/TDD/Tdd.Project.Tests/SQLite/database.test.db";
    private static object _lock = new object();

    public DatabaseFixture()
    {
        //EnsureDatabaseCreated();
        using var context = CreateContext();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        context.AddRange
        (
            Tarefa.Create(1, "teste-1", "-"),
            Tarefa.Create(2, "teste-2", "-"),
            Tarefa.Create(3, "teste-3", "-"),
            Tarefa.Create(4, "teste-4", "-"),
            Tarefa.Create(5, "teste-5", "-")
        );
       
        context.SaveChanges();
    }
    public TarefaContext CreateContext()
    {
        return new TarefaContext
        (
            new DbContextOptionsBuilder<TarefaContext>().UseSqlite(CONNECTION_STRING).Options
        );
    }

}
