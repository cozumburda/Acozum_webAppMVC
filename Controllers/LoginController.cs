using Acozum_webAppMVC.Hash;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation;
using FluentValidation.Results;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Acozum_webAppMVC.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {        // GET: Login
        AdminManager adm = new AdminManager(new EfAdminDal());
        WriterManager wm = new WriterManager(new EfWriterDal());
        AdminValidator adminvalidator = new AdminValidator();
        WriterValidator writervalidator = new WriterValidator();
        Cryptograph crypvalue = new Cryptograph();


        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Admin p)
        {
            ValidationResult results = adminvalidator.Validate(p);

            p.AdminUserName = crypvalue.Encrypt(p.AdminUserName);
            p.AdminPassword = crypvalue.Encrypt(p.AdminPassword);
            var c = adm.GetList();
            var adminuserinfo = c.FirstOrDefault(x => x.AdminUserName == p.AdminUserName && x.AdminPassword == p.AdminPassword);
            if (adminuserinfo != null)
            {
                FormsAuthentication.SetAuthCookie(adminuserinfo.AdminUserName, false);
                Session["AdminUserName"] = adminuserinfo.AdminUserName;
                return RedirectToAction("Index", "AdminCategory");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                return RedirectToAction("Index");
            }
            //Context c = new Context();
            //var adminuserinfo = c.Admins.FirstOrDefault(x => x.AdminUserName == p.AdminUserName && x.AdminPassword == p.AdminPassword);
        }

        [Authorize(Roles = "B")]
        public ActionResult ListAdmin()
        {
            var adminusername = adm.GetList().Where(x => x.AdminUserName != null).ToList();
            foreach (var item in adminusername)
            {
                item.AdminUserName = crypvalue.Decrypt(item.AdminUserName);
            }
            var adminvalues = adm.GetList();
            return View(adminvalues);
        }

        [HttpGet]
        public ActionResult AddAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddAdmin(Admin p)
        {
            ValidationResult result = adminvalidator.Validate(p);
            if (result.IsValid)
            {
                p.AdminUserName = crypvalue.Encrypt(p.AdminUserName);
                p.AdminPassword = crypvalue.Encrypt(p.AdminPassword);
                adm.AdminAdd(p);
                return RedirectToAction("ListAdmin");
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
        public ActionResult EditAdmin(int id)
        {
            var adminvalue = adm.GetByID(id);
            adminvalue.AdminUserName = crypvalue.Decrypt(adminvalue.AdminUserName);
            adminvalue.AdminPassword = crypvalue.Decrypt(adminvalue.AdminPassword);
            return View(adminvalue);
        }
        [HttpPost]
        public ActionResult EditAdmin(Admin p)
        {
            ValidationResult result = adminvalidator.Validate(p);
            if (result.IsValid)
            {
                p.AdminUserName = crypvalue.Encrypt(p.AdminUserName);
                p.AdminPassword = crypvalue.Encrypt(p.AdminPassword);
                adm.AdminUpdate(p);
                return RedirectToAction("ListAdmin");
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
        public ActionResult WriterLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult WriterLogin(Writer p)
        {
            ValidationResult results = writervalidator.Validate(p);
            var response = Request["g - recaptcha - response"];
            string secretKey = "6Lc5VFUoAAAAAHakRfedaNGp_zSWjTLdPdhUuacO";
            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");
            ViewBag.Message = status ? "Google reCaptcha Doğrulama Başarılı" : "Google reCaptcha Doğrulama Başarısız";

            p.WriterMail = crypvalue.Encrypt(p.WriterMail);
            p.WriterPassword = crypvalue.Encrypt(p.WriterPassword);
            var c = wm.GetList();
            var writeruserinfo = c.FirstOrDefault(x => x.WriterMail == p.WriterMail && x.WriterPassword == p.WriterPassword);
            if (writeruserinfo != null)
            {
                FormsAuthentication.SetAuthCookie(writeruserinfo.WriterMail, false);
                Session["WriterMail"] = writeruserinfo.WriterMail;
                return RedirectToAction("MyContent", "WriterPanelContent");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                return RedirectToAction("WriterLogin");
            }
            //Context c = new Context();
            //var adminuserinfo = c.Admins.FirstOrDefault(x => x.AdminUserName == p.AdminUserName && x.AdminPassword == p.AdminPassword);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Headings", "Default");
        }
    }
}