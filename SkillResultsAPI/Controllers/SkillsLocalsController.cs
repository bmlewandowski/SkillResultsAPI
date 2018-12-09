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
    public class SkillsLocalsController : ApiController
    {
        private SkillResultsDBEntities db = new SkillResultsDBEntities();

        // GET: api/SkillsLocals
        public IQueryable<SkillsLocal> GetSkillsLocals()
        {
            return db.SkillsLocals;
        }

        // GET: api/SkillsLocals/5
        [ResponseType(typeof(SkillsLocal))]
        public async Task<IHttpActionResult> GetSkillsLocal(int id)
        {
            SkillsLocal skillsLocal = await db.SkillsLocals.FindAsync(id);
            if (skillsLocal == null)
            {
                return NotFound();
            }

            return Ok(skillsLocal);
        }

        // PUT: api/SkillsLocals/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSkillsLocal(int id, SkillsLocal skillsLocal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != skillsLocal.Id)
            {
                return BadRequest();
            }

            db.Entry(skillsLocal).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SkillsLocalExists(id))
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

        // POST: api/SkillsLocals
        [ResponseType(typeof(SkillsLocal))]
        public async Task<IHttpActionResult> PostSkillsLocal(SkillsLocal skillsLocal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SkillsLocals.Add(skillsLocal);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = skillsLocal.Id }, skillsLocal);
        }

        // DELETE: api/SkillsLocals/5
        [ResponseType(typeof(SkillsLocal))]
        public async Task<IHttpActionResult> DeleteSkillsLocal(int id)
        {
            SkillsLocal skillsLocal = await db.SkillsLocals.FindAsync(id);
            if (skillsLocal == null)
            {
                return NotFound();
            }

            db.SkillsLocals.Remove(skillsLocal);
            await db.SaveChangesAsync();

            return Ok(skillsLocal);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SkillsLocalExists(int id)
        {
            return db.SkillsLocals.Count(e => e.Id == id) > 0;
        }
    }
}