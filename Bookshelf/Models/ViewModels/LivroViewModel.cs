namespace Bookshelf.Models.ViewModels
{
    public class LivroViewModel
    {
        public int Id { get; set; } // ID do livro
        public string Titulo { get; set; } = null!; // Título do livro
        public string Autor { get; set; } = null!; // Autor do livro
        public string? CapaUrl { get; set; } // URL da capa do livro
        public double MediaNotas { get; set; } // Média das notas dos usuários
        public int TotalAvaliacoes { get; set; } // Total de avaliações do livro
    }
}