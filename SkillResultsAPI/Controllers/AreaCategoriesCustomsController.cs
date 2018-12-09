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
    public class AreaCategoriesCustomsController : ApiController
    {
        private SkillResultsDBEntities db = new SkillResultsDBEntities();

        // GET: api/AreaCategoriesCustoms
        public IQueryable<AreaCategoriesCustom> GetAreaCategoriesCustoms()
        {
            return db.AreaCategoriesCustoms;
        }

        // GET: api/AreaCategoriesCustoms/5
        [ResponseType(typeof(AreaCategoriesCustom))]
        public async Task<IHttpActionResult> GetAreaCategoriesCustom(int id)
        {
            AreaCategoriesCustom areaCategoriesCustom = await db.AreaCategoriesCustoms.FindAsync(id);
            if (areaCategoriesCustom == null)
            {
                return NotFound();
            }

            return Ok(areaCategoriesCustom);
        }

        // PUT: api/AreaCategoriesCustoms/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAreaCategoriesCustom(int id, AreaCategoriesCustom areaCategoriesCustom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != areaCategoriesCustom.Id)
            {
                return BadRequest();
            }

            db.Entry(areaCategoriesCustom).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AreaCategoriesCustomExists(id))
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

        // POST: api/AreaCategoriesCustoms
        [ResponseType(typeof(AreaCategoriesCustom))]
        public async Task<IHttpActionResult> PostAreaCategoriesCustom(AreaCategoriesCustom areaCategoriesCustom)
        {
            //Get Current User from Claim Token
            var User = new AccountController().getUser();

            //Get Current User and apply to the Model
            areaCategoriesCustom.UserId = User.UserId;

            //Get Current Org and apply to the Model
            areaCategoriesCustom.OrgId = User.OrgId.ToString();

            //Get Current Date & Time and apply to the Model
            areaCategoriesCustom.Created = DateTime.Now;

            //Set the Area Type to the Model
            areaCategoriesCustom.Type = "custom";

            //Set the Area Type to the Model
            areaCategoriesCustom.CategoryType = "custom";


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AreaCategoriesCustoms.Add(areaCategoriesCustom);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = areaCategoriesCustom.Id }, areaCategoriesCustom);
        }

        // DELETE: api/AreaCategoriesCustoms/5
        [ResponseType(typeof(AreaCategoriesCustom))]
        public String DeleteAreaCategoriesCustom(int id)
        {
            string sqlstring = "EXEC dbo.delete_areacategoriescustoms @categoryid = '" + id + "'";
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

        private bool AreaCategoriesCustomExists(int id)
        {
            return db.AreaCategoriesCustoms.Count(e => e.Id == id) > 0;
        }
    }
}