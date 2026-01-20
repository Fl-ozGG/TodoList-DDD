using Microsoft.EntityFrameworkCore;
using TestDDD.Domain.Repositories;
using TestDDD.Domain.Services;
using TestDDD.Infrastructure.Persistance;
using TestDDD.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configurar DbContext
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlite("Data Source=todo.db"));

// Registrar Repositorios
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Registrar Servicios
builder.Services.AddScoped<TodoServices>();
builder.Services.AddScoped<UserServices>();

// Configuración MVC y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Migraciones automáticas al iniciar
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    if (!app.Environment.IsEnvironment("Testing"))
    {
        //db.Database.Migrate();
    }
}

// Configuración del pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }