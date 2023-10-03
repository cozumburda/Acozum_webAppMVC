using Acozum_webAppMVC.Hash;
using Acozum_webAppMVC.Models;
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
    public class AdminController : Controller
    {
        // GET: Admin
        AdminManager adm = new AdminManager(new EfAdminDal());
        AdminValidator adminvalidator = new AdminValidator();
        Cryptograph crypvalue = new Cryptograph();


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
            List<RoleClass> rc = new List<RoleClass>();
            rc.Add(new RoleClass()
            {
                RoleName = "A",
            });
            rc.Add(new RoleClass()
            {
                RoleName = "B",
            });
            rc.Add(new RoleClass()
            {
                RoleName = "C",
            });
            rc.Add(new RoleClass()
            {
                RoleName = "D",
            });
            List<SelectListItem> vl = (from x in rc
                                       select new SelectListItem
                                       {
                                           Text = x.RoleName
                                       }).ToList();
            ViewBag.lr = vl;
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
            List<RoleClass> rc = new List<RoleClass>();
            rc.Add(new RoleClass()
            {
                RoleName = "A",
            });
            rc.Add(new RoleClass()
            {
                RoleName = "B",
            });
            rc.Add(new RoleClass()
            {
                RoleName = "C",
            });
            rc.Add(new RoleClass()
            {
                RoleName = "D",
            });
            List<SelectListItem> vl = (from x in rc
                                       select new SelectListItem
                                       {
                                           Text = x.RoleName
                                       }).ToList();
            ViewBag.lr = vl;
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
                p.AdminStatus = true;
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
        public ActionResult DeleteAdmin(int id)
        {
            var adminval = adm.GetByID(id);
            if (adminval.AdminStatus == true)
            {
                adminval.AdminStatus = false;
            }
            else if (adminval.AdminStatus == false)
            {
                adminval.AdminStatus = true;
            }
            adm.AdminDelete(adminval);
            return RedirectToAction("ListAdmin");

        }
    }
}