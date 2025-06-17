using Microsoft.AspNetCore.Mvc;
using Bookshelf.Models;
using Bookshelf.Db;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Bookshelf.Models.ViewModels;
using System.Security.Claims;

namespace Bookshelf.Controllers
{
    public class LivroController : Controller
    {
        private readonly AppDbContext _context;

        // Construtor que recebe o contexto do banco de dados via injeção de dependência.
        public LivroController(AppDbContext context)
        {
            _context = context;
        }

        // Actions administrativas
        // Exibe a lista de livros cadastrados.
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult Gerenciamento()
        {
            var livros = _context.Livros.ToList();
            return View(livros); 
        }

        // Exibe o formulário para cadastrar um novo livro.
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult RegistroLivro()
        {
            return View();
        }

        // Salva um novo livro no banco de dados.
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult RegistroLivro(Livro livro)
        {
            _context.Livros.Add(livro);
            _context.SaveChanges();

            TempData["MensagemSucesso"] = "Livro cadastrado com sucesso!";
            return RedirectToAction("Gerenciamento");
        }

        // Exibe o formulário para editar um livro existente.
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(int id)
        {
            var livro = _context.Livros.FirstOrDefault(l => l.Id == id);
            if (livro == null)
                return NotFound();

            return View(livro); 
        }

        // Salva as alterações feitas em um livro existente.
        [HttpPost]
        [Authorize(Roles = "Administrador")]
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
        [Authorize(Roles = "Administrador")]
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

