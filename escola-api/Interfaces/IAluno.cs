namespace escola_api.Interfaces
{
    internal interface IAluno
    {
        int Id { get; set; }
        int Matricula { get; set; }
        string Nome { get; set; }
        string Email { get; set; }
        string Telefone { get; set; }
    }
}
