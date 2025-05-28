using Microsoft.AspNetCore.Mvc;
using Bookshelf.Models;
using Bookshelf.Db;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Bookshelf.Controllers
{
    // Este controller é responsável pelo CRUD de livros.
    // Apenas usuários com a role "Administrador" podem acessar as actions deste controller.
    [Authorize(Roles = "Administrador")]
    public class LivroController : Controller
    {
        private readonly AppDbContext _context;

        // Construtor que recebe o contexto do banco de dados via injeção de dependência.
        public LivroController(AppDbContext context)
        {
            _context = context;
        }

        // Exibe a lista de livros cadastrados.
        [HttpGet]
        public IActionResult Gerenciamento()
        {
            var livros = _context.Livros.ToList();
            return View(livros); 
        }

        // Exibe o formulário para cadastrar um novo livro.
        [HttpGet]
        public IActionResult RegistroLivro()
        {
            return View();
        }

        // Salva um novo livro no banco de dados.
        [HttpPost]
        public IActionResult RegistroLivro(Livro livro)
        {
            _context.Livros.Add(livro);
            _context.SaveChanges();

            TempData["MensagemSucesso"] = "Livro cadastrado com sucesso!";
            return RedirectToAction("Gerenciamento");
        }

        // Exibe o formulário para editar um livro existente.
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var livro = _context.Livros.FirstOrDefault(l => l.Id == id);
            if (livro == null)
                return NotFound();

            return View(livro); 
        }

        // Salva as alterações feitas em um livro existente.
        [HttpPost]
        public IActionResult Editar(Livro livro)
        {
            var livroDb = _context.Livros.FirstOrDefault(l => l.Id == livro.Id);
            if (livroDb == null)
                return NotFound();

            livroDb.Titulo = livro.Titulo;
            livroDb.Autor = livro.Autor;
            livroDb.Editora = livro.Editora;
            livroDb.AnoPublicacao = livro.AnoPublicacao;
            livroDb.ISBN = livro.ISBN;
            livroDb.Genero = livro.Genero;
            livroDb.CapaUrl = livro.CapaUrl;

            _context.SaveChanges();

            return RedirectToAction("Gerenciamento");
        }

        // Remove um livro do banco de dados.
        [HttpGet]
        public IActionResult Excluir(int id)
        {
            var livro = _context.Livros.FirstOrDefault(l => l.Id == id);
            if (livro == null)
                return NotFound();

            _context.Livros.Remove(livro);
            _context.SaveChanges();

            TempData["MensagemSucesso"] = "Livro excluído com sucesso!";
            return RedirectToAction("Gerenciamento");
        }
    }
}
