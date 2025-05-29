namespace Bookshelf.Models
{
    public class LivroNaLista
    {
        public int ListaLivroId { get; set; }
        public ListaLivro ListaLivro { get; set; } = null!;
        public int LivroId { get; set; }
        public Livro Livro { get; set; } = null!;
        public DateTime DataAdicionado { get; set; }
    }
}