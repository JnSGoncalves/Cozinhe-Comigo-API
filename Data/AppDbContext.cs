using Microsoft.EntityFrameworkCore;
using Cozinhe_Comigo_API.Models;
using DotNetEnv;
using Receitas.Models;


// TODO: mudar "avaliation" para "Avaliation". Da erro caso eu tente "Avaliation".
namespace Cozinhe_Comigo_API.Data
{
    public class AppDbContext : DbContext
    {
        private readonly string _schema;
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Env.Load();
            _schema = Environment.GetEnvironmentVariable("SCHEMA");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define o schema a ser usado
            modelBuilder.HasDefaultSchema(_schema);
        }

        //Tabelas que deseja acessar no banco de dados.
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Avaliation> avaliations { get; set; }

    }
}
