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
                    Text = item.Text,
                    CategoryID = item.NewsCategory.Id,
                    CategoryName = item.NewsCategory.Name,
                    ShotDate = item.Date.Value.ToString("MM-dd")
                });
            }
            return news;
        }

        // GET: api/News
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
                        LinkUrl = item.LinkUrl,
                        Text = item.Text,
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
                Text = news.Text,
                CategoryID = news.NewsCategory.Id,
                CategoryName = news.NewsCategory.Name,
                ShotDate = news.Date.Value.ToString("MM-dd")
            });
        }

        // GET: api/News/5
        [HttpGet]
        public HttpResponseMessage GetByLink(string url)
        {
            if (string.IsNullOrEmpty(url))
                return Request.CreateResponse(HttpStatusCode.NotFound, "Please check your url");

            NewsItem news = db.NewsItems.Where(x => x.LinkUrl == url.ToLower()).FirstOrDefault();
            if (news == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Please check your data, no news was found!");


            return Request.CreateResponse(HttpStatusCode.OK, new News()
            {
                Name = news.Name,
                Date = news.Date,
                Id = news.Id,
                LinkUrl = news.LinkUrl,
                Text = news.Text,
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

            if(categoryID < 0)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Please check the category id!!!!");
            NewsCategory newsCategory = db.NewsCategories.Where(x => x.Id == categoryID).FirstOrDefault();
            if(newsCategory == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Please check your data, no category was found!");
            if(!Utilities.URL.Validator.HasSpecialChars(linkUrl))
                return Request.CreateResponse(HttpStatusCode.NotFound, "Please check the link your provided, no special characters are allowded!");
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
                db.NewsItems.Add(news);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, $"{news.Name} added succefully!");
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
            if (id < 0)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Please check your id!!!!");
            NewsItem news = db.NewsItems.Where(x => x.Id == id).FirstOrDefault();
            if (news == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Please check your data, no news was found!");

            if (categoryID < 0)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Please check the category id!!!!");
            NewsCategory newsCategory = db.NewsCategories.Where(x => x.Id == categoryID).FirstOrDefault();
            if (newsCategory == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Please check your data, no category was found!");
            if (!Utilities.URL.Validator.HasSpecialChars(linkUrl))
                return Request.CreateResponse(HttpStatusCode.NotFound, "Please check the link your provided, no special characters are allowded!");
            try
            {
                news.Date = date;
                news.Name = name.ToLower();
                news.Text = text;
                news.LinkUrl = linkUrl.Replace(" ", "_").ToLower();
                news.NewsCategoryId = newsCategory.Id;
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, $"{news.Name} updated succefully!");

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
