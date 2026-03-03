using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace biblioteca_api.Repositories
{
    public class Livro : IRepository<Models.Livro>
    {
        private SqlConnection conexao;
        private SqlCommand command;

        public Livro(string connectionString)
        {
            conexao = new SqlConnection(connectionString);
            command = new SqlCommand();
            command.Connection = conexao;
        }

        public async Task<List<Models.Livro>> GetAllAsync()
        {
            List<Models.Livro> livroList = new List<Models.Livro>();

            using (conexao)
            {
                await conexao.OpenAsync();

                using (command)
                {
                    command.CommandText = "select id, titulo, autor, ano from livros;";

                    var dataList = await command.ExecuteReaderAsync();

                    using (dataList)
                    {
                        while(await dataList.ReadAsync())
                        {
                            Models.Livro livro = new Models.Livro();
                            livro.Id = (int) dataList["id"];
                            livro.Titulo = dataList["titulo"].ToString();
                            livro.Autor = dataList["autor"].ToString();
                            livro.Ano = (int) dataList["ano"];

                            livroList.Add(livro);
                        }
                    }
                }
            }

            return livroList;
        }

        public async Task<Models.Livro> GetAsync(int id)
        {
            Models.Livro livro = null;

            using (conexao)
            {
                await conexao.OpenAsync();

                using (command)
                {
                    command.CommandText = "select id, titulo, autor, ano from livros where id = @id;";

                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                    SqlDataReader dataLivro = await command.ExecuteReaderAsync();

                    using (dataLivro)
                    {
                        if (await dataLivro.ReadAsync())
                        {
                            livro = new Models.Livro();
                            livro.Id = (int) dataLivro["id"];
                            livro.Titulo = dataLivro["titulo"].ToString();
                            livro.Autor = dataLivro["autor"].ToString();
                            livro.Ano = (int) dataLivro["ano"];
                        }                                               
                    }
                }
            }

            return livro;
        }

        public async Task CreateAsync(Models.Livro livro)
        {
            using (conexao)
            {
                await conexao.OpenAsync();

                using (command)
                {
                    command.CommandText = "insert into livros (titulo, autor, ano) values (@titulo, @autor, @ano); select convert(int, SCOPE_IDENTITY());";

                    command.Parameters.Add(new SqlParameter("@titulo", SqlDbType.VarChar)).Value = livro.Titulo;
                    command.Parameters.Add(new SqlParameter("@autor", SqlDbType.VarChar)).Value = livro.Autor;
                    command.Parameters.Add(new SqlParameter("@ano", SqlDbType.Int)).Value = livro.Ano;

                    livro.Id = (int) await command.ExecuteScalarAsync();

                }
            }
        }

        public async Task<bool> UpdateAsync(Models.Livro livro)
        {
            int line = 0;

            using (conexao)
            {
                await conexao.OpenAsync();

                using (command)
                {
                    command.CommandText = "update livros set titulo = @titulo, autor = @autor, ano = @ano where id = @id;";

                    command.Parameters.Add(new SqlParameter("@titulo", SqlDbType.VarChar)).Value = livro.Titulo;
                    command.Parameters.Add(new SqlParameter("@autor", SqlDbType.VarChar)).Value = livro.Autor;
                    command.Parameters.Add(new SqlParameter("@ano", SqlDbType.Int)).Value = livro.Ano;
                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = livro.Id;

                    line = await command.ExecuteNonQueryAsync();             
                }
            }

            return line > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            int line = 0;

            using (conexao)
            {
                await conexao.OpenAsync();

                using (command)
                {
                    command.CommandText = "delete from livros where id = @id";

                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                    line = await command.ExecuteNonQueryAsync();                 
                }
            }

            return line > 0;
        }

    }
}