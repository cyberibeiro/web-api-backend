using posts_api.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace posts_api.Models
{
    public class Post : IPost
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Titulo { get; set; }

        [Required]
        public string Conteudo { get; set; }

        [Required]
        [StringLength(100)]
        public string Autor { get; set; }

        public DateTime Data { get; set; }

        public Post() {

        }
    }
}