using DataLayer;
using PNMS.Web.API.Canvas;
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
    [EnableCors(origins: "*", headers: "*", methods: "*")] //For test Only
    public class NewsController : ApiController
    {
        EntitiesContainer db = new EntitiesContainer(); //Database context

        // GET: api/News
        [AllowAnonymous]
        public IEnumerable<News> Get()
        {
            List<News> news = new List<News>();
            foreach(NewsItem item in db.NewsItems.ToList())
            {
                news.Add(new News()
                {
                    Name = item.Name,
                    Date = item.Date,
                    Id = item.Id,
                    LinkUrl = item.LinkUrl,
                    NormalDate = item.Date.Value.ToShortDateString(),
                    LongDate = item.Date.Value.ToLongDateString(),
                    Text = item.Text,
                    CategoryID = item.NewsCategory.Id,
                    CategoryName = item.NewsCategory.Name,
                    ShotDate = item.Date.Value.ToString("MM-dd")
                });
            }
            return news;
        }

        // GET: api/News
        [AllowAnonymous]
        public IEnumerable<News> GetByCategory(string category)
        {
            List<News> news = new List<News>();

            if (string.IsNullOrEmpty(category))
            {
                foreach (NewsItem item in db.NewsItems.ToList())
                {
                    news.Add(new News()
                    {
                        Name = item.Name,
                        Date = item.Date,
                        Id = item.Id,
                        LongDate = item.Date.Value.ToLongDateString(),
                        LinkUrl = item.LinkUrl,
                        Text = item.Text,
                        NormalDate = item.Date.Value.ToShortDateString(),
                        CategoryID = item.NewsCategory.Id,
                        CategoryName = item.NewsCategory.Name,
                        ShotDate = item.Date.Value.ToString("MM-dd")
                    });
                }
            }

            NewsCategory _category = db.NewsCategories.Where(x => x.Name.ToLower() == category.ToLower()).FirstOrDefault();
            if (_category != null)
            {
                foreach (NewsItem item in db.NewsItems.Where(x => x.NewsCategoryId == _category.Id).ToList())
                {
                    news.Add(new News()
                    {
                        Name = item.Name,
                        Date = item.Date,
                        Id = item.Id,
                        LinkUrl = item.LinkUrl,
                        LongDate = item.Date.Value.ToLongDateString(),
                        NormalDate = item.Date.Value.ToShortDateString(),
                        Text = item.Text,
                        CategoryID = item.NewsCategory.Id,
                        CategoryName = item.NewsCategory.Name,
                        ShotDate = item.Date.Value.ToString("MM-dd")
                    });
                }
            }
            return news;
        }

        // GET: api/News/5
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(int id)
        {
            if (id < 0)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Please check your id!!!!");

            NewsItem news = db.NewsItems.Where(x => x.Id == id).FirstOrDefault();
            if (news == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Please check your data, no news was found!");


            return Request.CreateResponse(HttpStatusCode.OK, new News()
            {
                Name = news.Name,
                Date = news.Date,
                Id = news.Id,
                LinkUrl = news.LinkUrl,
                LongDate = news.Date.Value.ToLongDateString(),
                NormalDate = news.Date.Value.ToShortDateString(),
                Text = news.Text,
                CategoryID = news.NewsCategory.Id,
                CategoryName = news.NewsCategory.Name,
                ShotDate = news.Date.Value.ToString("MM-dd")
            });
        }

        // GET: api/News/5
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetByLink(string url)
        {
            //Is url empty or null
            if (string.IsNullOrEmpty(url))
                return Request.CreateResponse(HttpStatusCode.NotFound, "Please check your url");
            //Get news from database
            NewsItem news = db.NewsItems.Where(x => x.LinkUrl == url.ToLower()).FirstOrDefault();
            if (news == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Please check your data, no news was found!");

            //Return Data as json
            return Request.CreateResponse(HttpStatusCode.OK, new News()
            {
                Name = news.Name,
                Date = news.Date,
                Id = news.Id,
                LinkUrl = news.LinkUrl,
                LongDate = news.Date.Value.ToLongDateString(),
                Text = news.Text,
                NormalDate = news.Date.Value.ToShortDateString(),
                CategoryID = news.NewsCategory.Id,
                CategoryName = news.NewsCategory.Name,
                ShotDate = news.Date.Value.ToString("MM-dd")
            });
        }

        // POST: api/News
        [HttpPost]
        public HttpResponseMessage Store()
        {
            string name = HttpContext.Current.Request.Params["name"];
            string text = HttpContext.Current.Request.Params["text"];
            string linkUrl = HttpContext.Current.Request.Params["link"];
            int categoryID = int.Parse(HttpContext.Current.Request.Params["categoryid"]);
            DateTime date = DateTime.Parse(HttpContext.Current.Request.Params["date"]);
            //Verify if all the data required is valid
            if(string.IsNullOrEmpty(name) || date == null || string.IsNullOrEmpty(text))
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Name, Text and date are requierd, please check your request!");
            //Check if the category id is not negative
            if (categoryID < 0)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Please check the category id!!!!");
            //Get Category from database
            NewsCategory newsCategory = db.NewsCategories.Where(x => x.Id == categoryID).FirstOrDefault();
            if(newsCategory == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Please check your data, no category was found!");
            //Check if the url is valid and contains no special char
            //if (!Utilities.URL.Validator.HasSpecialChars(linkUrl))
            //    return Request.CreateResponse(HttpStatusCode.NotFound, "Please check the link your provided, no special characters are allowded!");
            //Create new item
            NewsItem news = new NewsItem()
            {
                Name = name.ToLower(),
                Text = text,
                LinkUrl = linkUrl.ToLower().Replace(" ", "_").ToLower(),
                Date = date,
                NewsCategoryId = newsCategory.Id
            };

            try
            {
                //Save it to Database
                db.NewsItems.Add(news);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.Created, new News()
                {
                    CategoryID = news.NewsCategoryId,
                    CategoryName = news.NewsCategory.Name,
                    Date = news.Date,
                    Name = news.Name,
                    Id = news.Id,
                    LinkUrl = news.LinkUrl,
                    LongDate = news.Date.Value.ToLongDateString(),
                    NormalDate = news.Date.Value.ToShortDateString(),
                    ShotDate = news.Date.Value.ToShortDateString(),
                    Text = news.Text
                });
            }
            catch
            {
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError, "Somehting Went wrong!");
        }

        // PUT: api/News/5
        [HttpPut]
        public HttpResponseMessage Put()
        {
            int id = int.Parse(HttpContext.Current.Request.Params["id"]);
            string name = HttpContext.Current.Request.Params["name"];
            int categoryID = int.Parse(HttpContext.Current.Request.Params["categoryid"]);
            string linkUrl = HttpContext.Current.Request.Params["link"];
            string text = HttpContext.Current.Request.Params["text"];
            DateTime date = DateTime.Parse(HttpContext.Current.Request.Params["date"]);
            //Check id if not negative
            if (id < 0)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Please check your id!!!!");
            //Get news from database
            NewsItem news = db.NewsItems.Where(x => x.Id == id).FirstOrDefault();
            if (news == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Please check your data, no news was found!");
            //Check category id if not negative for updating
            if (categoryID < 0)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Please check the category id!!!!");
            //Get the category from the database
            NewsCategory newsCategory = db.NewsCategories.Where(x => x.Id == categoryID).FirstOrDefault();
            if (newsCategory == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Please check your data, no category was found!");
            //Verify if the link is valid
            //if (!Utilities.URL.Validator.HasSpecialChars(linkUrl))
            //    return Request.CreateResponse(HttpStatusCode.NotFound, "Please check the link your provided, no special characters are allowded!");
            try
            {
                news.Date = date;
                news.Name = name.ToLower();
                news.Text = text;
                news.LinkUrl = linkUrl.Replace(" ", "_").ToLower();
                news.NewsCategoryId = newsCategory.Id;
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.Created, new News()
                {
                    CategoryID = news.NewsCategoryId,
                    CategoryName = news.NewsCategory.Name,
                    Date = news.Date,
                    Name = news.Name,
                    Id = news.Id,
                    LinkUrl = news.LinkUrl,
                    LongDate = news.Date.Value.ToLongDateString(),
                    NormalDate = news.Date.Value.ToShortDateString(),
                    ShotDate = news.Date.Value.ToShortDateString(),
                    Text = news.Text
                });
            }
            catch
            {
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError, "Somehting Went wrong!");

        }

        // DELETE: api/News/5
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            //Verify the id if not negative
            if (id < 0)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Please check your id!!!!");
            //Get news from the database
            NewsItem news = db.NewsItems.Where(x => x.Id == id).FirstOrDefault();
            if (news == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Please check your data, no news was found!");
            try
            {
                //Delete it
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
