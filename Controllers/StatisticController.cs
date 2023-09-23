using Antlr.Runtime.Tree;
using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.Ajax.Utilities;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using WebGrease.Css.Extensions;

namespace Acozum_webAppMVC.Controllers
{
    public class StatisticController : Controller
    {
        // GET: Statistic
        CategoryManager cm = new CategoryManager(new EfCategoryDal());
        WriterManager wm = new WriterManager(new EfWriterDal());
        HeadingManager hm = new HeadingManager(new EfHeadingDal());
        public ActionResult Index()
        {
            var Categorysum = cm.GetList().Count.ToString();
            ViewBag.Categorysum = Categorysum;

            List<Category> s = cm.GetList();
            var sL = s.Where(x => x.CategoryStatus == true);
            var sL1 = s.Where(x => x.CategoryStatus == false);
            int sT = sL.Count();
            int sF = sL1.Count();
            var dF = (sT - sF).ToString();
            ViewBag.dF = dF;

            List<Writer> isim = wm.GetList();
            var WL = isim.Where(x => x.WriterName.Contains("A"));
            var WLA = WL.Select(x => x.WriterName).ToArray();
            var WNS = "";
            for (int i = 0; i < WLA.Length; i++)
            {
                var WNV = WLA[i];
                WNS += ("-" + WNV);

                ViewBag.WNA = WNS.Substring(1);
            }

            List<Heading> kt = hm.GetList();
            var Kg = kt.GroupBy(x => x.CategoryID).OrderByDescending(x => x.Count());
            var Km = Kg.First().Key;
            var Ka = cm.GetByID(Km).CategoryName.ToString();
            var Ky = Kg.SingleOrDefault(x => x.Key == 19).Count().ToString();
            ViewBag.Ka = Ka;
            ViewBag.Ky = Ky;
            return View();
        }
    }
}