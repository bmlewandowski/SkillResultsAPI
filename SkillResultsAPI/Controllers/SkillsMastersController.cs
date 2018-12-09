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
    public class SkillsMastersController : ApiController
    {
        private SkillResultsDBEntities db = new SkillResultsDBEntities();

        // GET: api/SkillsMasters
        public IQueryable<SkillsMaster> GetSkillsMasters()
        {
            return db.SkillsMasters;
        }

        // GET: api/SkillsMasters/5
        [ResponseType(typeof(SkillsMaster))]
        public async Task<IHttpActionResult> GetSkillsMaster(int id)
        {
            SkillsMaster skillsmaster = await db.SkillsMasters.FindAsync(id);
            if (skillsmaster == null)
            {
                return NotFound();
            }

            return Ok(skillsmaster);
        }

        // GET: api/SkillsGroupMasters/5/
        [Route("api/SkillsGroupMasters/{id}/")]
        [ResponseType(typeof(SkillsMaster))]
        public IEnumerable<SkillsMaster> GetSkillsGroupMasters(int id)
        {
            string sqlstring = "EXEC dbo.get_skillsbycategorymasters @id = '" + id + "'";
            IEnumerable<SkillsMaster> dataObj = db.Database.SqlQuery<SkillsMaster>(sqlstring);
            return dataObj;
        }

        // PUT: api/SkillsMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSkillsMaster(int id, SkillsMaster skillsMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != skillsMaster.Id)
            {
                return BadRequest();
            }

            db.Entry(skillsMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SkillsMasterExists(id))
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

        // POST: api/SkillsMasters
        [ResponseType(typeof(SkillsMaster))]
        public async Task<IHttpActionResult> PostSkillsMaster(SkillsMaster skillsMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            //Get Current Date & Time and apply to the Model
            skillsMaster.Created = DateTime.Now;

            db.SkillsMasters.Add(skillsMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = skillsMaster.Id }, skillsMaster);
        }

        // DELETE: api/SkillsMasters/5
        [ResponseType(typeof(SkillsMaster))]
        public async Task<IHttpActionResult> DeleteSkillsMaster(int id)
        {
            SkillsMaster skillsMaster = await db.SkillsMasters.FindAsync(id);
            if (skillsMaster == null)
            {
                return NotFound();
            }

            db.SkillsMasters.Remove(skillsMaster);
            await db.SaveChangesAsync();

            return Ok(skillsMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SkillsMasterExists(int id)
        {
            return db.SkillsMasters.Count(e => e.Id == id) > 0;
        }
    }
}