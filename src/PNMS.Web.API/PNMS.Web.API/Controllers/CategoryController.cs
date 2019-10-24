using DataLayer;
using PNMS.Web.API.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PNMS.Web.API.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")] //For test Only
    public class CategoryController : ApiController
    {
        EntitiesContainer db = new EntitiesContainer(); //Database context

        // GET: api/Category
        [AllowAnonymous]
        public IEnumerable<Category> Get()
        {
            List<Category> categories = new List<Category>();
            foreach(NewsCategory ctg in db.NewsCategories.ToList())
            {
                categories.Add(new Category()
                {
                    Id = ctg.Id,
                    ImageUrl = $"Uploads/Categories/{ctg.Image}",
                    Name = ctg.Name
                });
            }
            return categories;
        }

        // GET: api/Category/5
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(int id)
        {
            if (id < 0)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Please check your id!!!!");

            NewsCategory ctg = db.NewsCategories.Where(x => x.Id == id).FirstOrDefault();
            if(ctg == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Please check your data, no category was found!");

            return Request.CreateResponse(HttpStatusCode.OK, new Category() {Name = ctg.Name, ImageUrl = $"Uploads/Categories/{ctg.Image}", Id = ctg.Id });
        }

        // POST: api/Category
        [HttpPost]
        public HttpResponseMessage Store()
        {
            string name = HttpContext.Current.Request.Params["name"];

            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 25;

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png", ".jpeg" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please Upload image of type .jpg,.gif,.png,.jpeg.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format($"Please Upload a file with max size {MaxContentLength}.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            var filePath = HttpContext.Current.Server.MapPath("~/Uploads/Categories/" + postedFile.FileName);
                            postedFile.SaveAs(filePath);

                            NewsCategory ctg = new NewsCategory()
                            {
                                Image = postedFile.FileName,
                                Name = name.ToLower()
                            };

                            try
                            {
                                db.NewsCategories.Add(ctg);
                                db.SaveChanges();
                            }
                            catch
                            {
                                var msg = string.Format("Somehting Went wrong!");
                                dict.Add("error", msg);
                                return Request.CreateResponse(HttpStatusCode.InternalServerError, dict);
                            }

                        }
                    }

                    var message1 = string.Format("Category added succefully!");
                    return Request.CreateErrorResponse(HttpStatusCode.Created, message1); ;
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                var res = string.Format("Somehting Went wrong!");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }

        // PUT: api/Category/5
        [HttpPut]
        public HttpResponseMessage Put()
        {
            int id = int.Parse(HttpContext.Current.Request.Params["id"]);
            string name = HttpContext.Current.Request.Params["name"];
            if (id < 0)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Please check your id!!!!");
            NewsCategory Category = db.NewsCategories.Where(x => x.Id == id).FirstOrDefault();
            if (Category == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Please check your data, no category was found!");
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 25;

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png", ".jpeg" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please Upload image of type .jpg,.gif,.png,.jpeg.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format($"Please Upload a file with max size {MaxContentLength}.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            var filePath = HttpContext.Current.Server.MapPath("~/Uploads/Categories/" + postedFile.FileName);
                            postedFile.SaveAs(filePath);

                            try
                            {
                                Category.Image = postedFile.FileName;
                                db.SaveChanges();
                            }
                            catch
                            {
                                var msg = string.Format("Somehting Went wrong!");
                                dict.Add("error", msg);
                                return Request.CreateResponse(HttpStatusCode.InternalServerError, dict);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var res = string.Format("Somehting Went wrong!");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }

            try
            {
                Category.Name = name.ToLower();
                db.SaveChanges();
            }
            catch
            {
                var msg = string.Format("Somehting Went wrong!");
                dict.Add("error", msg);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, dict);
            }
            var message1 = string.Format("Category updated succefully!");
            return Request.CreateErrorResponse(HttpStatusCode.Created, message1); ;
        }

        // DELETE: api/Category/5
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            if(id < 0)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Please check your id!!!!");

            NewsCategory newsCategory = db.NewsCategories.Where(x => x.Id == id).FirstOrDefault();
            if (newsCategory == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Please check your data, no category was found!");
            try
            {
                db.NewsCategories.Remove(newsCategory);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, $"{newsCategory.Name} removed succefully!");
            }
            catch (Exception ex)
            {

            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError, "");
        }
    }
}
