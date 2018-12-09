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
    public class UserSkillsController : ApiController
    {
        private SkillResultsDBEntities db = new SkillResultsDBEntities();

        // GET: api/UserSkills
        public IQueryable<UserSkill> GetUserSkills()
        {
            return db.UserSkills;
        }

        // GET: api/UserSkills/5
        /// <summary>
        /// Get User Skill for given userskillId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [Route("api/UserSkills/{id}/{type}")]
        public async Task<IHttpActionResult> GetUsersSkillDetails(int id, string type)
        {
            if (type == "master")

            {
                var query = from usr in db.UserSkills
                            join ski in db.SkillsMasters on usr.SkillId equals ski.Id
                            where usr.SkillId == id
                            select new
                            {
                                usr.Id,
                                usr.SkillId,
                                usr.Rating,
                                usr.Created,
                                usr.Modified,
                                usr.Private,
                                ski.Name,
                                ski.Description,
                                ski.Type

                            };

                return Ok(query);

            }

            else
            {
                var query = from usr in db.UserSkills
                            join ski in db.SkillsCustoms on usr.SkillId equals ski.Id
                            where usr.SkillId == id
                            select new
                            {
                                usr.Id,
                                usr.SkillId,
                                usr.Rating,
                                usr.Created,
                                usr.Modified,
                                usr.Private,
                                ski.Name,
                                ski.Description,
                                ski.Type

                            };

                return Ok(query);
            }

        }

        // GET: api/UserSkills/GetUsersSkills/5/custom
        /// <summary>
        /// Returns the Skills associated with a User
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [Route("api/UserSkills/GetUsersSkills/{id}/{type}")]
        public async Task<IHttpActionResult> GetUsersSkills(string id, string type)
        {

            if (type == "master")

            {
                var query = from usr in db.UserSkills
                            join ski in db.SkillsMasters on usr.SkillId equals ski.Id
                            where usr.UserId == id
                            select new
                            {
                                usr.Rating,
                                usr.Created,
                                usr.Modified,
                                ski.Name,
                                ski.Description,
                                ski.Id,
                                ski.Type

                            };

                return Ok(query);

            }
            else
            {
                var query = from usr in db.UserSkills
                            join ski in db.SkillsCustoms on usr.SkillId equals ski.Id
                            where usr.UserId == id
                            select new
                            {
                                usr.Rating,
                                usr.Created,
                                usr.Modified,
                                ski.Name,
                                ski.Description,
                                ski.Id,
                                ski.Type

                            };

                return Ok(query);

            }
        }

        // GET: api/UserSkills/GetSelectedSkill/5/custom
        /// <summary>
        /// Get detail for skill to add 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [Route("api/UserSkills/GetSelectedSkill/{id}/{type}")]
        public async Task<IHttpActionResult> GetSelectedSkill(int id, string type)
        {
            if (type == "master")

            {
                var query = from ski in db.SkillsMasters
                            where ski.Id == id
                            select new
                            {
                                ski.Id,
                                ski.Name,
                                ski.Description,
                                ski.Type
                            };

                return Ok(query);

            }

            else
            {
                var query = from ski in db.SkillsCustoms
                            where ski.Id == id
                            select new
                            {
                                ski.Id,
                                ski.Name,
                                ski.Description,
                                ski.Type
                            };

                return Ok(query);
            }

        }

        // PUT: api/UserSkills/5
        /// <summary>
        /// Edit a User's Skill
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userSkill"></param>
        /// <returns></returns>
        [Route("api/UserSkills/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUserSkill(int id, UserSkill userSkill)
        {
            var User = new AccountController().getUser();
            userSkill.UserId = User.UserId;
            userSkill.Modified = DateTime.Now;
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userSkill.Id)
            {
                return BadRequest();
            }

            db.Entry(userSkill).State = EntityState.Modified;

            try
            {
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

       
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserSkillExists(id))
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

        // POST: api/UserSkills
        /// <summary>
        /// Create a new User association with a Skill
        /// </summary>
        /// <param name="userSkill"></param>
        /// <returns></returns>
        [ResponseType(typeof(UserSkill))]
        public async Task<IHttpActionResult> PostUserSkill(UserSkill userSkill)
        {

            var User = new AccountController().getUser();
            userSkill.UserId = User.UserId;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool recordAlreadyExists = db.UserSkills.Any(x => x.SkillId == userSkill.SkillId && x.UserId == userSkill.UserId);



            if (!recordAlreadyExists)
            {
                userSkill.Created = DateTime.Now;
                db.UserSkills.Add(userSkill);
                await db.SaveChangesAsync();

                return CreatedAtRoute("DefaultApi", new { id = userSkill.Id }, userSkill);
            }
            else
            {
                return NotFound();
            }
        }

        // DELETE: api/UserSkills/5
        /// <summary>
        /// Remove a User Skill association
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(UserSkill))]
        public async Task<IHttpActionResult> DeleteUserSkill(int id)
        {
            UserSkill userSkill = await db.UserSkills.FindAsync(id);
            if (userSkill == null)
            {
                return NotFound();
            }

            db.UserSkills.Remove(userSkill);
            await db.SaveChangesAsync();

            return Ok(userSkill);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserSkillExists(int id)
        {
            return db.UserSkills.Count(e => e.Id == id) > 0;
        }
    }
}