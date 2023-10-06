using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Acozum_webAppMVC.Controllers
{
    public class GalleryController : Controller
    {
        // GET: Gallery
        ImageFileManager im = new ImageFileManager(new EfImageFileDal());
        public ActionResult Index()
        {
            var imagevalues = im.GetList();
            return View(imagevalues);
        }

        [HttpPost]
        public ActionResult AddImageFile(HttpPostedFileBase file, ImageFile p)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/AdminLTE-3.0.4/images"), _FileName);
                    file.SaveAs(_path);
                    TempData["Message"] = "Başarılı bir şekilde yüklendi!!";
                    p.ImagePath = "/AdminLTE-3.0.4/images/" + _FileName;
                    p.ImageName = _FileName;
                    im.ImageFileAdd(p);

                }
                catch (Exception ex)
                {
                    TempData["Message2"] = ex.Message.ToString();
                }
            else
            {
                TempData["Message1"] = "Dosya Seçilmedi!!";

            }
            return RedirectToAction("Index");
        }
        public PartialViewResult GaleryPartial()
        {
            return PartialView();
        }
    }
}