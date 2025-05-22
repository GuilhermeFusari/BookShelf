namespace Bookshelf.Models
{
    public class Comentario
    {
        public int Id { get; private set; }
        public int PostId { get; set; }
        public int UsuarioId { get; set; }
        public required string Conteudo { get; set; }
        public DateTime DataComentario { get; set; }
    }
}
