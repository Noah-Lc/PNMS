using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PNMS.Web.API.Canvas
{
    /// <summary>
    /// Using this class to return data to users with json
    /// you can change the properties to hide/show more/less data to users
    /// </summary>
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}