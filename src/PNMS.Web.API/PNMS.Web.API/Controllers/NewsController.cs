using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PNMS.Web.API.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class NewsController : ApiController
    {
        EntitiesContainer db = new EntitiesContainer(); //Database context

        // GET: api/News
        public IEnumerable<NewsItem> Get()
        {
            return db.NewsItems.ToList();
        }

        // GET: api/News/5
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            if (id < 0)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Please check your id!!!!");

            NewsItem news = db.NewsItems.Where(x => x.Id == id).FirstOrDefault();
            if (news == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Please check your data, no news was found!");


            return Request.CreateResponse(HttpStatusCode.OK, news);
        }

        // POST: api/News
        [HttpPost]
        public HttpResponseMessage Store()
        {
            string name = HttpContext.Current.Request.Params["name"];
            string text = HttpContext.Current.Request.Params["text"];
            DateTime date = DateTime.Parse(HttpContext.Current.Request.Params["text"]);

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
                            var filePath = HttpContext.Current.Server.MapPath("~/Uploads/News/" + postedFile.FileName);
                            postedFile.SaveAs(filePath);

                            NewsItem news = new NewsItem()
                            {
                                Name = name.ToLower(),
                                Text = text,
                                ImageUrl = postedFile.FileName,
                                Date = date
                            };

                            try
                            {
                                db.NewsItems.Add(news);
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

                    var message1 = string.Format("News added succefully!");
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

        // PUT: api/News/5
        [HttpPut]
        public HttpResponseMessage Put()
        {
            int id = int.Parse(HttpContext.Current.Request.Params["id"]);
            string name = HttpContext.Current.Request.Params["name"];
            string text = HttpContext.Current.Request.Params["text"];
            DateTime date = DateTime.Parse(HttpContext.Current.Request.Params["text"]);
            if (id < 0)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Please check your id!!!!");
            NewsItem news = db.NewsItems.Where(x => x.Id == id).FirstOrDefault();
            if (news == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Please check your data, no news was found!");
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
                            var filePath = HttpContext.Current.Server.MapPath("~/Uploads/News/" + postedFile.FileName);
                            postedFile.SaveAs(filePath);

                            try
                            {
                                news.Date = date;
                                news.Name = name.ToLower();
                                news.Text = text;
                                news.ImageUrl = postedFile.FileName;
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

                    var message1 = string.Format("News updated succefully!");
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

        // DELETE: api/News/5
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            if (id < 0)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Please check your id!!!!");

            NewsItem news = db.NewsItems.Where(x => x.Id == id).FirstOrDefault();
            if (news == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Please check your data, no news was found!");
            try
            {
                db.NewsItems.Remove(news);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, $"{news.Name} removed succefully!");
            }
            catch (Exception ex)
            {

            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError, "");
        }
    }
}
