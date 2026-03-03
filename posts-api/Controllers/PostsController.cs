using System;
using System.Net;
using System.Web.Http;
using System.Configuration;
using System.Threading.Tasks;

namespace posts_api.Controllers
{
    public class PostsController : ApiController
    {
        private Tools.Logger logger;
        private Repositories.Post postRepo;
        public PostsController() 
        {
            logger = new Tools.Logger(ConfigurationManager.AppSettings["Path"]);
            postRepo = new Repositories.Post(ConfigurationManager.ConnectionStrings["conexao"].ConnectionString);
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            try
            {
                return Ok(await postRepo.GetAllAsync());
            }
            catch (Exception e)
            {
                logger.Log(e);
                return InternalServerError();
            }
        }

        // GET: api/Posts/5
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                Models.Post post = await postRepo.GetAsync(id);

                if (post == null)
                    return NotFound();

                return Ok(post);
            }
            catch (Exception e)
            {
                logger.Log(e);
                return InternalServerError();
            }
        }

        // POST: api/Posts
        [HttpPost]
        public async Task<IHttpActionResult> Create([FromBody]Models.Post post)
        {
            if (post == null)
                return BadRequest("Preencha todos os campos!");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await postRepo.CreateAsync(post);
                
                return Content(HttpStatusCode.Created, "Post criado com sucesso.");
            }
            catch (Exception e)
            {
                logger.Log(e);
                return InternalServerError();
            }
        }

        // PUT: api/Posts/5
        [HttpPut]
        public async Task<IHttpActionResult> Update(int id, [FromBody]Models.Post post)
        {
            if (post == null)
                return BadRequest("Preencha todos os campos!");

            if (id != post.Id) 
                return BadRequest("O id do endpoint é diferente do id do corpo da requisição!");  

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (!await postRepo.UpdateAsync(post))
                    return NotFound();

                return Ok();
            }
            catch (Exception e)
            {
                logger.Log(e);
                return InternalServerError();
            }
        }

        // DELETE: api/Posts/5
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                if (!await postRepo.DeleteAsync(id))
                    return NotFound();

                return Ok();
            }
            catch (Exception e)
            {
                logger.Log(e);
                return InternalServerError();
            }
        }
    }
}
