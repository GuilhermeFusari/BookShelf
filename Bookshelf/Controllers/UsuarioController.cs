using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Bookshelf.Models;
using Bookshelf.Models.Enums;
using Bookshelf.Db;
using System.Security.Claims;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Bookshelf.Models.ViewModels; // Adicione esta linha

namespace Bookshelf.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AppDbContext _context;

        // Construtor que injeta o contexto do banco de dados
        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        // Exibe a página de registro de usuários
        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }

        // Processa o registro de um novo usuário
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registrar(Usuario usuario)
        {
            // Verifica se o modelo é válido
            if (!ModelState.IsValid)
            {
                return View(usuario); // Retorna os erros de validação para a view
            }

            // Define a data de cadastro e criptografa a senha
            usuario.DataCadastro = DateTime.UtcNow;
            usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(usuario.SenhaHash);

            // Adiciona o usuário ao banco de dados
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            // Redireciona para a página de sucesso
            return RedirectToAction("Login", "Usuario");
        }

        // Exibe a página de sucesso após o registro
        public IActionResult Sucesso()
        {
            return View();
        }

        // Exibe a página de login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Processa o login do usuário
        [HttpPost]
        public async Task<IActionResult> Login(string email, string senha)
        {
            // Verifica se os campos de email e senha foram preenchidos
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
            {
                ModelState.AddModelError(string.Empty, "E-mail e senha são obrigatórios.");
                return View(); // Retorna a view com os erros de validação
            }

            // Busca o usuário no banco de dados pelo email
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            // Verifica se o usuário existe e se a senha está correta
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(senha, usuario.SenhaHash))
            {
                ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
                return View(); // Retorna a view com os erros de validação
            }

            // Cria as claims para autenticação
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Papel.ToString())
            };

            // Define a identidade do usuário
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Configura as propriedades de autenticação
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
            };

            // Realiza o login do usuário
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Redireciona para a página inicial
            return RedirectToAction("Index", "Home");
        }

        // Processa o logout do usuário
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Realiza o logout do usuário
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redireciona para a página de login
            return RedirectToAction("Login", "Usuario");
        }

        // Exibe a lista de usuários cadastrados
        [HttpGet]
        public IActionResult ListaUsuarios()
        {
            // Obtém todos os usuários do banco de dados
            var usuarios = _context.Usuarios.ToList();

            // Envia a lista para a view
            return View(usuarios);
        }

        // Exclui um usuário pelo ID
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Excluir(int id)
        {
            // Busca o usuário pelo ID
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);

            // Verifica se o usuário existe
            if (usuario == null)
            {
                TempData["MensagemErro"] = "Usuário não encontrado.";
                return RedirectToAction("ListaUsuarios");
            }

            // Remove o usuário do banco de dados
            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();

            // Define uma mensagem de sucesso e redireciona para a lista de usuários
            TempData["MensagemSucesso"] = "Usuário excluído com sucesso.";
            return RedirectToAction("ListaUsuarios");
        }


        // Exibe a página de Perfil de usuários
        [HttpGet]
        [Authorize]
        public IActionResult Perfil()
        {
            // Obtém o email do usuário autenticado
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Usuario");
            }

            // Busca o usuário no banco de dados pelo email
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario == null)
            {
                return RedirectToAction("Erro", "Home");
            }

            // Conta a quantidade de comunidades associadas ao usuário
            var quantidadeComunidades = _context.UsuarioComunidades
                .Count(uc => uc.UsuarioId == usuario.Id);

            // Conta a quantidade de livros na lista do usuário
            var quantidadeLivros = _context.LivrosNaLista
                .Count(ll => ll.ListaLivro.UsuarioId == usuario.Id);

            // Cria o ViewModel para passar os dados para a view
            var viewModel = new PerfilViewModel
            {
                Usuario = usuario,
                QuantidadeComunidades = quantidadeComunidades,
                QuantidadeLivros = quantidadeLivros
            };

            return View(viewModel);
        }
    }
}
