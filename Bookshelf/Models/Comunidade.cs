namespace Bookshelf.Models
{
    public class Comunidade
    {
        public int Id { get; private set; }
        public required string Nome { get; set; }
        public required string Descricao { get; set; }
        public DateTime DataCriacao { get; set; }
        public int CriadorId { get; set; }
    }
}
