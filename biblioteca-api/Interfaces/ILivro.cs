using System;


namespace biblioteca_api.Interfaces
{
    internal interface ILivro
    {
        int Id { get; set; }
        string Titulo { get; set; }
        string Autor { get; set; }
        int Ano { get; set; }
    }
}
