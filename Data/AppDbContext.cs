using MeuTodo.Models;
using Microsoft.EntityFrameworkCore;

namespace MeuTodo.Data
{
    public class AppDbContext : DbContext
    {
        //É o contexto de dados da aplicação
        //É a representação do banco de memória
        //DbSet é a representação da nossa tabela
        public DbSet<Todo> Todos { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder) 
            => optionsBuilder.UseSqlite(connectionString:"DataSource=app.db;Cache=Shared");
        
    }
}
