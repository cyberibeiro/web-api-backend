using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

//command.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar)).Value = aluno.Nome;

namespace veiculos_api.Repositories
{
    public class Veiculo : IRepository<Models.Veiculo>
    {
        SqlConnection conn;
        SqlCommand cmd;
        public Veiculo(string connectionString) 
        {
            conn = new SqlConnection(connectionString);
            cmd = new SqlCommand();
            cmd.Connection = conn;
        }

        public async Task<List<Models.Veiculo>> ReadAllAsync()
        {
            List<Models.Veiculo> veiculos = new List<Models.Veiculo>();
            using (conn)
            {
                await conn.OpenAsync();

                using (cmd)
                {
                    cmd.CommandText = "select id, marca, nome, anomodelo, datafabricacao, valor, opcionais from veiculos;";

                    SqlDataReader dr = await cmd.ExecuteReaderAsync();

                    using (dr)
                    {
                        while (await dr.ReadAsync())
                        {
                            Models.Veiculo veiculo = new Models.Veiculo();
                            veiculo.Id = (int) dr["id"];
                            veiculo.Marca = dr["marca"].ToString();
                            veiculo.Nome = dr["nome"].ToString();
                            veiculo.AnoModelo = (int) dr["anomodelo"];
                            veiculo.DataFabricacao = (DateTime) dr["datafabricacao"];
                            veiculo.Valor = Convert.ToDouble(dr["valor"]);
                            veiculo.Opcionais = dr["opcionais"].ToString();

                            veiculos.Add(veiculo);
                        }
                    }
                }
            }

            return veiculos;
        }

        public async Task<Models.Veiculo> ReadAsync(int id)
        {
            Models.Veiculo veiculo = null;
            using (conn)
            {
                await conn.OpenAsync();

                using (cmd)
                {
                    cmd.CommandText = "select id, marca, nome, anomodelo, datafabricacao, valor, opcionais from veiculos where id = @id;";

                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                    SqlDataReader dr = await cmd.ExecuteReaderAsync();

                    using (dr)
                    {
                        if (await dr.ReadAsync())
                        {
                            veiculo = new Models.Veiculo();

                            veiculo.Id = (int)dr["id"];
                            veiculo.Marca = dr["marca"].ToString();
                            veiculo.Nome = dr["nome"].ToString();
                            veiculo.AnoModelo = (int)dr["anomodelo"];
                            veiculo.DataFabricacao = (DateTime)dr["datafabricacao"];
                            veiculo.Valor = Convert.ToDouble(dr["valor"]);
                            veiculo.Opcionais = dr["opcionais"].ToString();
                        }
                    }
                }
            }

            return veiculo;
        }

        public async Task<List<Models.Veiculo>> ReadAsync(string nome)
        {
            List<Models.Veiculo> veiculos = new List<Models.Veiculo>();
            using (conn)
            {
                await conn.OpenAsync();

                using (cmd)
                {
                    cmd.CommandText = "select id, nome, anomodelo, valor from veiculos where nome like @nome;";

                    cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = $"%{nome}%";

                    SqlDataReader dr = await cmd.ExecuteReaderAsync();

                    using (dr)
                    {
                        while (await dr.ReadAsync())
                        {
                            Models.Veiculo veiculo = new Models.Veiculo();
                            veiculo.Id = (int) dr["id"];
                            veiculo.Nome = dr["nome"].ToString();
                            veiculo.AnoModelo = (int) dr["anomodelo"];
                            veiculo.Valor = Convert.ToDouble(dr["valor"]);

                            veiculos.Add(veiculo);
                        }
                    }
                }
            }

            return veiculos;
        }

        public async Task CreateAsync(Models.Veiculo veiculo)
        {
            using (conn)
            {
                await conn.OpenAsync();

                using (cmd)
                {
                    cmd.CommandText = "insert into veiculos (marca, nome, anomodelo, datafabricacao, valor, opcionais) values (@marca, @nome, @anomodelo, @datafabricacao, @valor, @opcionais); select convert(int, SCOPE_IDENTITY());";

                    cmd.Parameters.Add(new SqlParameter("@marca", SqlDbType.VarChar)).Value = veiculo.Marca;
                    cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = veiculo.Nome;
                    cmd.Parameters.Add(new SqlParameter("@anomodelo", SqlDbType.Int)).Value = veiculo.AnoModelo;
                    cmd.Parameters.Add(new SqlParameter("@datafabricacao", SqlDbType.Date)).Value = veiculo.DataFabricacao;
                    cmd.Parameters.Add(new SqlParameter("@valor", SqlDbType.Decimal)).Value = veiculo.Valor;
                    cmd.Parameters.Add(new SqlParameter("@opcionais", SqlDbType.VarChar)).Value = veiculo.Opcionais;

                    veiculo.Id = (int) await cmd.ExecuteScalarAsync();
                    

                }
            }
        }

        public async Task<bool> UpdateAsync(Models.Veiculo veiculo)
        {
            int line = 0;

            using (conn)
            {
                await conn.OpenAsync();

                using (cmd)
                {
                    cmd.CommandText = "update veiculos set marca = @marca, nome = @nome, anomodelo = @anomodelo, datafabricacao = @datafabricacao, valor = @valor, opcionais = @opcionais where id = @id;";

                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = veiculo.Id;
                    cmd.Parameters.Add(new SqlParameter("@marca", SqlDbType.VarChar)).Value = veiculo.Marca;
                    cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = veiculo.Nome;
                    cmd.Parameters.Add(new SqlParameter("@anomodelo", SqlDbType.Int)).Value = veiculo.AnoModelo;
                    cmd.Parameters.Add(new SqlParameter("@datafabricacao", SqlDbType.Date)).Value = veiculo.DataFabricacao;
                    cmd.Parameters.Add(new SqlParameter("@valor", SqlDbType.Decimal)).Value = veiculo.Valor;
                    cmd.Parameters.Add(new SqlParameter("@opcionais", SqlDbType.VarChar)).Value = veiculo.Opcionais;

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
                    cmd.CommandText = "delete from veiculos where id = @id;";

                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                    line = await cmd.ExecuteNonQueryAsync();
                 
                }
            }

            return line > 0;
        }
    }
}