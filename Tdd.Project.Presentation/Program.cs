using Microsoft.EntityFrameworkCore;
using Tdd.Project.Application.Services;
using Tdd.Project.Domain.Interfaces.Repositories;
using Tdd.Project.Domain.Interfaces.Services;
using Tdd.Project.Domain.Interfaces.UnitOfWork;
using Tdd.Project.Infrastructure.Data;
using Tdd.Project.Infrastructure.Repositories;
using Tdd.Project.Infrastructure.UnitOfWork;
using TDD.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TarefaContext>
(
    opt => opt.UseSqlite("Data Source=../Tdd.Project.Infrastructure/SQLite/database.db")
);
builder.Services.AddScoped<ITarefaService, TarefaService>();
builder.Services.AddScoped<IBaseRepository<Tarefa>, BaseRepository<Tarefa, TarefaContext>>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

