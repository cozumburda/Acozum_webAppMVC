using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Acozum_webAppMVC.Controllers
{
    [AllowAnonymous]
    public class DefaultController : Controller
    {
        // GET: Default
        HeadingManager hm = new HeadingManager(new EfHeadingDal());
        ContentManager ctm = new ContentManager(new EfContentDal());
        public ActionResult Headings()
        {
            var headinlist = hm.GetList();
            return View(headinlist);
        }
        public PartialViewResult Index(int id = 0)
        {
            var contentlist = ctm.GetListByHeadingID(id);
            return PartialView(contentlist);
        }
    }
}