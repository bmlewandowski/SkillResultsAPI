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

namespace SkillResultsAPI.Controllers
{
    [Authorize]
    public class CategorySkillsMastersController : ApiController
    {
        private SkillResultsDBEntities db = new SkillResultsDBEntities();

        // GET: api/CategorySkillsMasters
        public IQueryable<CategorySkillsMaster> GetCategorySkillsMasters()
        {
            return db.CategorySkillsMasters;
        }

        // GET: api/CategorySkillsMasters/5
        [ResponseType(typeof(CategorySkillsMaster))]
        public async Task<IHttpActionResult> GetCategorySkillsMaster(int id)
        {
            CategorySkillsMaster categorySkillsMaster = await db.CategorySkillsMasters.FindAsync(id);
            if (categorySkillsMaster == null)
            {
                return NotFound();
            }

            return Ok(categorySkillsMaster);
        }

        // PUT: api/CategorySkillsMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCategorySkillsMaster(int id, CategorySkillsMaster categorySkillsMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != categorySkillsMaster.Id)
            {
                return BadRequest();
            }

            db.Entry(categorySkillsMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategorySkillsMasterExists(id))
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

        // POST: api/CategorySkillsMasters
        [ResponseType(typeof(CategorySkillsMaster))]
        public async Task<IHttpActionResult> PostCategorySkillsMaster(CategorySkillsMaster categorySkillsMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CategorySkillsMasters.Add(categorySkillsMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = categorySkillsMaster.Id }, categorySkillsMaster);
        }

        // DELETE BY SKILL ID: api/CategorySkillsBySkillMasters/5
        [Route("api/CategorySkillsBySkillMasters/{id}")]
        [ResponseType(typeof(CategorySkillsMaster))]
        public String DeleteCategorySkillsBySkillMaster(int id)
        {
            string sqlstring = "delete from CategorySkillsMasters where SkillId = " + id;

            db.Database.ExecuteSqlCommand(sqlstring);

            return "Deleted";
        }

        // DELETE: api/CategorySkillsMasters/5
        [ResponseType(typeof(CategorySkillsMaster))]
        public async Task<IHttpActionResult> DeleteCategorySkillsMaster(int id)
        {
            CategorySkillsMaster categorySkillsMaster = await db.CategorySkillsMasters.FindAsync(id);
            if (categorySkillsMaster == null)
            {
                return NotFound();
            }

            db.CategorySkillsMasters.Remove(categorySkillsMaster);
            await db.SaveChangesAsync();

            return Ok(categorySkillsMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategorySkillsMasterExists(int id)
        {
            return db.CategorySkillsMasters.Count(e => e.Id == id) > 0;
        }
    }
}