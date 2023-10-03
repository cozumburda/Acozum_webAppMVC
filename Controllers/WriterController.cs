using Acozum_webAppMVC.Hash;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Acozum_webAppMVC.Controllers
{
    public class WriterController : Controller
    {
        // GET: Writer
        WriterManager wm = new WriterManager(new EfWriterDal());
        WriterValidator writervalidator = new WriterValidator();
        HeadingManager hm = new HeadingManager(new EfHeadingDal());
        Cryptograph crypvalue = new Cryptograph();

        [Authorize]
        public ActionResult Index()
        {
            var mail = wm.GetList().Where(x => x.WriterMail != null).ToList();
            foreach (var item in mail)
            {
                item.WriterMail = crypvalue.Decrypt(item.WriterMail);
            }
            var writervalues = wm.GetList();
            return View(writervalues);
        }

        [HttpGet]
        public ActionResult AddWriter()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddWriter(Writer p)
        {
            ValidationResult result = writervalidator.Validate(p);
            if (result.IsValid)
            {
                p.WriterMail = crypvalue.Encrypt(p.WriterMail);
                p.WriterPassword = crypvalue.Encrypt(p.WriterPassword);
                wm.WriterAdd(p);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult EditWriter(int id)
        {
            var writervalue = wm.GetByID(id);
            writervalue.WriterMail = crypvalue.Decrypt(writervalue.WriterMail);
            writervalue.WriterPassword = crypvalue.Decrypt(writervalue.WriterPassword);
            return View(writervalue);
        }
        [HttpPost]
        public ActionResult EditWriter(Writer p)
        {
            ValidationResult result = writervalidator.Validate(p);
            if (result.IsValid)
            {
                p.WriterMail = crypvalue.Encrypt(p.WriterMail);
                p.WriterPassword = crypvalue.Encrypt(p.WriterPassword);
                wm.WriterUpdate(p);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();
        }
        public ActionResult HeadingsWriter(int id)
        {
            var headingvalues = hm.GetListByWriter(id);
            return View(headingvalues);
        }
        public ActionResult DeleteWriter(int id)
        {
            var writerval = wm.GetByID(id);
            if (writerval.WriterStatus == true)
            {
                writerval.WriterStatus = false;
            }
            else if (writerval.WriterStatus == false)
            {
                writerval.WriterStatus = true;
            }
            wm.WriterDelete(writerval);
            return RedirectToAction("Index");
        }
    }
}