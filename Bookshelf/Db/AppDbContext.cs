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
        public DbSet<ListaLivro> ListaLivros { get; set; }
        public DbSet<LivroNaLista> LivrosNaLista { get; set; } // <-- Adicione aqui
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

            modelBuilder.Entity<LivroNaLista>()
                .HasKey(ll => new { ll.ListaLivroId, ll.LivroId });

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
