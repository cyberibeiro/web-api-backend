using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace escola_api.Repositories
{
    public class Disciplina : IRepository<Models.Disciplina>
    {
        SqlConnection connection;
        SqlCommand command;
        public Disciplina(string connectionString)
        {
            connection = new SqlConnection(connectionString);
            command = new SqlCommand();
            command.Connection = connection;
        }

        public async Task<List<Models.Disciplina>> GetAllAsync()
        {
            List<Models.Disciplina> disciplinasList = new List<Models.Disciplina>();

            using (connection)
            {
                await connection.OpenAsync();

                using (command)
                {
                    command.CommandText = "select id, codigo, materia, professor from disciplinas;";

                    var disciplinas = await command.ExecuteReaderAsync();

                    using (disciplinas)
                    {
                        while (await disciplinas.ReadAsync())
                        {
                            Models.Disciplina disciplina = new Models.Disciplina();
                            disciplina.Id = (int) disciplinas["id"];
                            disciplina.Codigo = (int) disciplinas["codigo"];
                            disciplina.Materia = disciplinas["materia"].ToString();
                            disciplina.Professor = disciplinas["professor"].ToString();

                            disciplinasList.Add(disciplina);
                        }
                    }
                }
            }

            return disciplinasList;
        }

        public async Task<Models.Disciplina> GetAsync(int id)
        {
            Models.Disciplina itemDisciplina = null;

            using (connection)
            {
                await connection.OpenAsync();

                using (command)
                {
                    command.CommandText = "select id, codigo, materia, professor from disciplinas where id = @id;";

                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                    var disciplina = await command.ExecuteReaderAsync();

                    using (disciplina)
                    {
                        if(await disciplina.ReadAsync())
                        {
                            itemDisciplina = new Models.Disciplina();

                            itemDisciplina.Id = (int) disciplina["id"];
                            itemDisciplina.Codigo = (int) disciplina["codigo"];
                            itemDisciplina.Materia = disciplina["materia"].ToString();
                            itemDisciplina.Professor = disciplina["professor"].ToString();
                        
                        }
                    }
                }
            }

            return itemDisciplina;
        }

        public async Task CreateAsync(Models.Disciplina disciplina)
        {
            using (connection)
            {
                await connection.OpenAsync();

                using (command)
                {
                    command.CommandText = "insert into disciplinas (codigo, materia, professor) values (@codigo, @materia, @professor); select convert(int, SCOPE_IDENTITY());";

                    command.Parameters.Add(new SqlParameter("@codigo", SqlDbType.Int)).Value = disciplina.Codigo;
                    command.Parameters.Add(new SqlParameter("@materia", SqlDbType.VarChar)).Value = disciplina.Materia;
                    command.Parameters.Add(new SqlParameter("@professor", SqlDbType.VarChar)).Value = disciplina.Professor;

                    disciplina.Id = (int) await command.ExecuteScalarAsync();
                }
            }
        }

        public async Task<bool> UpdateAsync(Models.Disciplina disciplina)
        {

            int line = 0;

            using (connection)
            {
                await connection.OpenAsync();

                using (command)
                {
                    command.CommandText = "update disciplinas set codigo = @codigo, materia = @materia, professor = @professor where id = @id;";

                    command.Parameters.Add(new SqlParameter("@codigo", SqlDbType.Int)).Value = disciplina.Codigo;
                    command.Parameters.Add(new SqlParameter("@materia", SqlDbType.VarChar)).Value = disciplina.Materia;
                    command.Parameters.Add(new SqlParameter("@professor", SqlDbType.VarChar)).Value = disciplina.Professor;
                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = disciplina.Id;

                    line = await command.ExecuteNonQueryAsync();
                }
            }

            return line > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            int line = 0;

            using (connection)
            {
                await connection.OpenAsync();

                using (command)
                {
                    command.CommandText = "delete from disciplinas where id = @id;";

                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                    line =  await command.ExecuteNonQueryAsync();                   
                }
            }

            return line > 0;
        }
    }
}