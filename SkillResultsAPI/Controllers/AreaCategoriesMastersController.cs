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
    public class AreaCategoriesMastersController : ApiController
    {
        private SkillResultsDBEntities db = new SkillResultsDBEntities();

        // GET: api/AreaCategoriesMasters
        public IQueryable<AreaCategoriesMaster> GetAreaCategoriesMasters()
        {
            return db.AreaCategoriesMasters;
        }

        // GET: api/AreaCategoriesMasters/5
        [ResponseType(typeof(AreaCategoriesMaster))]
        public async Task<IHttpActionResult> GetAreaCategoriesMaster(int id)
        {
            AreaCategoriesMaster areaCategoriesMaster = await db.AreaCategoriesMasters.FindAsync(id);
            if (areaCategoriesMaster == null)
            {
                return NotFound();
            }

            return Ok(areaCategoriesMaster);
        }

        // PUT: api/AreaCategoriesMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAreaCategoriesMaster(int id, AreaCategoriesMaster areaCategoriesMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != areaCategoriesMaster.Id)
            {
                return BadRequest();
            }

            db.Entry(areaCategoriesMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AreaCategoriesMasterExists(id))
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

        // POST: api/AreaCategoriesMasters
        [ResponseType(typeof(AreaCategoriesMaster))]
        public async Task<IHttpActionResult> PostAreaCategoriesMaster(AreaCategoriesMaster areaCategoriesMaster)
        {

            //Get Current Date & Time and apply to the Model
            areaCategoriesMaster.Created = DateTime.Now;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AreaCategoriesMasters.Add(areaCategoriesMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = areaCategoriesMaster.Id }, areaCategoriesMaster);
        }

        // DELETE: api/AreaCategoriesMasters/5
        [ResponseType(typeof(AreaCategoriesMaster))]
        public async Task<IHttpActionResult> DeleteAreaCategoriesMaster(int id)
        {
            AreaCategoriesMaster areaCategoriesMaster = await db.AreaCategoriesMasters.FindAsync(id);
            if (areaCategoriesMaster == null)
            {
                return NotFound();
            }

            db.AreaCategoriesMasters.Remove(areaCategoriesMaster);
            await db.SaveChangesAsync();

            return Ok(areaCategoriesMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AreaCategoriesMasterExists(int id)
        {
            return db.AreaCategoriesMasters.Count(e => e.Id == id) > 0;
        }
    }
}