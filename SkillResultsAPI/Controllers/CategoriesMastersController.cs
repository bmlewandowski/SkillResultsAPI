using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SkillResultsAPI;
using System.Security.Claims;

namespace SkillResultsAPI.Controllers
{
    [Authorize]
    public class CategoriesMastersController : ApiController
    {
        private SkillResultsDBEntities db = new SkillResultsDBEntities();

        // GET: api/CategoriesMasters
        public IQueryable<CategoriesMaster> GetCategoriesMasters()
        {
            return db.CategoriesMasters;
        }

        // GET: api/CategoriesMasters/5
        [ResponseType(typeof(CategoriesMaster))]
        public async Task<IHttpActionResult> GetCategoriesMaster(int id)
        {
            CategoriesMaster categoriesMaster = await db.CategoriesMasters.FindAsync(id);
            if (categoriesMaster == null)
            {
                return NotFound();
            }

            return Ok(categoriesMaster);
        }

        // GET: api/CategoriesbyAreaMasters/5/
        [Route("api/CategoriesbyAreaMasters/{id}/")]
        [ResponseType(typeof(CategoriesMaster))]
        public IEnumerable<CategoriesMaster> GetCategoriesbyAreaMaster(int id)
        {
            string sqlstring = "EXEC dbo.get_categoriesbyareamasters @id = '" + id + "'";
            IEnumerable<CategoriesMaster> dataObj = db.Database.SqlQuery<CategoriesMaster>(sqlstring);
            return dataObj;
        }


        // PUT: api/CategoriesMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCategoriesMaster(int id, CategoriesMaster categoriesMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != categoriesMaster.Id)
            {
                return BadRequest();
            }

            db.Entry(categoriesMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriesMasterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/CategoriesMasters
        [ResponseType(typeof(CategoriesMaster))]
        public async Task<IHttpActionResult> PostCategoriesMaster(CategoriesMaster categoriesMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CategoriesMasters.Add(categoriesMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = categoriesMaster.Id }, categoriesMaster);
        }

        // DELETE: api/CategoriesMasters/5
        [ResponseType(typeof(CategoriesMaster))]
        public async Task<IHttpActionResult> DeleteCategoriesMaster(int id)
        {
            CategoriesMaster categoriesMaster = await db.CategoriesMasters.FindAsync(id);
            if (categoriesMaster == null)
            {
                return NotFound();
            }

            db.CategoriesMasters.Remove(categoriesMaster);
            await db.SaveChangesAsync();

            return Ok(categoriesMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoriesMasterExists(int id)
        {
            return db.CategoriesMasters.Count(e => e.Id == id) > 0;
        }
    }
}