using biblioteca_api.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace biblioteca_api.Models
{
    public class Livro : ILivro
    {
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; }

        [Required]
        public string Autor { get; set; }

        [Required]
        public int Ano { get; set; }

        public Livro() 
        { 

        }
    }
}