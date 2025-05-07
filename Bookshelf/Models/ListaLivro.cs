namespace Bookshelf.Models
{
    public class ListaLivro
    {
        public int Id { get; private set; }
        public int UsuarioId { get; set; }
        public required string Nome { get; set; }
        public string? Descricao { get; set; }
        public bool Privada { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
