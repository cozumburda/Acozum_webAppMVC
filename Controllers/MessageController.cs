using Acozum_webAppMVC.Hash;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Abstract;
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
    public class MessageController : Controller
    {
        // GET: Message
        MessageManager mm = new MessageManager(new EfMessageDal());
        MessageValidator messagevalidator = new MessageValidator();
        Cryptograph crypvalue = new Cryptograph();
        [Authorize]
        public ActionResult Inbox()
        {
            string p = (string)Session["AdminUserName"];
            var messageinlist = mm.GetListInbox(p);
            foreach (var item in messageinlist)
            {
                item.SenderMail = crypvalue.Decrypt(item.SenderMail);
            }
            return View(messageinlist);
        }

        public ActionResult Sendbox()
        {
            string p = (string)Session["AdminUserName"];
            var messagesendlist = mm.GetListSendbox(p);
            foreach (var item in messagesendlist)
            {
                item.ReceiverMail = crypvalue.Decrypt(item.ReceiverMail);
            }
            return View(messagesendlist);
        }

        [HttpGet]
        public ActionResult NewMessage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewMessage(Message p, string send, string draft)
        {
            string sender = (string)Session["AdminUserName"];
            ValidationResult result = messagevalidator.Validate(p);
            if (result.IsValid)
            {
                if (send != null)
                {
                    p.MessageDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                    p.SenderMail = sender;
                    p.MessageValueStatusSender = "A";
                    p.MessageValueStatusReceiver = "A";
                    p.ReceiverMail = crypvalue.Encrypt(p.ReceiverMail);
                    mm.MessageAdd(p);
                    return RedirectToAction("Sendbox");
                }
                else if (draft != null)
                {
                    p.MessageDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                    p.SenderMail = sender;
                    p.MessageValueStatusSender = "D";
                    p.MessageValueStatusReceiver = "0";
                    p.ReceiverMail = crypvalue.Encrypt(p.ReceiverMail);
                    mm.MessageAdd(p);
                    return RedirectToAction("Draftbox");
                }
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
        public ActionResult Draftbox()
        {
            string p = (string)Session["AdminUserName"];
            var messagedraftlist = mm.GetListDraftbox(p);
            foreach (var item in messagedraftlist)
            {
                item.ReceiverMail = crypvalue.Decrypt(item.ReceiverMail);
            }
            return View(messagedraftlist);
        }
        public ActionResult Spambox()
        {
            string p = (string)Session["AdminUserName"];
            var messagespamlist = mm.GetListSpambox(p);
            foreach (var item in messagespamlist)
            {
                item.SenderMail = crypvalue.Decrypt(item.SenderMail);
            }
            return View(messagespamlist);
        }
        public ActionResult Trashbox()
        {
            string p = (string)Session["AdminUserName"];
            var messagetrashlist = mm.GetListTrashbox(p);
            foreach (var item in messagetrashlist)
            {
                item.SenderMail = crypvalue.Decrypt(item.SenderMail);
                item.ReceiverMail = crypvalue.Decrypt(item.ReceiverMail);
            }
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
            invalues.SenderMail = crypvalue.Decrypt(invalues.SenderMail);
            return View(invalues);
        }
        public ActionResult GetSendboxMessageDetails(int id)
        {
            var snvalues = mm.GetByID(id);
            var rawmessage = snvalues.MessageContent;
            ViewBag.rwm = rawmessage;
            snvalues.ReceiverMail = crypvalue.Decrypt(snvalues.ReceiverMail);
            return View(snvalues);
        }
        public ActionResult GetDraftboxMessageDetails(int id)
        {
            var dnvalues = mm.GetByID(id);
            var rawmessage = dnvalues.MessageContent;
            ViewBag.rwmd = rawmessage;
            dnvalues.ReceiverMail = crypvalue.Decrypt(dnvalues.ReceiverMail);
            return View(dnvalues);
        }
        public ActionResult GetSpamboxMessageDetails(int id)
        {
            var spnvalues = mm.GetByID(id);
            spnvalues.SenderMail = crypvalue.Decrypt(spnvalues.SenderMail);
            return View(spnvalues);
        }
        public ActionResult GetTrashboxMessageDetails(int id)
        {
            var tnvalues = mm.GetByID(id);
            tnvalues.SenderMail = crypvalue.Decrypt(tnvalues.SenderMail);
            tnvalues.ReceiverMail = crypvalue.Decrypt(tnvalues.ReceiverMail);
            return View(tnvalues);
        }
        public ActionResult DeleteMessageInbox(int id)
        {
            var messageval = mm.GetByID(id);
            if (messageval.MessageValueStatusReceiver == "A" || messageval.MessageValueStatusReceiver == "S")
            {
                messageval.MessageValueStatusReceiver = "T";
            }
            else if (messageval.MessageValueStatusReceiver == "T")
            {
                messageval.MessageValueStatusReceiver = "A";
            }
            mm.MessageUpdate(messageval);
            return RedirectToAction("Inbox");
        }
        public ActionResult DeleteMessageSendbox(int id)
        {
            var messagevals = mm.GetByID(id);
            if (messagevals.MessageValueStatusSender == "A" || messagevals.MessageValueStatusSender == "D")
            {
                messagevals.MessageValueStatusSender = "T";
            }
            else if (messagevals.MessageValueStatusSender == "T")
            {
                messagevals.MessageValueStatusSender = "A";
            }
            mm.MessageUpdate(messagevals);
            return RedirectToAction("Sendbox");
        }
        public ActionResult DeleteMessage(int id)
        {
            var messagevals = mm.GetByID(id);
            mm.MessageDelete(messagevals);
            return RedirectToAction("Trashbox");
        }
        public ActionResult SingSpamMessage(int id)
        {
            var messagevalsp = mm.GetByID(id);
            if (messagevalsp.MessageValueStatusReceiver == "A")
            {
                messagevalsp.MessageValueStatusReceiver = "S";
            }
            else if (messagevalsp.MessageValueStatusReceiver == "S")
            {
                messagevalsp.MessageValueStatusReceiver = "A";
            }
            mm.MessageUpdate(messagevalsp);
            return RedirectToAction("Inbox");
        }
        public ActionResult DraftToSend(int id)
        {
            var messagevald = mm.GetByID(id);
            messagevald.MessageValueStatusReceiver = "A";
            messagevald.MessageValueStatusSender = "A";
            mm.MessageUpdate(messagevald);
            return RedirectToAction("Sendbox");
        }

        [HttpGet]
        public ActionResult DraftMessageEdit(int id)
        {
            var messagevald = mm.GetByID(id);
            var rawmessage = messagevald.MessageContent;
            ViewBag.rwmd = rawmessage;
            messagevald.ReceiverMail = crypvalue.Decrypt(messagevald.ReceiverMail);
            return View(messagevald);
        }
        [HttpPost]
        public ActionResult DraftMessageEdit(Message p, string send, string draft)
        {
            string sender = (string)Session["AdminUserName"];
            ValidationResult result = messagevalidator.Validate(p);
            if (result.IsValid)
            {
                if (send != null)
                {
                    p.MessageDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                    p.SenderMail = sender;
                    p.MessageValueStatusSender = "A";
                    p.MessageValueStatusReceiver = "A";
                    p.ReceiverMail = crypvalue.Encrypt(p.ReceiverMail);
                    mm.MessageUpdate(p);
                    return RedirectToAction("Sendbox");
                }
                else if (draft != null)
                {
                    p.MessageDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                    p.SenderMail = sender;
                    p.MessageValueStatusSender = "D";
                    p.MessageValueStatusReceiver = "0";
                    p.ReceiverMail = crypvalue.Encrypt(p.ReceiverMail);
                    mm.MessageUpdate(p);
                    return RedirectToAction("Draftbox");
                }
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
    }
}