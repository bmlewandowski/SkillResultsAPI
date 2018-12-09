using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using SkillResultsAPI.Models;
using System.IO;
using System.Web.Http.Description;

namespace SkillResultsAPI.Controllers
{
    [Authorize]
    public class OrganizationUsersController : ApiController
    {
        // GET: api/OrganizationUsers/GetOrganizationsUsers/
        /// <summary>
        /// Returns list of Users in the logged in User's Organization
        /// </summary>
        /// <returns></returns>
        [Route("api/OrganizationUsers/GetOrganizationsUsers/")]
        public HttpResponseMessage GetOrganizationsUsers()
        {

            var User = new AccountController().getUser();
            var userId = User.UserId;

            List<OrganizationUsers> OrganizationUsers = new List<OrganizationUsers>();

            if (userId != null)
            {
                SqlDataReader reader = null;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Connection = con;
                con.Open();
                sqlCmd.CommandText = "SELECT OrgId FROM OrganizationUsers WHERE UserId = @UserId";

                sqlCmd.Parameters.Add("@UserId", SqlDbType.VarChar);
                sqlCmd.Parameters["@UserId"].Value = userId;

                var OrgId = sqlCmd.ExecuteScalar();


                sqlCmd.CommandText = "SELECT o.OrgId, o.UserId, o.Created, o.Admin, a.UserName FROM OrganizationUsers as o, AspNetUsers as a WHERE o.UserId = a.Id  AND o.OrgId = @OrgId";

                sqlCmd.Parameters.Add("@OrgId", SqlDbType.Int);
                sqlCmd.Parameters["@OrgId"].Value = OrgId;

                reader = sqlCmd.ExecuteReader();

                while (reader.Read())
                {
                    OrganizationUsers f = new OrganizationUsers();
                    f.UserName = (string)reader["UserName"];
                    f.OrgId = (int)reader["OrgId"];
                    f.UserId = (string)reader["UserId"];
                    f.Created = (DateTime)reader["Created"];
                    f.Admin = (int)reader["Admin"];
                    OrganizationUsers.Add(f);
                }

                con.Close();
            }

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, OrganizationUsers);
            return response;
        }

        // GET: api/OrganizationUsers/name@company.com
        /// <summary>
        /// See if OrganizationUser exists from email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [ResponseType(typeof(OrganizationUsers))]
        [AllowAnonymous]
        [Route("api/OrganizationUsers/GetOrganizationUser/")]
        public HttpResponseMessage GetOrganizationUser([FromUri]string email)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Connection = con;
            con.Open();
            sqlCmd.CommandText = "Select 1 FROM AspNetUsers WHERE Email = @Email";

            sqlCmd.Parameters.Add("@Email", SqlDbType.VarChar);
            sqlCmd.Parameters["@Email"].Value = email;

            var exists = sqlCmd.ExecuteScalar();
            con.Close();

            HttpResponseMessage response = new HttpResponseMessage();

            if (exists == null)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, 0);
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.OK, exists);
            }
                      
            return response;
        }

        // GET: api/OrganizationUsers/GetOrganizationMaxUsers/
        /// <summary>
        /// Returns maximum number of allowed users for logged in users organization
        /// </summary>
        /// <returns></returns>
        [Route("api/OrganizationUsers/GetOrganizationMaxUsers/")]
        public HttpResponseMessage GetOrganizationMaxUsers()
        {

            var User = new AccountController().getUser();
            var userId = User.UserId;
            int MaxUsers = 0;

            if (userId != null)
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Connection = con;
                con.Open();
                sqlCmd.CommandText = "SELECT OrgId FROM OrganizationUsers WHERE UserId = @UserId";

                sqlCmd.Parameters.Add("@UserId", SqlDbType.VarChar);
                sqlCmd.Parameters["@UserId"].Value = userId;

                var OrgId = sqlCmd.ExecuteScalar();


                sqlCmd.CommandText = "SELECT MaxUsers FROM Organizations WHERE Id = @OrgId";

                sqlCmd.Parameters.Add("@OrgId", SqlDbType.Int);
                sqlCmd.Parameters["@OrgId"].Value = OrgId;

                object result = sqlCmd.ExecuteScalar();
                result = (result == DBNull.Value) ? null : result;
                MaxUsers = Convert.ToInt32(result);

                con.Close();
            }

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, MaxUsers);
            return response;
        }

        // POST: api/OrganizationUsers/
        /// <summary>
        /// Parses uploaded csv document into datatable
        /// </summary>
        /// <param name="upload"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> PostUserList(System.Web.HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {

                if (upload != null && upload.ContentLength > 0)
                {

                    if (upload.FileName.EndsWith(".csv"))
                    {
                        //Stream stream = upload.InputStream;
                        DataTable dt = new DataTable();

                        using (StreamReader sr = new StreamReader(upload.InputStream))
                        {
                            string[] headers = sr.ReadLine().Split(',');
                            foreach (string header in headers)
                            {
                                dt.Columns.Add(header);
                            }
                            while (!sr.EndOfStream)
                            {
                                string[] rows = sr.ReadLine().Split(',');
                                DataRow dr = dt.NewRow();
                                for (int i = 0; i < headers.Length; i++)
                                {
                                    dr[i] = rows[i];
                                }
                                dt.Rows.Add(dr);
                            }

                        }

                        return Created($"api/resource/", dt);
                    }
                    else
                    {
                        ModelState.AddModelError("File", "This file format is not supported");
                        return NotFound();
                    }
                }
                else
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                }
            }
            return BadRequest(ModelState);
        }

    }
}
