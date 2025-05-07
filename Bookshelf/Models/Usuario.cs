using System.ComponentModel.DataAnnotations;

namespace Bookshelf.Models
{
    public class Usuario
    {
        public int Id { get; private set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string SenhaHash { get; set; }
        public DateTime DataCadastro { get; set; }
        public string? FotoPerfil { get; set; }
    }
}
