using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;

namespace escola_api.Repositories
{
    public class Aluno : IRepository<Models.Aluno>
    {
        SqlConnection connection;
        SqlCommand command;

        public Aluno(string connectionString)
        {
            connection = new SqlConnection(connectionString);
            command = new SqlCommand();
            command.Connection = connection;

        }

        public async Task<List<Models.Aluno>> GetAllAsync()
        {
            List<Models.Aluno> alunosList = new List<Models.Aluno>();
            using (connection)
            {
                await connection.OpenAsync();

                using (command)
                {
                    command.CommandText = "select id, matricula, nome, email, telefone from alunos;";

                    var alunos = await command.ExecuteReaderAsync();

                    using (alunos)
                    {
                        while (await alunos.ReadAsync())
                        {
                            Models.Aluno aluno = new Models.Aluno();
                            aluno.Id = (int) alunos["id"];
                            aluno.Matricula = (int) alunos["matricula"];
                            aluno.Nome = alunos["nome"].ToString();
                            aluno.Email = alunos["email"].ToString();
                            aluno.Telefone = alunos["telefone"].ToString(); 

                            alunosList.Add(aluno);
                        }
                    }                   
                }
            }

            return alunosList;
        }

        public async Task<Models.Aluno> GetAsync(int id)
        {
            Models.Aluno aluno = null;
            using (connection)
            {
                await connection.OpenAsync();

                using (command)
                {
                    command.CommandText = "select id, matricula, nome, email, telefone from alunos where id = @id;";

                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                    var alunos = await command.ExecuteReaderAsync();

                    using (alunos)
                    {
                        if (await alunos.ReadAsync())
                        {
                            aluno = new Models.Aluno();

                            aluno.Id = (int) alunos["id"];
                            aluno.Matricula = (int) alunos["matricula"];
                            aluno.Nome = alunos["nome"].ToString();
                            aluno.Email = alunos["email"].ToString();
                            aluno.Telefone = alunos["telefone"].ToString();

                        }                     
                        
                    }
                }
            }

            return aluno;
        }

        public async Task CreateAsync(Models.Aluno aluno)
        {
            using (connection)
            {
                await connection.OpenAsync();

                using (command)
                {
                    command.CommandText = "insert into alunos (matricula, nome, email, telefone) values (@matricula, @nome, @email, @telefone); select convert(int, SCOPE_IDENTITY());";

                    command.Parameters.Add(new SqlParameter("@matricula", SqlDbType.Int)).Value = aluno.Matricula;
                    command.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = aluno.Nome;
                    command.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar)).Value = aluno.Email;
                    command.Parameters.Add(new SqlParameter("@telefone", SqlDbType.VarChar)).Value = aluno.Telefone;
                  
                    aluno.Id = (int) await command.ExecuteScalarAsync();

                    //command.ExecuteNonQuery();
                }
            }
        }

        public async Task<bool> UpdateAsync(Models.Aluno aluno)
        {
            int line = 0;

            using (connection)
            {
                await connection.OpenAsync();

                using (command)
                {
                    command.CommandText = "update alunos set matricula = @matricula, nome = @nome, email = @email, telefone = @telefone where id = @id;";

                    command.Parameters.Add(new SqlParameter("@matricula", SqlDbType.Int)).Value = aluno.Matricula;
                    command.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = aluno.Nome;
                    command.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar)).Value = aluno.Email;
                    command.Parameters.Add(new SqlParameter("@telefone", SqlDbType.VarChar)).Value = aluno.Telefone;
                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = aluno.Id;

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
                    command.CommandText = "delete from alunos where id = @id;";

                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                    line = await command.ExecuteNonQueryAsync();                 
                }
            }

            return line > 0;
        }

    }
}