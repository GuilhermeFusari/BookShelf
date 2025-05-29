using System.Collections.Generic;

namespace Bookshelf.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public Usuario? Usuario { get; set; } // Usuário autenticado (opcional)
        public List<LivroViewModel> Livros { get; set; } = new(); // Lista de livros para exibir na página inicial
    }
}