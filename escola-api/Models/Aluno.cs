using escola_api.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace escola_api.Models
{
    public class Aluno : IAluno
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public int Matricula { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required(AllowEmptyStrings = false)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Phone]
        public string Telefone { get; set; }

        public Aluno()
        {

        }
    }
}