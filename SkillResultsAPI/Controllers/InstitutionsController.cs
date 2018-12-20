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
    public class InstitutionsController : ApiController
    {
        private SkillResultsDBEntities db = new SkillResultsDBEntities();

        // GET: api/Institutions
        public IQueryable<Institution> GetInstitutions()
        {
            return db.Institutions;
        }

        // GET: api/GetInstitutionsByState/CA
        [Route("api/GetInstitutionsByState/{state}/")]
        public IQueryable<Institution> GetInstitutionsByState(string state)
        {
            return db.Institutions.Where(x => x.State == state);

        }

        // GET: api/Institutions/5
        [ResponseType(typeof(Institution))]
        public async Task<IHttpActionResult> GetInstitution(int id)
        {
            Institution institution = await db.Institutions.FindAsync(id);
            if (institution == null)
            {
                return NotFound();
            }

            return Ok(institution);
        }

        // PUT: api/Institutions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutInstitution(int id, Institution institution)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != institution.Id)
            {
                return BadRequest();
            }

            db.Entry(institution).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstitutionExists(id))
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

        // POST: api/Institutions
        [ResponseType(typeof(Institution))]
        public async Task<IHttpActionResult> PostInstitution(Institution institution)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Institutions.Add(institution);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = institution.Id }, institution);
        }

        // DELETE: api/Institutions/5
        [ResponseType(typeof(Institution))]
        public async Task<IHttpActionResult> DeleteInstitution(int id)
        {
            Institution institution = await db.Institutions.FindAsync(id);
            if (institution == null)
            {
                return NotFound();
            }

            db.Institutions.Remove(institution);
            await db.SaveChangesAsync();

            return Ok(institution);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InstitutionExists(int id)
        {
            return db.Institutions.Count(e => e.Id == id) > 0;
        }
    }
}