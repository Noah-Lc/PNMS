﻿using DataLayer;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PNMS.Web.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")] //JUST FOR TESTING
    public class UserController : ApiController
    {
        EntitiesContainer db = new EntitiesContainer(); //Database context

        /// <summary>
        /// Create new User
        /// </summary
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public HttpResponseMessage POST(FormDataCollection formData)
        {
            string username = formData["username"];
            string password = formData["password"];
            string firstname = formData["firstname"];
            string lastname = formData["lastname"];
            string email = formData["email"];
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
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Oops!");
            }

            return Request.CreateResponse(HttpStatusCode.OK, "User added succesfully");
        }

        public HttpResponseMessage Get(int id)
        {
            //if(id < 0)

            return Request.CreateResponse(HttpStatusCode.OK, "User added succesfully");
        }
    }
}