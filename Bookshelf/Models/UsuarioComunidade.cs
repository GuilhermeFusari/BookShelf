using Bookshelf.Models.Enums;

namespace Bookshelf.Models
{
    public class UsuarioComunidade
    {
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public int ComunidadeId { get; set; }
        public Comunidade Comunidade { get; set; } = null!;

        public PapelComunidade Papel { get; set; }
        public DateTime DataEntrada { get; set; }
    }
}
