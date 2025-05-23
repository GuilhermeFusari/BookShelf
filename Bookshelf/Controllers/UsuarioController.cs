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

        // GET: /Usuario/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Usuario/Login
        [HttpPost]
        public IActionResult Login(string email, string senha)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email && u.SenhaHash == senha);


            if (usuario != null)
            {
                // Aqui você pode adicionar lógica de autenticação (cookies, sessão, etc.)
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos");
            return View();
        }
    }
}
