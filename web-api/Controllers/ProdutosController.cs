using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using web_api.Models;

namespace web_api.Controllers
{
    [EnableCors(origins: "http://127.0.0.1:5500", headers: "*", methods: "*")]
    public class ProdutosController : ApiController
    {
        private static List<Produto> produtoList = new List<Produto>();

        private static int id = 0;

        public ProdutosController() 
        { 

        }


        // GET: api/Produtos
        public IHttpActionResult Get()
        {
            return Ok(produtoList);
        }

        // GET: api/Produtos/5
        public IHttpActionResult Get(int id)
        {
            foreach (Produto produto in produtoList)
            {
                if(produto.Id == id)
                {
                    return Ok(produto);
                }
            }

            return NotFound();
        }

        // POST: api/Produtos
        public IHttpActionResult Post([FromBody]Models.Produto value)
        {
            value.Id = ++id;
            produtoList.Add(value);
            return Ok();
        }

        // PUT: api/Produtos/5
        public IHttpActionResult Put(int id, [FromBody]Models.Produto value)
        {
            if(id != value.Id)
            {
                return BadRequest("Os id's são diferentes!");
            }

            foreach (Produto produto in produtoList)
            {
                if (produto.Id == id)
                {
                    produto.Nome = value.Nome;
                    produto.Valor = value.Valor;
                    return Ok(value);
                }
            }

            return NotFound();
        }

        // DELETE: api/Produtos/5
        public IHttpActionResult Delete(int id)
        {
            foreach (Produto produto in produtoList)
            {
                if(produto.Id == id)
                {
                    produtoList.Remove(produto);
                    return Ok(produto);
                }
            }

            return NotFound();
        }
    }
}
