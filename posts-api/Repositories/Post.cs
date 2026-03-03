using posts_api.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace posts_api.Repositories
{
    public class Post : IRepository<Models.Post>
    {
        SqlConnection conn;
        SqlCommand cmd;

        public Post(string connectionString) 
        {
            conn = new SqlConnection(connectionString);
            cmd = new SqlCommand();
            cmd.Connection = conn;
        }

        public async Task<List<Models.Post>> GetAllAsync()
        {
            List<Models.Post> posts = new List<Models.Post>();

            using (conn)
            {
                await conn.OpenAsync();

                using (cmd)
                {
                    cmd.CommandText = "select id, titulo, conteudo, autor, data from posts;";

                    SqlDataReader dados = await cmd.ExecuteReaderAsync();

                    using (dados)
                    {
                        while (await dados.ReadAsync())
                        {
                            Models.Post post = new Models.Post();

                            post.Id = (int) dados["id"];
                            post.Titulo = dados["titulo"].ToString();
                            post.Conteudo = dados["conteudo"].ToString();
                            post.Autor = dados["autor"].ToString();
                            post.Data = (DateTime) dados["data"];
                            
                            posts.Add(post);
                        }
                    }
                }
            }

            return posts;
        }

        public async Task<Models.Post> GetAsync(int id)
        {
            Models.Post post = null;

            using (conn)
            {
                await conn.OpenAsync();

                using (cmd)
                {
                    cmd.CommandText = "select id, titulo, conteudo, autor, data from posts where id = @id;";

                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                    SqlDataReader dado = await cmd.ExecuteReaderAsync();

                    using (dado)
                    {
                        if (await dado.ReadAsync())
                        {
                            post = new Models.Post();

                            post.Id = (int) dado["id"];
                            post.Titulo = dado["titulo"].ToString();
                            post.Conteudo = dado["conteudo"].ToString();
                            post.Autor = dado["autor"].ToString();
                            post.Data = (DateTime) dado["data"];
                        }
                    }
                }
            }

            return post;
        }

        public async Task CreateAsync(Models.Post post)
        {
            using (conn)
            {
                await conn.OpenAsync();

                using (cmd)
                {
                    cmd.CommandText = "insert into posts (titulo, conteudo, autor, data) values (@titulo, @conteudo, @autor, @data);";

                    cmd.Parameters.Add(new SqlParameter("@titulo", SqlDbType.VarChar)).Value = post.Titulo;

                    cmd.Parameters.Add(new SqlParameter("@conteudo", SqlDbType.VarChar)).Value = post.Conteudo;

                    cmd.Parameters.Add(new SqlParameter("@autor", SqlDbType.VarChar)).Value = post.Autor;   
                    
                    cmd.Parameters.Add(new SqlParameter("@data", SqlDbType.DateTime)).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");                  

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }       
      
        public async Task<bool> UpdateAsync(Models.Post post)
        {
            int line = 0;

            using (conn)
            {
                await conn.OpenAsync();

                using (cmd)
                {
                    cmd.CommandText = "update posts set titulo = @titulo, conteudo = @conteudo, autor = @autor where id = @id;";

                    cmd.Parameters.Add(new SqlParameter("@titulo", SqlDbType.VarChar)).Value = post.Titulo;
                    cmd.Parameters.Add(new SqlParameter("@conteudo",SqlDbType.VarChar)).Value = post.Conteudo;
                    cmd.Parameters.Add(new SqlParameter("@autor", SqlDbType.VarChar)).Value = post.Autor;
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = post.Id;

                    line = await cmd.ExecuteNonQueryAsync();
                }
            }

            return line > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            int line = 0;

            using (conn)
            {
                await conn.OpenAsync();

                using (cmd)
                {
                    cmd.CommandText = "delete from posts where id = @id;";

                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                    line = await cmd.ExecuteNonQueryAsync();
                }
            }

            return line > 0;
        }
    }
}