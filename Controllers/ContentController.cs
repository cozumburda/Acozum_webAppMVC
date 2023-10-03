using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Acozum_webAppMVC.Controllers
{
    public class ContentController : Controller
    {
        // GET: Content
        ContentManager ctm = new ContentManager(new EfContentDal());
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetAllContent(string p)
        {
            var valuesa= ctm.GetList();
            var valuesp = ctm.GetListSearch(p);


            if (!string.IsNullOrEmpty(p))
            {
                return View(valuesp.ToList());
            }
            else
            {
                return View(valuesa.ToList());
            }
            //var values=c.Contents.ToList();
            
        }
        public ActionResult ContentByHeading(int id)
        {
            var contentvalues = ctm.GetListByHeadingID(id);
            return View(contentvalues);
        }
    }
}