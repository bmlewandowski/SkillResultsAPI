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
    public class DegreeTypesController : ApiController
    {
        private SkillResultsDBEntities db = new SkillResultsDBEntities();

        // GET: api/DegreeTypes
        public IQueryable<DegreeType> GetDegreeTypes()
        {
            return db.DegreeTypes;
        }

        // GET: api/DegreeTypes/5
        [ResponseType(typeof(DegreeType))]
        public async Task<IHttpActionResult> GetDegreeType(int id)
        {
            DegreeType degreeType = await db.DegreeTypes.FindAsync(id);
            if (degreeType == null)
            {
                return NotFound();
            }

            return Ok(degreeType);
        }

        // PUT: api/DegreeTypes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDegreeType(int id, DegreeType degreeType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != degreeType.Id)
            {
                return BadRequest();
            }

            db.Entry(degreeType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DegreeTypeExists(id))
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

        // POST: api/DegreeTypes
        [ResponseType(typeof(DegreeType))]
        public async Task<IHttpActionResult> PostDegreeType(DegreeType degreeType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DegreeTypes.Add(degreeType);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = degreeType.Id }, degreeType);
        }

        // DELETE: api/DegreeTypes/5
        [ResponseType(typeof(DegreeType))]
        public async Task<IHttpActionResult> DeleteDegreeType(int id)
        {
            DegreeType degreeType = await db.DegreeTypes.FindAsync(id);
            if (degreeType == null)
            {
                return NotFound();
            }

            db.DegreeTypes.Remove(degreeType);
            await db.SaveChangesAsync();

            return Ok(degreeType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DegreeTypeExists(int id)
        {
            return db.DegreeTypes.Count(e => e.Id == id) > 0;
        }
    }
}