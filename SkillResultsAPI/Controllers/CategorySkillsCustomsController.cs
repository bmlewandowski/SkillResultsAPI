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
    public class CategorySkillsCustomsController : ApiController
    {
        private SkillResultsDBEntities db = new SkillResultsDBEntities();

        // GET: api/CategorySkillsCustoms
        public IQueryable<CategorySkillsCustom> GetCategorySkillsCustoms()
        {
            return db.CategorySkillsCustoms;
        }

        // GET: api/CategorySkillsCustoms/5
        [ResponseType(typeof(CategorySkillsCustom))]
        public async Task<IHttpActionResult> GetCategorySkillsCustom(int id)
        {
            CategorySkillsCustom categorySkillsCustom = await db.CategorySkillsCustoms.FindAsync(id);
            if (categorySkillsCustom == null)
            {
                return NotFound();
            }

            return Ok(categorySkillsCustom);
        }

        // PUT: api/CategorySkillsCustoms/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCategorySkillsCustom(int id, CategorySkillsCustom categorySkillsCustom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != categorySkillsCustom.Id)
            {
                return BadRequest();
            }

            db.Entry(categorySkillsCustom).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategorySkillsCustomExists(id))
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

        // POST: api/CategorySkillsCustoms
        [ResponseType(typeof(CategorySkillsCustom))]
        public async Task<IHttpActionResult> PostCategorySkillsCustom(CategorySkillsCustom categorySkillsCustom)
        {
            //Get Current User from Claim Token
            var User = new AccountController().getUser();

            //Get Current User and apply to the Model
            categorySkillsCustom.UserId = User.UserId;

            //Get Current Org and apply to the Model
            categorySkillsCustom.OrgId = User.OrgId.ToString();

            //Get Current Date & Time and apply to the Model
            categorySkillsCustom.Created = DateTime.Now;

            //Set the Area Type to the Model
            categorySkillsCustom.Type = "custom";

            //Set the Area Type to the Model
            categorySkillsCustom.CategoryType = "custom";

            //Set the Area Type to the Model
            categorySkillsCustom.SkillType = "custom";

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CategorySkillsCustoms.Add(categorySkillsCustom);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = categorySkillsCustom.Id }, categorySkillsCustom);
        }

        // DELETE: api/CategorySkillsCustoms/5
        [ResponseType(typeof(CategorySkillsCustom))]
        public String DeleteCategorySkillsCustom(int id)
        {
            string sqlstring = "EXEC dbo.delete_categoryskillcustoms @skillid = '" + id + "'";
            db.Database.ExecuteSqlCommand(sqlstring);
            return "deleted";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategorySkillsCustomExists(int id)
        {
            return db.CategorySkillsCustoms.Count(e => e.Id == id) > 0;
        }
    }
}