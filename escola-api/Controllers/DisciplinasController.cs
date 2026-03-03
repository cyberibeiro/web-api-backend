using System;
using System.Net;
using System.Web.Http;
using System.Configuration;
using System.Threading.Tasks;

namespace escola_api.Controllers
{
    public class DisciplinasController : ApiController
    {
        private Utils.Logger logger;
        private Repositories.Disciplina ObjDisciplina;

        public DisciplinasController()
        {
            logger = new Utils.Logger(ConfigurationManager.AppSettings["Path"]);
            ObjDisciplina = new Repositories.Disciplina(ConfigurationManager.ConnectionStrings["conexao"].ConnectionString);
        }

        // GET: api/Disciplinas
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            try
            {
                return Ok(await ObjDisciplina.GetAllAsync());
            }
            catch (Exception ex)
            {
                logger.Log(ex);

                return InternalServerError();          
            }
            
        }

        // GET: api/Disciplinas/5
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                var disciplina = await ObjDisciplina.GetAsync(id);

                if (disciplina == null) 
                    return NotFound();

                return Ok(disciplina);
            }
            catch (Exception ex)
            {
                logger.Log(ex);

                return InternalServerError();
            }
        }

        // POST: api/Disciplinas
        [HttpPost]
        public async Task<IHttpActionResult> Create([FromBody]Models.Disciplina disciplina)
        {
            if (disciplina == null)
                return BadRequest("Preencha todos os campos!");

            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            try
            {
                await ObjDisciplina.CreateAsync(disciplina);

                return Content(HttpStatusCode.Created, disciplina);
            }
            catch (Exception ex)
            {
                logger.Log(ex);

                return InternalServerError();
            }
        }

        // PUT: api/Disciplinas/5
        [HttpPut]
        public async Task<IHttpActionResult> Update(int id, [FromBody]Models.Disciplina disciplina)
        {
            if (disciplina == null)
                return BadRequest("Preencha todos os campos!");

            if (disciplina.Id != id) 
                return BadRequest("O id do corpo da requisição é diferente do id do endpoint.");

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            try
            {
                if(!await ObjDisciplina.UpdateAsync(disciplina)) 
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                logger.Log(ex);

                return InternalServerError();
            }
        }

        // DELETE: api/Disciplinas/5
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                if(!await ObjDisciplina.DeleteAsync(id)) 
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
