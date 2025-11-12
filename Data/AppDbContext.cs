using Microsoft.EntityFrameworkCore;
using Cozinhe_Comigo_API.Models;
using DotNetEnv;

namespace Cozinhe_Comigo_API.Data {
    public class AppDbContext : DbContext {
        private readonly string _schema;
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
            Env.Load();
            _schema = Environment.GetEnvironmentVariable("SCHEMA");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            // Define o schema a ser usado
            modelBuilder.HasDefaultSchema(_schema);
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
