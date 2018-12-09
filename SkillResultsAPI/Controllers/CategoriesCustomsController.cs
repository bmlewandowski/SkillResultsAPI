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
using System.Security.Claims;

namespace SkillResultsAPI.Controllers
{
    [Authorize]
    public class CategoriesCustomsController : ApiController
    {
        /// <summary>
        /// Get Current User ID from Claim of Auth Token
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public string getUserId()
        {
            var claimsIdentity = (ClaimsIdentity)this.RequestContext.Principal.Identity;
            var userId = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return userId;
        }

        private SkillResultsDBEntities db = new SkillResultsDBEntities();

        // GET: api/CategoriesCustoms
        public IQueryable<CategoriesCustom> GetCategoriesCustoms()
        {
            return db.CategoriesCustoms;
        }

        // GET: api/CategoriesCustoms/5
        [ResponseType(typeof(CategoriesCustom))]
        public async Task<IHttpActionResult> GetCategoriesCustom(int id)
        {
            CategoriesCustom categoriesCustom = await db.CategoriesCustoms.FindAsync(id);
            if (categoriesCustom == null)
            {
                return NotFound();
            }

            return Ok(categoriesCustom);
        }

        // GET: api/GetCategoriesbyAreaCustom/5/
        [Route("api/CategoriesbyAreaCustoms/{id}/")]
        [ResponseType(typeof(CategoriesCustom))]
        public IEnumerable<CategoriesMaster> GetCategoriesbyAreaCustom(int id)
        {
            string sqlstring = "EXEC dbo.get_categoriesbyareacustoms @id = '" + id + "'";
            IEnumerable<CategoriesMaster> dataObj = db.Database.SqlQuery<CategoriesMaster>(sqlstring);
            return dataObj;
        }

        // PUT: api/CategoriesCustoms/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCategoriesCustom(int id, CategoriesCustom categoriesCustom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != categoriesCustom.Id)
            {
                return BadRequest();
            }

            db.Entry(categoriesCustom).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriesCustomExists(id))
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

        // POST: api/CategoriesCustoms
        [ResponseType(typeof(CategoriesCustom))]
        public async Task<IHttpActionResult> PostCategoriesCustom(CategoriesCustom categoriesCustom)
        {
            //Get Current User from Claim Token
            var User = new AccountController().getUser();

            //Get Current User and apply to the Model
            categoriesCustom.UserId = User.UserId;

            //Get Current Org and apply to the Model
            categoriesCustom.OrgId = User.OrgId.ToString();

            //Get Current Date & Time and apply to the Model
            categoriesCustom.Created = DateTime.Now;

            //Set the Area Type to the Model
            categoriesCustom.Type = "custom";


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CategoriesCustoms.Add(categoriesCustom);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = categoriesCustom.Id }, categoriesCustom);
        }

        // DELETE: api/CategoriesCustoms/5
        [ResponseType(typeof(CategoriesCustom))]
        public async Task<IHttpActionResult> DeleteCategoriesCustom(int id)
        {
            CategoriesCustom categoriesCustom = await db.CategoriesCustoms.FindAsync(id);
            if (categoriesCustom == null)
            {
                return NotFound();
            }

            db.CategoriesCustoms.Remove(categoriesCustom);
            await db.SaveChangesAsync();

            return Ok(categoriesCustom);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoriesCustomExists(int id)
        {
            return db.CategoriesCustoms.Count(e => e.Id == id) > 0;
        }
    }
}