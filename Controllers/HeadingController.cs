using Acozum_webAppMVC.Models;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;

namespace Acozum_webAppMVC.Controllers
{
    public class HeadingController : Controller
    {
        // GET: Heading
        HeadingManager hm = new HeadingManager(new EfHeadingDal());
        CategoryManager cm = new CategoryManager(new EfCategoryDal());
        WriterManager wm = new WriterManager(new EfWriterDal());
        public ActionResult Index()
        {
            var headingvalues = hm.GetList();
            return View(headingvalues);
        }

        public ActionResult HeadingReport()
        {
            var headingvalues = hm.GetList();
            return View(headingvalues);
        }

        [HttpGet]
        public ActionResult AddHeading()
        {
            List<SelectListItem> valuecategory = (from x in cm.GetList()
                                                  select new SelectListItem
                                                  {
                                                      Text = x.CategoryName,
                                                      Value = x.CategoryID.ToString(),
                                                  }).ToList();

            List<SelectListItem> valuewriter = (from x in wm.GetList()
                                                select new SelectListItem
                                                {
                                                    Text = x.WriterName + " " + x.WriterSurName,
                                                    Value = x.WriterID.ToString(),
                                                }).ToList();
            ViewBag.vlw = valuewriter;
            ViewBag.vlc = valuecategory;
            return View();
        }

        [HttpPost]
        public ActionResult AddHeading(Heading p)
        {
            p.HeadingDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            hm.HeadingAdd(p);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditHeading(int id)
        {
            List<SelectListItem> valuecategory = (from x in cm.GetList()
                                                  select new SelectListItem
                                                  {
                                                      Text = x.CategoryName,
                                                      Value = x.CategoryID.ToString(),
                                                  }).ToList();
            ViewBag.vlc = valuecategory;
            var headingvalue = hm.GetByID(id);
            return View(headingvalue);
        }

        [HttpPost]
        public ActionResult EditHeading(Heading p)
        {
            hm.HeadingUpdate(p);
            return RedirectToAction("Index");
        }

        public ActionResult DeleteHeading(int id)
        {
            var headingvalue = hm.GetByID(id);
            if (headingvalue.HeadingStatus == true)
            {
                headingvalue.HeadingStatus = false;
            }
            else if (headingvalue.HeadingStatus == false)
            {
                headingvalue.HeadingStatus = true;
            }
            hm.HeadingDelete(headingvalue);
            return RedirectToAction("Index");

        }
        public ActionResult CalendarIndex()
        {
            return View();
        }
        public JsonResult CalendarHeading()
        {
            var Events = CalenderList();
            var events = Events.ToList();
            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public List<CalenderClass> CalenderList()
        {
            var calendervalues = hm.GetList();
            List<CalenderClass> cC = new List<CalenderClass>();
            string eT = "Green";
            foreach (var e in calendervalues)
            {
                switch (e.Category.CategoryName)
                {
                    case "Tiyatro":
                        eT = "Green";
                        break;
                    case "Eğitim":
                        eT = "Blue";
                        break;
                    case "Yazılım":
                        eT = "Red";
                        break;
                    case "Kitap":
                        eT = "Yellow";
                        break;
                    case "Spor":
                        eT = "Orange";
                        break;
                    case "Film":
                        eT = "Pink";
                        break;
                    case "Dizi":
                        eT = "Gray";
                        break;
                    case "Sosyal Medya":
                        eT = "Purple";
                        break;
                    case "Seyahat":
                        eT = "Cyan";
                        break;
                }
                int eID = e.HeadingID;
                string eS = e.HeadingName;
                string eD = e.Category.CategoryName;
                DateTime eSt = DateTime.Parse(e.HeadingDate.ToString());
                DateTime eEnd = eSt.AddHours(1);
                string etc = eT;
                bool eisfull = false;

                cC.Add(new CalenderClass()
                {
                    EventID = eID,
                    Subject = eS,
                    Description = eD,
                    Start = eSt,
                    End = eEnd,
                    ThemeColor = eT,
                    IsFullDay = eisfull,
                });
            }

            return cC;
        }
    }
}