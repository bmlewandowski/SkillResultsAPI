﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using SkillResultsAPI.Models;


namespace SkillResultsAPI.Controllers
{
    public class AuthenticateController : ApiController
    {

        //Establish Database Connection for the Controller
        private ApplicationDbContext authdb = new ApplicationDbContext();

        /// <summary>
        /// Function to Send Email
        /// </summary>
        /// <param name="link"></param>
        /// <param name="sendaddress"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage SendMail(string link, string sendaddress, string template)
        {

            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(sendaddress, sendaddress));
            msg.From = new MailAddress("admin@skillresults.com", "SkillResults");
            msg.Subject = "SkillResults Login";

            var htmlContent = FetchTemplate.ReadFile("~/Templates/" + template);
            htmlContent = htmlContent.Replace("{@link}", link);

            msg.Body = htmlContent;
            msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("admin@skillresults.com", System.Configuration.ConfigurationManager.AppSettings["Office365Password"]);
            client.Port = 587;
            client.Host = "smtp.office365.com";
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Send(msg);

            HttpResponseMessage response2 = Request.CreateResponse(HttpStatusCode.OK, "Mail Sent");
            return response2;

        }

        // GET: api/authenticate/id
        /// <summary>
        /// Returns user object for provided userId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("api/Authenticate/{id}")]
        public IEnumerable<UserObjectViewModel> GetViewUserObject(string id)
        {
            string sqlstring = "EXEC dbo.user_object @userId = '" + id + "'";
            IEnumerable<UserObjectViewModel> userobj = authdb.Database.SqlQuery<UserObjectViewModel>(sqlstring);
            return userobj;
        }

        // GET: api/authenticate/
        /// <summary>
        /// Takes UserId and 2 provided GUIDs as Querystrings and tests that the keys are correct and that        
        /// the expiration for the keys hasn't passed and then sends the requester an authentication token.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Get([FromUri] AccessRequest value)
        {

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            SqlConnection con = new SqlConnection();
            con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (con)
            {
                string dbauthkey01 = "";
                string dbauthkey02 = "";
                DateTime dbauthexpires = DateTime.Now;

                SqlCommand command = new SqlCommand("SELECT UserId, AuthKey01, AuthKey02, AuthKeyExpires FROM OrganizationUsers WHERE UserId = '" + value.UserId + "';", con);
                con.Open();

                SqlDataReader reader = command.ExecuteReader();


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dbauthkey01 = reader.GetString(1);
                        dbauthkey02 = reader.GetString(2);
                        dbauthexpires = reader.GetDateTime(3);
                    }
                }

                else
                {
                    Console.WriteLine("No rows found.");
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NotFound, "User Not Found");
                    return response;
                }

                reader.Close();

                // Check that the provided GUIDs match the OrganizationUsers keys and that the expiration hasn't passed
                if (dbauthkey01 == value.AuthKey01 && dbauthkey02 == value.AuthKey02 && dbauthexpires > DateTime.Now)
                {

                    con.Close();
                    con.Dispose();

                    //Guid Keys have passed
                    var user = UserManager.FindById(value.UserId);
                    var userId = user.Id;
                    var tokenExpiration = TimeSpan.FromDays(1);
                    ClaimsIdentity identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));
                    identity.AddClaim(new Claim("role", "user"));
                    var props = new AuthenticationProperties()
                    {
                        IssuedUtc = DateTime.UtcNow,
                        ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration),
                    };
                    var ticket = new AuthenticationTicket(identity, props);
                    var accessToken = Startup.OAuthOptions.AccessTokenFormat.Protect(ticket);
                    JObject tokenResponse = new JObject(
                    new JProperty("userId", userId),
                    new JProperty("userName", user.UserName),
                    new JProperty("access_token", accessToken),
                    new JProperty("token_type", "bearer"),
                    new JProperty("expires_in", tokenExpiration.TotalSeconds.ToString()),
                    new JProperty(".issued", ticket.Properties.IssuedUtc.GetValueOrDefault().DateTime.ToUniversalTime()),
                    new JProperty(".expires", ticket.Properties.ExpiresUtc.GetValueOrDefault().DateTime.ToUniversalTime()));

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, tokenResponse);
                    return response;

                }
                else
                {
                    con.Close();
                    con.Dispose();

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid User Key");
                    return response;

                }

            }

        }

        // POST api/authenticate/
        /// <summary>
        /// Takes provided email address and if user exists, generates 2 new GUIDS and expiration date and
        /// saves them on the organizationuser record while generating a querystring url to be used to login.
        /// </summary>
        /// <param name="reqemail"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] ReqEmail reqemail)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            if (UserManager.FindByName(reqemail.Email) != null)
            {
                var user = UserManager.FindByName(reqemail.Email);
                var NewAuthKey01 = Guid.NewGuid().ToString();
                var NewAuthKey02 = Guid.NewGuid().ToString();
                var NewAuthKeyExpires = DateTime.Now.AddDays(1);

                //Basic UPDATE method with Parameters
                SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                SqlCommand sqlComm = new SqlCommand();
                sqlComm = sqlConn.CreateCommand();
                sqlComm.CommandText = "UPDATE OrganizationUsers SET AuthKey01=@AuthKey01,AuthKey02=@AuthKey02,AuthKeyExpires = @AuthKeyExpires WHERE UserId=@UserId";

                sqlComm.Parameters.Add("@AuthKey01", SqlDbType.VarChar);
                sqlComm.Parameters["@AuthKey01"].Value = NewAuthKey01;

                sqlComm.Parameters.Add("@AuthKey02", SqlDbType.VarChar);
                sqlComm.Parameters["@AuthKey02"].Value = NewAuthKey02;

                sqlComm.Parameters.Add("@AuthKeyExpires", SqlDbType.DateTime2);
                sqlComm.Parameters["@AuthKeyExpires"].Value = NewAuthKeyExpires;

                sqlComm.Parameters.Add("@UserId", SqlDbType.VarChar);
                sqlComm.Parameters["@UserId"].Value = user.Id;

                sqlConn.Open();
                sqlComm.ExecuteNonQuery();
                sqlConn.Close();

                var tokenResponse = "/auth/" + user.Id + "/" + NewAuthKey01 + "/" + NewAuthKey02 + "/";
              //  await SendMail(tokenResponse, user.Email, reqemail.Template);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, tokenResponse);
                return response;
            }

            else
            {

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NotFound, "User Not Found");
                return response;
            }

        }

    }
}
