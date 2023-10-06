using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Acozum_webAppMVC.Controllers
{
    public class SkillController : Controller
    {
        // GET: Skill
        SkillManager sm = new SkillManager(new EfSkillDal());
        WriterManager wm = new WriterManager(new EfWriterDal());
        public ActionResult Index()
        {
            var skillvalues = (sm.GetList().Where(x=> x.SkillStatus==true)).ToList();
            return View(skillvalues);
        }
        [HttpGet]
        public ActionResult AddSkill()
        {
            List<SelectListItem> valuewriter = (from x in wm.GetList()
                                                select new SelectListItem
                                                {
                                                    Text = x.WriterName + " " + x.WriterSurName,
                                                    Value = x.WriterID.ToString(),
                                                }).ToList();
            ViewBag.vlw = valuewriter;
            return View();
        }
        [HttpPost]
        public ActionResult AddSkill(Skill p)
        {
            p.SkillStatus = true;
            sm.SkillAdd(p);
            return RedirectToAction("SkillList");
        }
        public ActionResult DeleteSkill(int id)
        {
            var skillvalue = sm.GetByID(id);
            if (skillvalue.SkillStatus == true)
            {
                skillvalue.SkillStatus = false;
            }
            else if (skillvalue.SkillStatus == false)
            {
                skillvalue.SkillStatus = true;
            }
            sm.SkillDelete(skillvalue);
            return RedirectToAction("SkillList");
        }

        [HttpGet]
        public ActionResult EditSkill(int id)
        {
            var skillvalue = sm.GetByID(id);
            return View(skillvalue);
        }

        [HttpPost]
        public ActionResult EditSkill(Skill p)
        {
            p.SkillStatus = true;
            sm.SkillUpdate(p);
            return RedirectToAction("SkillList");
        }
        public ActionResult SkillList()
        {
            var skillvalues = sm.GetList();
            return View(skillvalues);
        }
    }
}