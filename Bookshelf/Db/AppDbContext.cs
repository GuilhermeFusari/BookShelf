using Microsoft.EntityFrameworkCore;
using Bookshelf.Models;

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
        public DbSet<Comentario> Comentarios {get; set;}
    }
}
