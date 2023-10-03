using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acozum_webAppMVC.Models
{
    public class CategoryClass
    {
        //HeadingManager hm = new HeadingManager(new EfHeadingDal());
        public string CategoryName { get; set; }
        public int CategoryCount { get; set; }
    }
}