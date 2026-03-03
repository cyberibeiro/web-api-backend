using System.Net;
using System.Web.Http;
using System.Configuration;
using System.Threading.Tasks;
using System;


//API Carro v1 => 2025-10-23 11:33 => métodos no padrão REST
//API Carro v2 => 2025-10-23 11:53 => IHttpActionResult para retorno dós códigos HTTP
//API Carro v3 => 2025-10-28 12:01 => utilização de Linq para facilitar as consultas nos dados
//API Carro v4 => 2025-10-29 09:30 => try / catch para tratamento de exceções
//API Carro v5 => 2025-10-29 11:23 => Injeção de dependência do Logger para logar as exceções.
//API Carro v6 => 2025-10-29 11:32 => Encapsulamento do caminho do Log no arquivo de configuração - Web.config
//API Carro v7 => 2025-11-11 12:00 => Utilização completa do repositório de dados.
//API Carro v8 => 2025-11-12 09:13 => Utilização de repositório com parâemtros para melhorar a performance e mitigar injeção de sql.
//API Carro v9 => 2025-11-12 09:30 => Implementação do DataAnnotations e validação de BadRequest.
//API Carro v10 (turbo) => 2025-11-13 12:02 =>  Implementação do Async Await - Task.

namespace biblioteca_api.Controllers
{
    public class LivrosController : ApiController
    {
        private Utils.Logger logger;
        private Repositories.Livro itemLivro;

        public LivrosController()
        {
            logger = new Utils.Logger(ConfigurationManager.AppSettings["Log"]);//Injeção de dependência
            itemLivro = new Repositories.Livro(ConfigurationManager.ConnectionStrings["conexao"].ConnectionString);
        }

        // GET: api/Livros
        [HttpGet] //anotação que permite alterar o nome da função, porém, mantém o método GET.
        public async Task<IHttpActionResult> GetAll()
        {
            //try
            //{
            //    int x = 0;
            //}
            //catch (System.TimeoutException ex)
            //{

            //    throw;
            //}
            //catch (System.DivideByZeroException ex2)
            //{

            //    throw;
            //}
            //catch (System.Exception e)
            //{

            //    throw;
            //}

            try
            {
                return Ok(await itemLivro.GetAllAsync());
            }
            catch (Exception e)
            {
                logger.Log(e);
    
                return InternalServerError();
            }
            
        }

        // GET: api/Livros/5
        public async Task<IHttpActionResult> Get(int id)
        {
            //var livro = from x in listLivros where x.Id == id select listLivros;//forma que era escrito antigamente no estilo sql.

            try
            {
                Models.Livro livro = await itemLivro.GetAsync(id);

                if (livro == null)
                    return NotFound();

                return Ok(livro);
            }
            catch (Exception e)
            {
                logger.Log(e);
                
                return InternalServerError();
            }

            //foreach (Models.Livro item in listLivros)
            //{
            //    if(item.Id == id)
            //    {
            //        return Ok(item);
            //    }
            //}

        }

        // POST: api/Livros
        public async Task<IHttpActionResult> Post([FromBody]Models.Livro livro)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await itemLivro.CreateAsync(livro);

                return Content(HttpStatusCode.Created, livro); // retorna o código 201 e o objeto livro.
            }
            catch (Exception e)
            {
                logger.Log(e);

                return InternalServerError();
            }
            
            //return Ok(livro);
        }

        // PUT: api/Livros/5
        public async Task<IHttpActionResult> Put(int id, [FromBody]Models.Livro livro)
        {
            if (livro.Id != id) return BadRequest("Id's são diferentes!");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {              
                if (!await itemLivro.UpdateAsync(livro))
                    return NotFound();

                return Ok();
            }
            catch (Exception e)
            {
                logger.Log(e);

                return InternalServerError();
            }

            //foreach (Models.Livro item in listLivros)
            //{
            //    if (item.Id == id)
            //    {
            //        item.Titulo = value.Titulo;
            //        item.Autor = value.Autor;
            //        item.Ano = value.Ano;
            //        return Ok(value);
            //    }
            //}

        }

        // DELETE: api/Livros/5
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                if (!await itemLivro.DeleteAsync(id)) 
                    return NotFound();

                return Ok();
            }
            catch (Exception e)
            {
                logger.Log(e);

                return InternalServerError();
            }

            //foreach (Models.Livro item in listLivros)
            //{
            //    if (item.Id == id)
            //    {
            //        listLivros.Remove(item);
            //        return Ok();
            //    }
            //}

        }
    }
}
