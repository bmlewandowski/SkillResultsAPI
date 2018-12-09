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
    public class AreasMastersController : ApiController
    {
        private SkillResultsDBEntities db = new SkillResultsDBEntities();

        // GET: api/AreasMasters
        public IQueryable<AreasMaster> GetAreasMasters()
        {
            return db.AreasMasters;
        }

        // GET: api/AreasMasters/5
        [ResponseType(typeof(AreasMaster))]
        public async Task<IHttpActionResult> GetAreasMaster(int id)
        {
            AreasMaster areasMaster = await db.AreasMasters.FindAsync(id);
            if (areasMaster == null)
            {
                return NotFound();
            }

            return Ok(areasMaster);
        }

        // PUT: api/AreasMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAreasMaster(int id, AreasMaster areasMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != areasMaster.Id)
            {
                return BadRequest();
            }

            db.Entry(areasMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AreasMasterExists(id))
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

        // POST: api/AreasMasters
        [ResponseType(typeof(AreasMaster))]
        public async Task<IHttpActionResult> PostAreasMaster(AreasMaster areasMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AreasMasters.Add(areasMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = areasMaster.Id }, areasMaster);
        }

        // DELETE: api/AreasMasters/5
        [ResponseType(typeof(AreasMaster))]
        public async Task<IHttpActionResult> DeleteAreasMaster(int id)
        {
            AreasMaster areasMaster = await db.AreasMasters.FindAsync(id);
            if (areasMaster == null)
            {
                return NotFound();
            }

            db.AreasMasters.Remove(areasMaster);
            await db.SaveChangesAsync();

            return Ok(areasMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AreasMasterExists(int id)
        {
            return db.AreasMasters.Count(e => e.Id == id) > 0;
        }
    }
}