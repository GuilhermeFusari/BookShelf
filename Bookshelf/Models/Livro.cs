namespace Bookshelf.Models
{
    public class Livro
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public string? Autor { get; set; }
        public string? Editora { get; set; }
        public int AnoPublicacao { get; set; }
        public required string ISBN { get; set; }
        public required string Genero { get; set; }
        public required string CapaUrl { get; set; }
    }
}
