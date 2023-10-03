using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Acozum_webAppMVC.Controllers
{
    public class WriterPanelContentController : Controller
    {
        // GET: WriterPanelContent
        ContentManager ctm = new ContentManager(new EfContentDal());
        WriterManager wm = new WriterManager(new EfWriterDal());
        public ActionResult MyContent(string p)
        {
            var c = wm.GetList();
            p = (string)Session["WriterMail"];
            var writeridinfo = c.Where(x => x.WriterMail == p).Select(y => y.WriterID).FirstOrDefault();
            //ViewBag.d = p;
            var contentvaluesw = ctm.GetListByWriter(writeridinfo);
            return View(contentvaluesw);
        }
        public ActionResult ContentbyHeading(int id)
        {
            var contentvaluesw = ctm.GetListByHeadingID(id);
            return View(contentvaluesw);
        }

        [HttpGet]
        public ActionResult AddContent(int id)
        {
            ViewBag.d = id;
            return View();
        }

        [HttpPost]
        public ActionResult AddContent(Content p)
        {
            var c = wm.GetList();
            var writerinfo = (string)Session["WriterMail"];
            var writeridinfo = c.Where(x => x.WriterMail == writerinfo).Select(y => y.WriterID).FirstOrDefault();
            p.ContentDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            p.WriterID = writeridinfo;
            p.ContentStatus = true;
            ctm.ContentAdd(p);
            return RedirectToAction("MyContent");
        }
        public ActionResult ToDoList()
        {
            return View();
        }
    }
}