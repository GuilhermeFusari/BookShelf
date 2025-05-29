namespace Bookshelf.Models.ViewModels
{
    public class ListaLivrosViewModel
    {
        public List<Livro> Livros { get; set; } = new();
        public List<string> Generos { get; set; } = new();
        public List<string> Autores { get; set; } = new();
    }
}