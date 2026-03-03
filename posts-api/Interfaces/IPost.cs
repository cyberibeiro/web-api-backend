using System;

namespace posts_api.Interfaces
{
    public interface IPost
    {
        int Id { get; set; }

        string Titulo { get; set; }

        string Conteudo { get; set; }

        string Autor { get; set; }

        DateTime Data { get; set; }
    }
}