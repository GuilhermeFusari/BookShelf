using System.ComponentModel.DataAnnotations;
using Bookshelf.Models.Enums; //importando PapelUsuario

namespace Bookshelf.Models
{
    public class Usuario
    {
        public int Id { get; private set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string SenhaHash { get; set; } = string.Empty;

        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

        public string? FotoPerfil { get; set; }

        public PapelUsuario Papel { get; set; } = PapelUsuario.Usuario;

        public List<Comunidade> ComunidadesCriadas { get; set; } = new();
    }
}
