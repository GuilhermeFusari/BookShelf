using Bookshelf.Models.ViewModels; 
using Bookshelf.Models; 
using Bookshelf.Db; 
using Microsoft.AspNetCore.Mvc;
using System.Linq; // Importa funcionalidades para manipulação de coleções, como LINQ

namespace Bookshelf.Controllers
{
    // Controlador responsável pelas ações relacionadas à página inicial
    public class HomeController : Controller
    {
        private readonly AppDbContext _context; // Contexto do banco de dados para acessar as tabelas

        // Construtor que injeta o contexto do banco de dados
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // Método que exibe a página inicial
        public IActionResult Index()
        {
            // Obtém o email do usuário autenticado (se houver)
            var email = User.Identity?.Name;

            Usuario usuario = null;
            if (email != null)
            {
                usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);
            }

            // Obtém todos os livros cadastrados no banco de dados e calcula a média de notas e total de avaliações
            var livros = _context.Livros
                .Select(livro => new LivroViewModel
                {
                    Id = livro.Id,
                    Titulo = livro.Titulo,
                    Autor = livro.Autor,
                    CapaUrl = livro.CapaUrl,
                    MediaNotas = _context.Avaliacoes
                        .Where(a => a.LivroId == livro.Id)
                        .Average(a => (double?)a.Nota) ?? 0, // Calcula a média ou retorna 0 se não houver avaliações
                    TotalAvaliacoes = _context.Avaliacoes
                        .Count(a => a.LivroId == livro.Id) // Conta o total de avaliações para o livro
                })
                .ToList();

            // Cria o ViewModel para passar os dados para a view
            var viewModel = new HomeIndexViewModel
            {
                Usuario = usuario,
                Livros = livros
            };

            // Retorna a view com o ViewModel
            return View(viewModel);
        }
    }
}
