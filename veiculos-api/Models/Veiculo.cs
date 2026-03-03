using System;
using System.ComponentModel.DataAnnotations;

namespace veiculos_api.Models
{
    public class Veiculo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Tamanho de string inválida.")]
        public string Marca { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        public int AnoModelo { get; set; }

        [Required]
        public DateTime DataFabricacao { get; set; }

        [Required]
        public double Valor {  get; set; }

        public string Opcionais { get; set; }

        public Veiculo()
        {
        }
    }
}