using System;
using System.Net;
using System.Web.Http;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace veiculos_api.Controllers
{
    [RoutePrefix("api/veiculos")]
    public class VeiculosController : ApiController
    {
        private Utils.Logger logger;
        private Repositories.Veiculo repoVeiculo;

        public VeiculosController() 
        {
            logger = new Utils.Logger(ConfigurationManager.AppSettings["Path"]);
            repoVeiculo = new Repositories.Veiculo(ConfigurationManager.ConnectionStrings["conexao"].ConnectionString);
        }

        // GET: api/Veiculos
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                return Ok(await repoVeiculo.ReadAllAsync());
            }
            catch (Exception ex)
            {
                logger.Log(ex);

                return InternalServerError();              
            }
            
        }

        [Route("{id:int}")]
        // GET: api/Veiculos/5
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                var veiculo = await repoVeiculo.ReadAsync(id);

                if (veiculo == null)
                    return NotFound();

                return Ok(veiculo);
            }
            catch (Exception ex)
            {
                logger.Log(ex);

                return InternalServerError();
            }
        }

        [Route("{nome}")]
        // GET: api/Veiculos/Corolla
        public async Task<IHttpActionResult> Get(string nome)
        {
            if(nome == null || nome == "")
            {
                return BadRequest("Nome é obrigatório!");
            }

            try
            {
                return Ok(await repoVeiculo.ReadAsync(nome));
            }
            catch (Exception ex)
            {
                logger.Log(ex);

                return InternalServerError();
                
            }
        }

        // POST: api/Veiculos
        public async Task<IHttpActionResult> Post([FromBody]Models.Veiculo veiculo)
        {          
            if(veiculo == null)
                return BadRequest("Preencha todos os dados.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            try
            {
                await repoVeiculo.CreateAsync(veiculo);

                return Content(HttpStatusCode.Created, veiculo);
            }
            catch (Exception ex)
            {
                logger.Log(ex);

                return InternalServerError();
            }
        }

        [Route("{id:int}")]
        // PUT: api/Veiculos/5
        public async Task<IHttpActionResult> Put(int id, [FromBody]Models.Veiculo veiculo)
        {
            if (veiculo == null)
                return BadRequest("Preencha todos os dados.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (veiculo.Id != id)
                return BadRequest("O id informado no endpoint é diferente do id informado no corpo da requisição.");

            try
            {
                if (!await repoVeiculo.UpdateAsync(veiculo))
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                logger.Log(ex);

                return InternalServerError();
            }
        }

        [Route("{id:int}")]
        // DELETE: api/Veiculos/5
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                if (!await repoVeiculo.DeleteAsync(id))
                    return NotFound();

                return Ok();

            }
            catch (Exception ex)
            {
                logger.Log(ex);

                return InternalServerError();
            }
        }
    }
}
