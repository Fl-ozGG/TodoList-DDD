using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TestDDD.Domain.Entities;
using TestDDD.Infrastructure.Persistance;


namespace Store.IntegrationTests;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private SqliteConnection _connection;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Elimina la configuración original de DbContext
            services.RemoveAll<DbContextOptions<TodoDbContext>>();

            // Crear conexión SQLite en memoria
            _connection = new SqliteConnection("DataSource=IntegrationTests;Mode=Memory;Cache=Shared");
            _connection.Open();

            // Registrar DbContext usando SQLite
            services.AddDbContext<TodoDbContext>(options =>
            {
                options.UseSqlite(_connection);
            });

            // Construir servicio provider para inicializar DB
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<TodoDbContext>();

                db.Database.EnsureCreated();   // Crea el esquema desde cero

                SeedData(db); // 🔹 Método auxiliar para insertar datos
            }
        });

        builder.UseEnvironment("Development");
    }

    private static void SeedData(TodoDbContext db)
    {
        var userIgnasi = new User("Ignasi");
        var userAlvaro = new User("Alvaro");
        
        db.Users.AddRange(userIgnasi, userAlvaro);
        db.SaveChanges();
    }
    
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            _connection?.Close();
            _connection?.Dispose();
        }
    }

}