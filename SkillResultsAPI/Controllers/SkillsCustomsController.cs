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
    public class SkillsCustomsController : ApiController
    {
        private SkillResultsDBEntities db = new SkillResultsDBEntities();

        // GET: api/SkillsCustoms
        public IQueryable<SkillsCustom> GetSkillsCustoms()
        {
            return db.SkillsCustoms;
        }

        // GET: api/SkillsCustoms/5
        [ResponseType(typeof(SkillsCustom))]
        public async Task<IHttpActionResult> GetSkillsCustom(int id)
        {
            SkillsCustom skillsCustom = await db.SkillsCustoms.FindAsync(id);
            if (skillsCustom == null)
            {
                return NotFound();
            }

            return Ok(skillsCustom);
        }

        // GET: api/SkillsGroupCustoms/5/
        [Route("api/SkillsGroupCustoms/{id}/")]
        [ResponseType(typeof(SkillsCustom))]
        public IEnumerable<SkillsCustom> GetSkillsGroupCustoms(int id)
        {
            string sqlstring = "EXEC dbo.get_skillsbycategorycustoms @id = '" + id + "'";
            IEnumerable<SkillsCustom> dataObj = db.Database.SqlQuery<SkillsCustom>(sqlstring);
            return dataObj;
        }

        // PUT: api/SkillsCustoms/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSkillsCustom(int id, SkillsCustom skillsCustom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != skillsCustom.Id)
            {
                return BadRequest();
            }

            db.Entry(skillsCustom).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SkillsCustomExists(id))
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

        // POST: api/SkillsCustoms
        [ResponseType(typeof(SkillsCustom))]
        public async Task<IHttpActionResult> PostSkillsCustom(SkillsCustom skillsCustom)
        {
            //Get Current User from Claim Token
            var User = new AccountController().getUser();

            //Get Current User and apply to the Model
            skillsCustom.UserId = User.UserId;

            //Get Current Org and apply to the Model
            skillsCustom.OrgId = User.OrgId.ToString();

            //Get Current Date & Time and apply to the Model
            skillsCustom.Created = DateTime.Now;

            //Set the Area Type to the Model
            skillsCustom.Type = "custom";

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SkillsCustoms.Add(skillsCustom);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = skillsCustom.Id }, skillsCustom);
        }

        // DELETE: api/SkillsCustoms/5
        [ResponseType(typeof(SkillsCustom))]
        public async Task<IHttpActionResult> DeleteSkillsCustom(int id)
        {
            SkillsCustom skillsCustom = await db.SkillsCustoms.FindAsync(id);
            if (skillsCustom == null)
            {
                return NotFound();
            }

            db.SkillsCustoms.Remove(skillsCustom);
            await db.SaveChangesAsync();

            return Ok(skillsCustom);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SkillsCustomExists(int id)
        {
            return db.SkillsCustoms.Count(e => e.Id == id) > 0;
        }
    }
}