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
    public class DegreeLevelsController : ApiController
    {
        private SkillResultsDBEntities db = new SkillResultsDBEntities();

        // GET: api/DegreeLevels
        public IQueryable<DegreeLevel> GetDegreeLevels()
        {
            return db.DegreeLevels;
        }

        // GET: api/DegreeLevels/5
        [ResponseType(typeof(DegreeLevel))]
        public async Task<IHttpActionResult> GetDegreeLevel(int id)
        {
            DegreeLevel degreeLevel = await db.DegreeLevels.FindAsync(id);
            if (degreeLevel == null)
            {
                return NotFound();
            }

            return Ok(degreeLevel);
        }

        // PUT: api/DegreeLevels/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDegreeLevel(int id, DegreeLevel degreeLevel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != degreeLevel.Id)
            {
                return BadRequest();
            }

            db.Entry(degreeLevel).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DegreeLevelExists(id))
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

        // POST: api/DegreeLevels
        [ResponseType(typeof(DegreeLevel))]
        public async Task<IHttpActionResult> PostDegreeLevel(DegreeLevel degreeLevel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DegreeLevels.Add(degreeLevel);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = degreeLevel.Id }, degreeLevel);
        }

        // DELETE: api/DegreeLevels/5
        [ResponseType(typeof(DegreeLevel))]
        public async Task<IHttpActionResult> DeleteDegreeLevel(int id)
        {
            DegreeLevel degreeLevel = await db.DegreeLevels.FindAsync(id);
            if (degreeLevel == null)
            {
                return NotFound();
            }

            db.DegreeLevels.Remove(degreeLevel);
            await db.SaveChangesAsync();

            return Ok(degreeLevel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DegreeLevelExists(int id)
        {
            return db.DegreeLevels.Count(e => e.Id == id) > 0;
        }
    }
}