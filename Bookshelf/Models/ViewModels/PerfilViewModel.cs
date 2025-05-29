using Bookshelf.Models.ViewModels;

namespace Bookshelf.Models.ViewModels
{
    public class PerfilViewModel
    {
        public Usuario Usuario { get; set; } = null!;
        public int QuantidadeLivros { get; set; } // Quantidade de livros na lista do usuário
        public int QuantidadeComunidades { get; set; } // Quantidade de comunidades do usuário
        public List<ListaLivro> Livros { get; set; } = new(); // Lista de livros do usuário
        public List<Comunidade> Comunidades { get; set; } = new(); // Lista de comunidades do usuário
    }
}