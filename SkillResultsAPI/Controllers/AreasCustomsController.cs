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
using System.Data.SqlClient;

namespace SkillResultsAPI.Controllers
{
    [Authorize]
    public class AreasCustomsController : ApiController
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

        // GET: api/AreasCustoms
        public IQueryable<AreasCustom> GetAreasCustoms()
        {
            return db.AreasCustoms;
        }

        // GET: api/AreasCustoms/5
        [ResponseType(typeof(AreasCustom))]
        public async Task<IHttpActionResult> GetAreasCustom(int id)
        {
            AreasCustom areasCustom = await db.AreasCustoms.FindAsync(id);
            if (areasCustom == null)
            {
                return NotFound();
            }

            return Ok(areasCustom);
        }

        // PUT: api/AreasCustoms/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAreasCustom(int id, AreasCustom areasCustom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != areasCustom.Id)
            {
                return BadRequest();
            }

            db.Entry(areasCustom).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AreasCustomExists(id))
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

        // POST: api/AreasCustoms
        [ResponseType(typeof(AreasCustom))]
        public async Task<IHttpActionResult> PostAreasCustom(AreasCustom areasCustom)
        {

            //Get Current User from Claim Token
            var User = new AccountController().getUser();

            //Get Current Date & Time and apply to the Model
            areasCustom.Created = DateTime.Now;

            //Get Current User and apply to the Model
            areasCustom.UserId = User.UserId;

            //Get Current User and apply to the Model
            areasCustom.OrgId = User.OrgId.ToString();

            //Set the Area Type to the Model
            areasCustom.Type = "custom";

            //Check to ensure the Model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Add the Model to the Database
            db.AreasCustoms.Add(areasCustom);
            await db.SaveChangesAsync();

            //Return call
            return CreatedAtRoute("DefaultApi", new { id = areasCustom.Id }, areasCustom);

        }

        // DELETE: api/AreasCustoms/5
        [ResponseType(typeof(AreasCustom))]
        public async Task<IHttpActionResult> DeleteAreasCustom(int id)
        {
            AreasCustom areasCustom = await db.AreasCustoms.FindAsync(id);
            if (areasCustom == null)
            {
                return NotFound();
            }

            db.AreasCustoms.Remove(areasCustom);
            await db.SaveChangesAsync();

            return Ok(areasCustom);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AreasCustomExists(int id)
        {
            return db.AreasCustoms.Count(e => e.Id == id) > 0;
        }
    }
}