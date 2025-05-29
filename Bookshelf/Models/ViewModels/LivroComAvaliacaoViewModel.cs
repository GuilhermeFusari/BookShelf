namespace Bookshelf.Models.ViewModels
{
    public class LivroComAvaliacaoViewModel
    {
        public int LivroId { get; set; }
        public string Titulo { get; set; } = null!;
        public string Autor { get; set; } = null!;
        public string? CapaUrl { get; set; }
        public AvaliacaoViewModel? Avaliacao { get; set; }
    }

    public class AvaliacaoViewModel
    {
        public int Nota { get; set; }
        public string Comentario { get; set; } = null!;
        public string Status { get; set; } = "Quero Ler"; 
    }
}