using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bookshelf.Models.Enums;

namespace Bookshelf.Models
{
    public class Usuario
    {
        public int Id { get; private set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [DataType(DataType.Password)]
        public string SenhaHash { get; set; } = string.Empty;

        [NotMapped]
        [Required(ErrorMessage = "A confirmação de senha é obrigatória.")]
        [DataType(DataType.Password)]
        [Compare("SenhaHash", ErrorMessage = "As senhas não coincidem.")]
        public string ConfirmarSenha { get; set; } = string.Empty;

        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

        public string? FotoPerfil { get; set; }

        public PapelUsuario Papel { get; set; } = PapelUsuario.Usuario;

        [NotMapped]
        [Required(ErrorMessage = "Você deve aceitar os termos.")]
        public bool Termos { get; set; } = false;

        public List<Comunidade> ComunidadesCriadas { get; set; } = new();
    }
}