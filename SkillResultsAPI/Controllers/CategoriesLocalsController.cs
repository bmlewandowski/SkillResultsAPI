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
    public class CategoriesLocalsController : ApiController
    {
        private SkillResultsDBEntities db = new SkillResultsDBEntities();

        // GET: api/CategoriesLocals
        public IQueryable<CategoriesLocal> GetCategoriesLocals()
        {
            return db.CategoriesLocals;
        }

        // GET: api/CategoriesLocals/5
        [ResponseType(typeof(CategoriesLocal))]
        public async Task<IHttpActionResult> GetCategoriesLocal(int id)
        {
            CategoriesLocal categoriesLocal = await db.CategoriesLocals.FindAsync(id);
            if (categoriesLocal == null)
            {
                return NotFound();
            }

            return Ok(categoriesLocal);
        }

        // PUT: api/CategoriesLocals/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCategoriesLocal(int id, CategoriesLocal categoriesLocal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != categoriesLocal.Id)
            {
                return BadRequest();
            }

            db.Entry(categoriesLocal).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriesLocalExists(id))
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

        // POST: api/CategoriesLocals
        [ResponseType(typeof(CategoriesLocal))]
        public async Task<IHttpActionResult> PostCategoriesLocal(CategoriesLocal categoriesLocal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CategoriesLocals.Add(categoriesLocal);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = categoriesLocal.Id }, categoriesLocal);
        }

        // DELETE: api/CategoriesLocals/5
        [ResponseType(typeof(CategoriesLocal))]
        public async Task<IHttpActionResult> DeleteCategoriesLocal(int id)
        {
            CategoriesLocal categoriesLocal = await db.CategoriesLocals.FindAsync(id);
            if (categoriesLocal == null)
            {
                return NotFound();
            }

            db.CategoriesLocals.Remove(categoriesLocal);
            await db.SaveChangesAsync();

            return Ok(categoriesLocal);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoriesLocalExists(int id)
        {
            return db.CategoriesLocals.Count(e => e.Id == id) > 0;
        }
    }
}