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
    public class FieldsController : ApiController
    {
        private SkillResultsDBEntities db = new SkillResultsDBEntities();

        // GET: api/Fields
        public IQueryable<Field> GetFields()
        {
            return db.Fields;
        }

        // GET: api/Fields/5
        [ResponseType(typeof(Field))]
        public async Task<IHttpActionResult> GetField(int id)
        {
            Field field = await db.Fields.FindAsync(id);
            if (field == null)
            {
                return NotFound();
            }

            return Ok(field);
        }

        // PUT: api/Fields/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutField(int id, Field field)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != field.Id)
            {
                return BadRequest();
            }

            db.Entry(field).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FieldExists(id))
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

        // POST: api/Fields
        [ResponseType(typeof(Field))]
        public async Task<IHttpActionResult> PostField(Field field)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Fields.Add(field);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = field.Id }, field);
        }

        // DELETE: api/Fields/5
        [ResponseType(typeof(Field))]
        public async Task<IHttpActionResult> DeleteField(int id)
        {
            Field field = await db.Fields.FindAsync(id);
            if (field == null)
            {
                return NotFound();
            }

            db.Fields.Remove(field);
            await db.SaveChangesAsync();

            return Ok(field);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FieldExists(int id)
        {
            return db.Fields.Count(e => e.Id == id) > 0;
        }
    }
}