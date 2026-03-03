using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace web_api.Repositories
{
    public class Carro : IRepository<Models.Carro>
    {
        private SqlConnection SqlConnection;
        private SqlCommand command;
        public Carro(string connectionString)
        {
            SqlConnection = new SqlConnection(connectionString);
            command = new SqlCommand();
            command.Connection = SqlConnection;
        }

        public async Task<List<Models.Carro>> ReadAllAsync() //indicamos que é async o método
        {
            List<Models.Carro> carroList = new List<Models.Carro>();

            using (SqlConnection)
            {
                await SqlConnection.OpenAsync();// abri a conexão com o SGBD

                using (command)
                {
                    command.CommandText = "select id, nome, valor from carros"; //comando sql de consulta de dados.

                    using (SqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        while(await dataReader.ReadAsync())
                        {
                            Models.Carro carroObj = new Models.Carro();
                            carroObj.Id = (int) dataReader["id"];
                            carroObj.Nome = dataReader["nome"].ToString();
                            carroObj.Valor = Convert.ToDouble(dataReader["valor"]);
                            
                            carroList.Add(carroObj);
                        }
                    }
                        
                }
            }

            return carroList;

        }

        public async Task<Models.Carro> ReadAsync(int id)
        {
            Models.Carro carroObj = null;
            using (SqlConnection)
            {
                await SqlConnection.OpenAsync();// await aguarda estabelecer conexão com o BD

                using (command)
                {
                    command.CommandText = "select id, nome, valor from carros where id = @id";

                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                    SqlDataReader carro = await command.ExecuteReaderAsync();

                    using (carro)
                    {
                        if(await carro.ReadAsync())
                        {
                            carroObj = new Models.Carro();
                            carroObj.Id = (int) carro["id"];
                            carroObj.Nome = carro["nome"].ToString();
                            carroObj.Valor = Convert.ToDouble(carro["valor"]);
                        }
                       
                    }
                }
            }

            return carroObj;
        }


        public async Task<List<Models.Carro>> ReadAsync(string nome)
        {
            List<Models.Carro> carros = new List<Models.Carro>();
            using (SqlConnection)
            {
                await SqlConnection.OpenAsync();// await aguarda estabelecer conexão com o BD

                using (command)
                {
                    command.CommandText = "select id, nome, valor from carros where nome like @nome";

                    command.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = $"%{nome}%";

                    SqlDataReader carro = await command.ExecuteReaderAsync();

                    using (carro)
                    {
                        while (await carro.ReadAsync())
                        {
                            Models.Carro carroObj = new Models.Carro();

                            carroObj.Id = (int) carro["id"];
                            carroObj.Nome = carro["nome"].ToString();
                            carroObj.Valor = Convert.ToDouble(carro["valor"]);

                            carros.Add(carroObj);
                        }
                       
                    }
                }
            }

            return carros;
        }


        public async Task CreateAsync(Models.Carro carro)
        {
            using (SqlConnection)
            {
                await SqlConnection.OpenAsync();// abri a conexão com o SGBD

                using (command)
                {
                    command.CommandText = "insert into carros (nome, valor) values (@nome, @valor); select convert(int, SCOPE_IDENTITY());"; // comando sql com os dados.

                    command.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = carro.Nome;
                    command.Parameters.Add(new SqlParameter("@valor", SqlDbType.VarChar)).Value = carro.Valor;

                    carro.Id = (int) await command.ExecuteScalarAsync(); // retorna um único valor que é convertido para int.

                    //command.ExecuteNonQuery(); // transferência dos dados da API pro SGBD
                }
            }
        }

        public async Task<bool> UpdateAsync(Models.Carro carro)
        {
            int line = 0;

            using (SqlConnection)
            {
                await SqlConnection.OpenAsync();

                using (command)
                {
                    command.CommandText = "update carros set nome = @nome, valor = @valor where id = @id;";

                    command.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = carro.Nome;

                    command.Parameters.Add(new SqlParameter("@valor", SqlDbType.VarChar)).Value = carro.Valor;

                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = carro.Id;

                    line = await command.ExecuteNonQueryAsync();

                }
            }

            return line > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            int line = 0;

            using (SqlConnection)
            {
                await SqlConnection.OpenAsync();

                using (command)
                {
                    command.CommandText = "delete from carros where id = @id";

                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                    line = await command.ExecuteNonQueryAsync();                 

                }
            }

            return line > 0;

        }       
    }
}