namespace Bookshelf.Models
{
    public class UsuarioComunidade
    {
        public int UsuarioId { get; set; }
        public int ComunidadeId { get; set; }
        public required string Papel { get; set; }
        public DateTime DataEntrada { get; set; }
    }
}
