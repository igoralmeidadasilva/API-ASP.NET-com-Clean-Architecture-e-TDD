using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TDD.Domain.Entities;

namespace Tdd.Project.Infrastructure.Data;

public sealed class TarefaContext : DbContext
{
    public TarefaContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Tarefa> Tarefas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tarefa>()
            .HasKey(x => x.Id);
        base.OnModelCreating(modelBuilder);
    } 
    
}
