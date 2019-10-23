using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PNMS.Web.API.Controllers
{
    public class UserController : ApiController
    {
        EntitiesContainer db = new EntitiesContainer(); //Database context

        /// <summary>
        /// Create new User
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public HttpResponseMessage POST(string username, string password, string firstname, string lastname, string email)
        {
            //Check email is its valid
            if (!Utilities.Email.Validation.IsValid(email))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "You have to specify a valid email address!");
            //Check if the user set a password
            if(string.IsNullOrEmpty(password))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "You have to specify a valid password!");
            //Check if the password is strong enough
            if(Utilities.PasswordAdvisor.CheckStrength(password) == Utilities.PasswordAdvisor.PasswordScore.Blank &&
                Utilities.PasswordAdvisor.CheckStrength(password) == Utilities.PasswordAdvisor.PasswordScore.VeryWeak &&
                Utilities.PasswordAdvisor.CheckStrength(password) == Utilities.PasswordAdvisor.PasswordScore.Weak)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Your password is not strong enough Boy!");

            //Verify the user first and last name
            if(string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(lastname))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "You have to specify your first and last name!");

            User newUser = new User() {
                Email = email.ToLower(),
                FirstName = firstname.ToLower(),
                LastName = lastname.ToLower(),
                Password = Utilities.PasswordHasher.Hash(password),
                UserName = username ?? email,
                Phone = ""
            };
            try
            {
                db.Users.Add(newUser);
                db.SaveChanges();
            }
            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Oops!");
            }

            return Request.CreateResponse(HttpStatusCode.OK, "User added succesfully");
        }
    }
}
