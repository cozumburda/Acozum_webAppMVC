using Acozum_webAppMVC.Models;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Acozum_webAppMVC.Controllers
{
    public class ChartController : Controller
    {
        // GET: Chart
        HeadingManager hm = new HeadingManager(new EfHeadingDal());
        ContentManager ctm = new ContentManager(new EfContentDal());
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CategoryChart()
        {
            return Json(BlogList(), JsonRequestBehavior.AllowGet);
        }

        public List<CategoryClass> BlogList()
        {
            var cv = hm.GetCategoryList().GroupBy(x => x.Category.CategoryName).ToList();
            List<CategoryClass> ct = new List<CategoryClass>();
            foreach (var c in cv)
            {
                var CN = c.Select(x => x.Category.CategoryName);
                int CC = CN.Count();
                ViewBag.cn = CN;
                ViewBag.cc = CC;
                string Cnn = CN.FirstOrDefault().ToString();

                ct.Add(new CategoryClass()
                {
                    CategoryName = Cnn,
                    CategoryCount = CC

                });
            }
            return ct;
            //ct.Add(new CategoryClass()
            //{
            //    CategoryName = "Seyahat",
            //    CategoryCount = 4
            //});
            //ct.Add(new CategoryClass()
            //{
            //    CategoryName = "Teknoloji",
            //    CategoryCount = 7

            //});
            //ct.Add(new CategoryClass()
            //{
            //    CategoryName = "Spor",
            //    CategoryCount = 1
            //});
            //return ct;
        }
        public ActionResult HeadingIndex()
        {
            return View();
        }

        public ActionResult HeadingChart()
        {
            return Json(HeadingList(), JsonRequestBehavior.AllowGet);
        }

        public List<HeadingClass> HeadingList()
        {
            var cvh = ctm.GetListHeading().GroupBy(x => x.Heading.HeadingName).ToList();
            List<HeadingClass> ch = new List<HeadingClass>();
            foreach (var h in cvh)
            {
                var HN = h.Select(x => x.Heading.HeadingName);
                int HC = HN.Count();
                ViewBag.hn = HN;
                ViewBag.hc = HC;
                string Hnn = HN.FirstOrDefault().ToString();

                ch.Add(new HeadingClass()
                {
                    HeadingName = Hnn,
                    HeadingCount = HC,
                });

            }
            return ch;
        }
        public ActionResult WriterIndex()
        {
            return View();
        }

        public ActionResult WriterChart()
        {
            return Json(WriterList(), JsonRequestBehavior.AllowGet);
        }

        public List<WriterClass> WriterList()
        {
            var cvw = ctm.GetListWriter().GroupBy(x => x.Writer.WriterID).ToList();
            List<WriterClass> cw = new List<WriterClass>();
            foreach (var w in cvw)
            {
                var WN = w.Select(x => x.Writer.WriterName + " " + x.Writer.WriterSurName);
                int WC = WN.Count();
                ViewBag.hn = WN;
                ViewBag.hc = WC;
                string Wnn = WN.FirstOrDefault().ToString();

                cw.Add(new WriterClass()
                {
                    WriterName = Wnn,
                    WriterCount = WC

                });

            }
            return cw;
        }
    }
}