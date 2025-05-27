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
            return View();
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
        public IActionResult ListarLivros()
        {
            var livros = _context.Livros.ToList();
            return Json(livros);
        }
    }
}
