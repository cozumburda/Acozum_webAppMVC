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
    public class WriterPanelMessageController : Controller
    {
        // GET: WriterPanelMessage
        MessageManager mm = new MessageManager(new EfMessageDal());
        MessageValidator messagevalidator = new MessageValidator();
        ContactManager ctm = new ContactManager(new EfContactDal());
        public ActionResult Inbox()
        {
            var messageinlist = mm.GetListInbox();
            return View(messageinlist);
        }
        [HttpGet]
        public ActionResult NewMessage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewMessage(Message p)
        {

            ValidationResult result = messagevalidator.Validate(p);
            if (result.IsValid)
            {
                p.MessageDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                p.SenderMail = "gizem@gmail.com";
                mm.MessageAdd(p);
                return RedirectToAction("Sendbox");
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
        public ActionResult Sendbox()
        {
            var messagesendlist = mm.GetListSendbox();
            return View(messagesendlist);
        }
        public ActionResult Draftbox()
        {
            var messagedraftlist = mm.GetListDraftbox();
            return View(messagedraftlist);
        }
        public ActionResult Spambox()
        {
            var messagespamlist = mm.GetListSpambox();
            return View(messagespamlist);
        }
        public ActionResult Trashbox()
        {
            var messagetrashlist = mm.GetListTrashbox();
            return View(messagetrashlist);
        }
        public ActionResult GetInboxMessageDetails(int id)
        {
            var invalues = mm.GetByID(id);
            if (invalues.MessageReadStatus == false)
            {
                invalues.MessageReadStatus = true;
                mm.MessageUpdate(invalues);
            }
            var rawmessagei = invalues.MessageContent;
            ViewBag.rwmi = rawmessagei;
            return View(invalues);
        }
        public ActionResult GetSendboxMessageDetails(int id)
        {
            var snvalues = mm.GetByID(id);
            var rawmessage = snvalues.MessageContent;
            ViewBag.rwm = rawmessage;
            return View(snvalues);
        }
        public ActionResult GetDraftboxMessageDetails(int id)
        {
            var dnvalues = mm.GetByID(id);
            return View(dnvalues);
        }
        public ActionResult GetSpamboxMessageDetails(int id)
        {
            var spnvalues = mm.GetByID(id);
            return View(spnvalues);
        }
        public ActionResult GetTrashboxMessageDetails(int id)
        {
            var tnvalues = mm.GetByID(id);
            return View(tnvalues);
        }

    }
}