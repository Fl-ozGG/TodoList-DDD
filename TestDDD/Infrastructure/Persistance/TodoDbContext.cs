using Microsoft.EntityFrameworkCore;
using TestDDD.Domain.Entities;

namespace TestDDD.Infrastructure.Persistance;

public class TodoDbContext : DbContext
{
    public DbSet<TodoItem> TodoItems { get; set; }
    public DbSet<User> Users { get; set; }

    public TodoDbContext(DbContextOptions<TodoDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=todos.db");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<TodoItem>()
            .Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(100);
        modelBuilder.Entity<TodoItem>()
            .Property(t => t.UserId)
            .IsRequired();
    }
}