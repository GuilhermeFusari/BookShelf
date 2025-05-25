using Microsoft.EntityFrameworkCore;
using Bookshelf.Models;
using System;
using BCrypt.Net;

namespace Bookshelf.Db
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Livro> Livros { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<ListaLivro> ListasLivros { get; set; }
        public DbSet<Comunidade> Comunidades { get; set; }
        public DbSet<UsuarioComunidade> UsuarioComunidades { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Avaliacao>()
                .HasKey(a => new { a.UsuarioId, a.LivroId });

            modelBuilder.Entity<UsuarioComunidade>()
                .HasKey(uc => new { uc.UsuarioId, uc.ComunidadeId });

            // Seed usuário admin:
            modelBuilder.Entity<Usuario>().HasData(
                new
                {
                    Id = 1,
                    Nome = "Admin",
                    Email = "admin@bookshelf.com",
                    SenhaHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    DataCadastro = DateTime.UtcNow,
                    Papel = Bookshelf.Models.Enums.PapelUsuario.Administrador

                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
