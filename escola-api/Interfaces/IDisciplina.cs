namespace escola_api.Interfaces
{
    internal interface IDisciplina
    {
        int Id { get; set; }
        int Codigo { get; set; }
        string Materia { get; set; }
        string Professor { get; set; }

    }
}
