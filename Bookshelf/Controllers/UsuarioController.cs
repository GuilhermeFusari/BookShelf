using Microsoft.AspNetCore.Mvc;
using Bookshelf.Models;
using Bookshelf.Models.Enums;
using Bookshelf.Db;
using System.Linq;

namespace Bookshelf.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Usuario/Registrar
        public IActionResult Registrar()
        {
            return View();
        }

        // POST: /Usuario/Registrar
        [HttpPost]
        public IActionResult Registrar(Usuario usuario)
        {
            if (!ModelState.IsValid)
                return View(usuario);

            // Força o papel como Usuario comum
            usuario.Papel = PapelUsuario.Usuario;
            usuario.DataCadastro = DateTime.UtcNow;

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return RedirectToAction("Sucesso");
        }

        // GET: /Usuario/Sucesso
        public IActionResult Sucesso()
        {
            return View();
        }
    }
}
