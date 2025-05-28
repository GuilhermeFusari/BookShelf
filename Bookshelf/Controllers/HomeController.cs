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

            Usuario usuario = null; // Inicializa o objeto usuário como nulo
            if (email != null)
            {
                // Busca o usuário no banco de dados pelo email
                usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);
            }

            // Obtém todos os livros cadastrados no banco de dados
            var livros = _context.Livros.ToList();

            // Cria o ViewModel para passar os dados para a view
            var viewModel = new HomeIndexViewModel
            {
                Usuario = usuario, // Usuário autenticado (se houver)
                Livros = livros // Lista de livros
            };

            // Retorna a view com o ViewModel
            return View(viewModel);
        }
    }
}
