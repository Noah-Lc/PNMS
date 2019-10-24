using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PNMS.Web.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")] //For test Only
    public class AuthentificationController : ApiController
    {
        EntitiesContainer db = new EntitiesContainer(); //Database context

        [HttpPost]
        public async Task<HttpResponseMessage> POST(FormDataCollection formData)
        {
            string username = formData["username"];
            string password = formData["password"];
            //Check if the username and password are valid
            if (string.IsNullOrEmpty(username) || string.IsNullOrWhiteSpace(password))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Username or Password not correct");
            //Get user from database
            User user = db.Users.Where(x => x.UserName == username.ToLower()).FirstOrDefault();
            if(user == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Username or Password not correct [user]");
            //Verify user password
            if (!Utilities.PasswordHasher.Verify(password, user.Password))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Username or Password not correct [password]");
            else//If everything is GOOD
            {
                var issuer = "http://localhost:52530/api";
                var authority = "http://localhost:52530/";
                var privateKey = "vr8h8cjHjcUbnvGppVLvvV8rWZYvDcTZhjnybn82n72Ay9XzmQh9kmM8jrEfQNXr8mSLANvPfYk4JYwnerhWda9Zyypxwcx2kBhb6f6yTENsjtDgGbBYdfQyXPSLbpXUq2xVtXaMqw5xvR7x7dekjGHAfUs3WjMfuweT9wMEd4RvPwcnFfJKrhUhVmrcYPs3R5pNF9qUenTXppGLUGdCUDegvmMGVqA3FDvwwmPe7ZepK2KTdxhU8cSVEvRAxw8K";
                var daysValid = 1;

                var createJwt = await Handlers.AuthentificationTokenGenerator.CreateJWTAsync(user, issuer, authority, privateKey, daysValid);
                //Return Token to user
                return Request.CreateResponse(HttpStatusCode.OK, new Dictionary<string, string>() {
                    { "access_token", createJwt },
                    { "token_type", "bearer" },
                    { "expires_in", Utilities.Time.UnixTimeStamp(DateTime.UtcNow.AddDays(daysValid)).ToString() },
                    { "username", user.UserName }
                });
            }
        }
    }
}
