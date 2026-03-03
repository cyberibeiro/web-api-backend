using escola_api.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace escola_api.Models
{
    public class Disciplina : IDisciplina
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public int Codigo { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Materia { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string Professor { get; set; }

        public Disciplina()
        {

        }
    }
}