        // Actions públicas
        [HttpGet]
        [AllowAnonymous] // Permite que qualquer usuário acesse esta página
        public IActionResult ListaLivros(string? genero, string? autor)
        {
            // Busca todos os livros
            var livros = _context.Livros.AsQueryable();

            // Aplica o filtro por gênero, se fornecido
            if (!string.IsNullOrEmpty(genero))
            {
                livros = livros.Where(l => l.Genero != null && l.Genero.Contains(genero));
            }

            // Aplica o filtro por autor, se fornecido
            if (!string.IsNullOrEmpty(autor))
            {
                livros = livros.Where(l => l.Autor != null && l.Autor.Contains(autor));
            }

            // Cria um ViewModel para passar os dados para a view
            var viewModel = new ListaLivrosViewModel
            {
                Livros = livros.ToList(),
                Generos = _context.Livros
                    .Where(l => l.Genero != null)
                    .Select(l => l.Genero!)
                    .Distinct()
                    .ToList(),
                Autores = _context.Livros
                    .Where(l => l.Autor != null)
                    .Select(l => l.Autor!)
                    .Distinct()
                    .ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Detalhes(int id)
        {
            var livro = _context.Livros.FirstOrDefault(l => l.Id == id);
            if (livro == null)
                return NotFound();

            // Calcula a média das avaliações
            var avaliacoes = _context.Avaliacoes
                .Where(a => a.LivroId == id)
                .OrderByDescending(a => a.DataAvaliacao)
                .ToList();
            double media = avaliacoes.Any() ? avaliacoes.Average(a => a.Nota) : 0;
            int totalAvaliacoes = avaliacoes.Count;

            // Busca os nomes dos usuários para cada avaliação
            var avaliacoesComUsuario = avaliacoes.Select(a =>
            {
                var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == a.UsuarioId);
                string nome = usuario?.Nome ?? "Usuário";
                string fotoPerfil = usuario?.FotoPerfil;
                string iniciais = new string(nome.Split(' ').Select(n => n[0]).Take(2).ToArray()).ToUpper();

                return new
                {
                    UsuarioNome = nome,
                    UsuarioAvatar = string.IsNullOrEmpty(fotoPerfil) ? null : fotoPerfil,
                    UsuarioIniciais = iniciais,
                    a.Nota,
                    a.Comentario,
                    a.DataAvaliacao
                };
            }).ToList();

            ViewBag.MediaAvaliacao = media;
            ViewBag.TotalAvaliacoes = totalAvaliacoes;
            ViewBag.Avaliacoes = avaliacoesComUsuario;

            return View(livro);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult SalvarParaLerDepois(int livroId)
        {
            // Obtém o e-mail do usuário autenticado
            var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            // Busca (ou cria) a lista "Para Ler Depois" do usuário
            var lista = _context.ListaLivros.FirstOrDefault(l => l.UsuarioId == usuario.Id && l.Nome == "Para Ler Depois");
            if (lista == null)
            {
                lista = new ListaLivro
                {
                    UsuarioId = usuario.Id,
                    Nome = "Para Ler Depois",
                    DataCriacao = DateTime.Now,
                    Privada = true
                };
                _context.ListaLivros.Add(lista);
                _context.SaveChanges();
            }

            // Verifica se o livro já está na lista
            bool jaExiste = _context.LivrosNaLista.Any(x => x.ListaLivroId == lista.Id && x.LivroId == livroId);
            if (!jaExiste)
            {
                // Adiciona o livro na lista
                _context.LivrosNaLista.Add(new LivroNaLista
                {
                    ListaLivroId = lista.Id,
                    LivroId = livroId,
                    DataAdicionado = DateTime.Now
                });
                _context.SaveChanges();
            }

            return RedirectToAction("Detalhes", new { id = livroId });
        }

        [HttpGet]
        [Authorize]
        public IActionResult MeusLivros()
        {
            // Obtém o e-mail do usuário autenticado
            var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            // Busca os livros adicionados à lista "Para Ler Depois" do usuário
            var livrosNaLista = _context.LivrosNaLista
                .Where(ll => ll.ListaLivro.UsuarioId == usuario.Id)
                .Select(ll => new
                {
                    Livro = ll.Livro,
                    Avaliacao = _context.Avaliacoes.FirstOrDefault(a => a.UsuarioId == usuario.Id && a.LivroId == ll.LivroId)
                })
                .ToList();

            // Cria o ViewModel para passar os dados para a view
            var viewModel = livrosNaLista.Select(l => new LivroComAvaliacaoViewModel
            {
                LivroId = l.Livro.Id,
                Titulo = l.Livro.Titulo,
                Autor = l.Livro.Autor,
                CapaUrl = l.Livro.CapaUrl,
                Avaliacao = l.Avaliacao != null ? new AvaliacaoViewModel
                {
                    Nota = l.Avaliacao.Nota,
                    Comentario = l.Avaliacao.Comentario
                } : null
            }).ToList();

            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Avaliar(int livroId)
        {
            var livro = _context.Livros.FirstOrDefault(l => l.Id == livroId);
            if (livro == null)
            {
                return NotFound();
            }

            var viewModel = new AvaliacaoViewModel
            {
                Nota = 0,
                Comentario = string.Empty,
                Status = "Quero Ler" // Valor padrão
            };

            ViewBag.LivroTitulo = livro.Titulo;
            ViewBag.LivroCapaUrl = livro.CapaUrl; // Certifique-se de que CapaUrl contém a URL correta
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Avaliar(int livroId, AvaliacaoViewModel model)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var avaliacao = new Avaliacao
            {
                UsuarioId = usuario.Id,
                LivroId = livroId,
                Nota = model.Nota,
                Comentario = model.Comentario,
                DataAvaliacao = DateTime.Now,
                Status = "Lido"
            };

            _context.Avaliacoes.Add(avaliacao);
            _context.SaveChanges();

            return RedirectToAction("Perfil", "Usuario");
        }

        [HttpGet]
        [Authorize]
        public IActionResult EditarAvaliacao(int livroId)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var avaliacao = _context.Avaliacoes.FirstOrDefault(a => a.UsuarioId == usuario.Id && a.LivroId == livroId);
            if (avaliacao == null)
            {
                return NotFound();
            }

            var viewModel = new AvaliacaoViewModel
            {
                Nota = avaliacao.Nota,
                Comentario = avaliacao.Comentario,
                Status = avaliacao.Status // Certifique-se de que a propriedade Status existe no modelo Avaliacao
            };

            var livro = _context.Livros.FirstOrDefault(l => l.Id == livroId);
            ViewBag.LivroTitulo = livro?.Titulo;
            ViewBag.LivroCapaUrl = livro?.CapaUrl; // Certifique-se de que CapaUrl contém a URL correta

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult EditarAvaliacao(int livroId, AvaliacaoViewModel model)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            var avaliacao = _context.Avaliacoes.FirstOrDefault(a => a.UsuarioId == usuario.Id && a.LivroId == livroId);
            if (avaliacao == null)
            {
                return NotFound();
            }

            avaliacao.Nota = model.Nota;
            avaliacao.Comentario = model.Comentario;
            _context.SaveChanges();

            return RedirectToAction("MeusLivros");
        }
    }
}
