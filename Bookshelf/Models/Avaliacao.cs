namespace Bookshelf.Models
{
    public class Avaliacao
    {
        public int UsuarioId { get; set; }
        public int LivroId { get; set; }

        public int Nota { get; set; } 
        public required string Comentario { get; set; }
        public required string Status { get; set; } 
        public DateTime DataAvaliacao { get; set; }
    }
}
