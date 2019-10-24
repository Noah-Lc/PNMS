using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PNMS.Web.API.Canvas
{
    public class News
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LinkUrl { get; set; }
        public string Text { get; set; }
        public DateTime? Date { get; set; }
        public string ShotDate { get; set; }
        public string CategoryName { get; set; }
        public int CategoryID { get; set; }
    }
}