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
    public class AreasLocalsController : ApiController
    {
        private SkillResultsDBEntities db = new SkillResultsDBEntities();

        // GET: api/AreasLocals
        public IQueryable<AreasLocal> GetAreasLocals()
        {
            return db.AreasLocals;
        }

        // GET: api/AreasLocals/5
        [ResponseType(typeof(AreasLocal))]
        public async Task<IHttpActionResult> GetAreasLocal(int id)
        {
            AreasLocal areasLocal = await db.AreasLocals.FindAsync(id);
            if (areasLocal == null)
            {
                return NotFound();
            }

            return Ok(areasLocal);
        }

        // PUT: api/AreasLocals/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAreasLocal(int id, AreasLocal areasLocal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != areasLocal.Id)
            {
                return BadRequest();
            }

            db.Entry(areasLocal).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AreasLocalExists(id))
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

        // POST: api/AreasLocals
        [ResponseType(typeof(AreasLocal))]
        public async Task<IHttpActionResult> PostAreasLocal(AreasLocal areasLocal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AreasLocals.Add(areasLocal);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = areasLocal.Id }, areasLocal);
        }

        // DELETE: api/AreasLocals/5
        [ResponseType(typeof(AreasLocal))]
        public async Task<IHttpActionResult> DeleteAreasLocal(int id)
        {
            AreasLocal areasLocal = await db.AreasLocals.FindAsync(id);
            if (areasLocal == null)
            {
                return NotFound();
            }

            db.AreasLocals.Remove(areasLocal);
            await db.SaveChangesAsync();

            return Ok(areasLocal);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AreasLocalExists(int id)
        {
            return db.AreasLocals.Count(e => e.Id == id) > 0;
        }
    }
}