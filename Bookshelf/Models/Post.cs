namespace Bookshelf.Models
{
    public class Post
    {
        public int Id { get; private set; }
        public int UsuarioId { get; set; }
        public int ComunidadeId { get; set; }
        public required string Titulo { get; set; }
        public required string Conteudo { get; set; }
        public DateTime DataPostagem { get; set; }
    }
}
