using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;
using System.Text;

namespace web_api.Repositories
{
    public class Usuario
    {
        private string connectionString;

        public Usuario(string ConnectionString)
        {
            connectionString = ConnectionString;
        }

        public async Task<Models.Usuario> FindUserByEmailAsync(string email)
        {
            Models.Usuario usuario = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select id, nome, email from usuarios where email = @email";
                    cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar)).Value = email;

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            usuario = new Models.Usuario
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Nome = reader["nome"].ToString(),
                                Email = reader["email"].ToString()
                            };
                        }
                    }
                }
            }

            return usuario;
        }

        public async Task<Boolean> RegisterAsync(Models.Usuario usuario)
        {
            int line = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "insert into usuarios (nome, email, senha) values (@nome, @email, @senha)";
                    cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = usuario.Nome;
                    cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar)).Value = usuario.Email;
                    cmd.Parameters.Add(new SqlParameter("@senha", SqlDbType.VarChar)).Value = Convert.ToBase64String(Encoding.UTF8.GetBytes(usuario.Senha));

                    line = await cmd.ExecuteNonQueryAsync();
                }
            }

            return line > 0;
        }

        public async Task<Models.Login> FindUserAsync(string email, string senha)
        {
            Models.Login auth = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select nome, senha from usuarios where email = @email";
                    cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar)).Value = email;

                    using (SqlDataReader user = await cmd.ExecuteReaderAsync())
                    {
                        if (await user.ReadAsync())
                        {
                            string Senha = user["senha"].ToString();

                            if (Senha == Convert.ToBase64String(Encoding.UTF8.GetBytes(senha)))
                            {
                                auth = new Models.Login();
                                auth.Nome = user["nome"].ToString();
                                auth.Auth = true;
                            }
                        }
                    }
                }
            }

            return auth;
        }
    }
}