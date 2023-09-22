using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Acozum_webAppMVC.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        ContactManager ctm = new ContactManager(new EfContactDal());
        MessageManager mm = new MessageManager(new EfMessageDal());
        ContactValidator ctv = new ContactValidator();
        public ActionResult Index()
        {
            var contactvalues = ctm.GetList();
            return View(contactvalues);
        }
        public PartialViewResult PartialMenu()
        {
            var contactcount = ctm.GetList().Count.ToString();
            var sendcount = mm.GetListSendbox().Count.ToString();
            var inboxcount = mm.GetListInbox().Count.ToString();
            var draftcount = mm.GetListDraftbox().Count.ToString();
            var spamboxcount = mm.GetListSpambox().Count.ToString();
            var trashboxcount = mm.GetListTrashbox().Count.ToString();
            var contnotread = ctm.GetList().Where(x => x.ContactReadStatus == false).ToList();
            var contnotreadv = contnotread.Count.ToString();
            var inboxnotread = mm.GetListInbox().Where(x => x.MessageReadStatus == false).ToList();
            var inboxnotreadv = inboxnotread.Count.ToString();
            ViewBag.inrv = inboxnotreadv;
            ViewBag.cnrv = contnotreadv;
            ViewBag.ivc = inboxcount;
            ViewBag.svc = sendcount;
            ViewBag.cvc = contactcount;
            ViewBag.dvc = draftcount;
            ViewBag.spvc = spamboxcount;
            ViewBag.tvc = trashboxcount;
            return PartialView();
        }


        public ActionResult GetContactDetails(int id)
        {
            var contactvalues = ctm.GetByID(id);
            if (contactvalues.ContactReadStatus==false)
            {
                contactvalues.ContactReadStatus = true;
                ctm.ContactUpdate(contactvalues);
            }
            
            return View(contactvalues);
        }
    }
}