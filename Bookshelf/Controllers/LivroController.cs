using Microsoft.AspNetCore.Mvc;
using Bookshelf.Models;
using Bookshelf.Db;
using System.Linq;

namespace Bookshelf.Controllers
{
    public class LivroController : Controller
    {
        private readonly AppDbContext _context;

        public LivroController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Gerenciamento()
        {
            var livros = _context.Livros.ToList();
            return View(livros); 
        }

        [HttpGet]
        public IActionResult RegistroLivro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegistroLivro(Livro livro)
        {
            _context.Livros.Add(livro);
            _context.SaveChanges();

            TempData["MensagemSucesso"] = "Livro cadastrado com sucesso!";
            return RedirectToAction("Gerenciamento");
        }
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var livro = _context.Livros.FirstOrDefault(l => l.Id == id);
            if (livro == null)
                return NotFound();

            return View(livro); 
        }

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
