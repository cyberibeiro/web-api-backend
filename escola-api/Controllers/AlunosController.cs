using System;
using System.Net;
using System.Web.Http;
using System.Configuration;
using System.Threading.Tasks;

namespace escola_api.Controllers
{
    public class AlunosController : ApiController
    {
        private Utils.Logger log;
        private Repositories.Aluno ObjAluno;

        public AlunosController()
        {
            log = new Utils.Logger(ConfigurationManager.AppSettings["Path"]);
            ObjAluno = new Repositories.Aluno(ConfigurationManager.ConnectionStrings["conexao"].ConnectionString);
        }

        // GET: api/Alunos
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            try
            {
                return Ok(await ObjAluno.GetAllAsync());
            }
            catch (Exception ex)
            {
                log.Log(ex);

                return InternalServerError();
            }
            
        }

        // GET: api/Alunos/5
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                var aluno = await ObjAluno.GetAsync(id);

                if (aluno == null) return NotFound();

                return Ok(aluno);
            }
            catch (Exception ex)
            {
                log.Log(ex);

                return InternalServerError(); 
            }
            
        }

        // POST: api/Alunos
        [HttpPost]
        public async Task<IHttpActionResult> Create([FromBody]Models.Aluno aluno)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await ObjAluno.CreateAsync(aluno);

                return Content(HttpStatusCode.Created, aluno);
            }
            catch (Exception ex)
            {
                log.Log(ex);

                return InternalServerError();
            }
            
        }

        // PUT: api/Alunos/5
        [HttpPut]
        public async Task<IHttpActionResult> Update(int id, [FromBody]Models.Aluno aluno)
        {
            if (aluno.Id != id) return BadRequest("O id informado no corpo da requisição é diferente do id do endpoint");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if(!await ObjAluno.UpdateAsync(aluno)) 
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                log.Log(ex);

                return InternalServerError();               
            }
        }

        // DELETE: api/Alunos/5
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {          
                if(!await ObjAluno.DeleteAsync(id)) 
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                log.Log(ex);

                return InternalServerError();
            }
            
        }
    }
}
