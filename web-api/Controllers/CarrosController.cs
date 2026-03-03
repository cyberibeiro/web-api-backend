using System;
using System.Net;
using System.Web.Http;
using System.Configuration;
using System.Threading.Tasks;

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

namespace web_api.Controllers
{
    [RoutePrefix("api/carros")]
    public class CarrosController : ApiController
    {
        private Tools.Logger logger;
        private Repositories.Carro carroObj;

        public CarrosController()
        {
            logger = new Tools.Logger(ConfigurationManager.AppSettings["PathLog"]);//Injeção de dependência
            carroObj = new Repositories.Carro(ConfigurationManager.ConnectionStrings["conexao"].ConnectionString);
        }

        // GET: api/Carros
        [HttpGet]// anotação
        public async Task<IHttpActionResult> GetAll() //indicamos que é async o método
        {
            try
            {
                return Ok(await carroObj.ReadAllAsync());
            }
            catch (Exception ex)
            {
                logger.log(ex);
                return InternalServerError();
            }

        }

        // GET: api/Carros/5
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                var carro = await carroObj.ReadAsync(id);

                if (carro == null) return NotFound();

                return Ok(carro);
            }
            catch (Exception ex)
            {
                logger.log(ex);
                return InternalServerError();
            }

            //foreach (Models.Carro carro in carrosList) 
            //{
            //    if(carro.Id == id) 
            //    { 
            //        return Ok(carro);
            //    }

            //}

        }

        // GET: api/Carros/fusca
        [Route("{nome}")]
        public async Task<IHttpActionResult> Get(string nome)
        {
            if (nome is null || nome == "")
            {
                return BadRequest("Nome é obrigatório!");
            }

            try
            {
                var carros = await carroObj.ReadAsync(nome);

                if(carros == null) return NotFound();

                return Ok(carros);
            }
            catch (Exception ex)
            {
                logger.log(ex);
                return InternalServerError();
            }

        }

        // POST: api/Carros
        public async Task<IHttpActionResult> Post([FromBody]Models.Carro carro)
        {
            if(carro == null) 
                return BadRequest("Preencha todos os campos!");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await carroObj.CreateAsync(carro);
                
                return Content(HttpStatusCode.Created, carro);
            }
            catch (Exception ex)
            {
                logger.log(ex);
                return InternalServerError();
            }
        }

        // PUT: api/Carros/5
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, [FromBody]Models.Carro carro)
        {
            if(id != carro.Id) return BadRequest("Id's do endpoint e do corpo da requisição são diferentes");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if(!await carroObj.UpdateAsync(carro)) 
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                logger.log(ex);
                return InternalServerError();
            }

            //foreach (Models.Carro carro in carrosList)
            //{
            //    if (carro.Id == id)
            //    {
            //        carro.Nome = value.Nome;
            //        carro.Valor = value.Valor;
            //        return Ok();
            //    }

            //}

            //return NotFound();
        }

        // DELETE: api/Carros/5
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                if (!await carroObj.DeleteAsync(id)) 
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                logger.log(ex);
                return InternalServerError();
            }
            //foreach (Models.Carro carro in carrosList)
            //{
            //    if (carro.Id == id)
            //    {
            //        carrosList.Remove(carro);
            //        return Ok();
            //    }

            //}

            //return NotFound();

        }
    }
}
