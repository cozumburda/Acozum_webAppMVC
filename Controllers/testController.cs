using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Acozum_webAppMVC.Controllers
{
    public class testController : Controller
    {
        // GET: test
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Test2()
        {
            return View();
        }
        public ActionResult ToDoList()
        {
            return View();
        }

        public ActionResult SweetAlert()
        {
            return View();
        }
    }
}