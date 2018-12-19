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
    public class CertificationsController : ApiController
    {
        private SkillResultsDBEntities db = new SkillResultsDBEntities();

        // GET: api/Certifications
        public IQueryable<Certification> GetCertifications()
        {
            return db.Certifications;
        }

        // GET: api/Certifications/5
        [ResponseType(typeof(Certification))]
        public async Task<IHttpActionResult> GetCertification(int id)
        {
            Certification certification = await db.Certifications.FindAsync(id);
            if (certification == null)
            {
                return NotFound();
            }

            return Ok(certification);
        }

        // PUT: api/Certifications/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCertification(int id, Certification certification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != certification.Id)
            {
                return BadRequest();
            }

            db.Entry(certification).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CertificationExists(id))
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

        // POST: api/Certifications
        [ResponseType(typeof(Certification))]
        public async Task<IHttpActionResult> PostCertification(Certification certification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Certifications.Add(certification);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = certification.Id }, certification);
        }

        // DELETE: api/Certifications/5
        [ResponseType(typeof(Certification))]
        public async Task<IHttpActionResult> DeleteCertification(int id)
        {
            Certification certification = await db.Certifications.FindAsync(id);
            if (certification == null)
            {
                return NotFound();
            }

            db.Certifications.Remove(certification);
            await db.SaveChangesAsync();

            return Ok(certification);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CertificationExists(int id)
        {
            return db.Certifications.Count(e => e.Id == id) > 0;
        }
    }
}