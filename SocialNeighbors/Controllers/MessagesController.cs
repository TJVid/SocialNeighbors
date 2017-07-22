using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SocialNeighbors.Models;

namespace SocialNeighbors.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Messages
        public ActionResult Index()
        {
            var userMessages = db.Messages
                .GroupBy(m => m.fromUser)
                .SelectMany(g => g.OrderByDescending(x => x.timeStamp).Take(1))
                .Where(x => (x.fromUser.ToUpper() == User.Identity.Name.ToUpper() || x.toUser.ToUpper() == User.Identity.Name.ToUpper())).ToList()
                .Take(10);


            var userSentMessages = db.Messages
                .GroupBy(m => m.toUser)
                .SelectMany(g => g.OrderByDescending(x => x.timeStamp).Take(1))
                .Where(x => (x.fromUser.ToUpper() == User.Identity.Name.ToUpper() || x.toUser.ToUpper() == User.Identity.Name.ToUpper())).ToList()
                       .Take(10);

            var sortedMessages = userMessages.Union(userSentMessages)
                .GroupBy(m => (m.toUser + m.fromUser).Replace(User.Identity.Name, ""))
                .SelectMany(g => g.OrderByDescending(x => x.timeStamp).Take(1))
                .OrderByDescending(m => m.timeStamp);
            //userMessages.Union(userSentMessages).OrderByDescending(m => (m.toUser + m.fromUser).Replace(User.Identity.Name, ""));

            return View(sortedMessages);
        }


        // GET: Messages
        public ActionResult MessagesPerPerson_messageID(int Id)
        {
            MessagingViewModel mvm = new MessagingViewModel();

            var me = User.Identity.Name.ToUpper();
            var person = db.Messages.Where(m => m.Id == Id).Select(m => (m.fromUser + m.toUser).Replace(User.Identity.Name, "")).FirstOrDefault().ToUpper();

            person = person.Trim();
            person = person.ToUpper();

            mvm.me = db.Users.FirstOrDefault(u => u.Email.ToUpper() == me);
            mvm.neighbor = db.Users.FirstOrDefault(u => u.Email.ToUpper() == person);

            mvm.messages = db.Messages
                .Where(m => (m.fromUser.ToUpper() == me || m.toUser.ToUpper() == me) && (m.fromUser.ToUpper().Trim() == person || m.toUser.ToUpper().Trim() == person))
                .OrderByDescending(m => m.timeStamp)
                .Take(10).ToList();


            return View("MessagesPerPerson", mvm);
        }


        [HttpGet]
        public ActionResult MessagesPerPerson_ajax(string neighborEmailId)
        {
            MessagingViewModel mvm = new MessagingViewModel();

            var me = User.Identity.Name;

            mvm.me = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            mvm.neighbor = db.Users.FirstOrDefault(u => u.Email.ToUpper().Trim() == neighborEmailId.ToUpper().Trim());

            mvm.messages = db.Messages
                .Where(m => (m.fromUser.ToUpper().Trim() == me.ToUpper().Trim()
                || m.toUser.ToUpper().Trim() == me.ToUpper().Trim()) && ((m.fromUser + m.toUser).Replace(me, "").ToUpper().Trim() == neighborEmailId.ToUpper()))
                .OrderByDescending(m => m.timeStamp)
                .Take(10).ToList();

            return Json(mvm, JsonRequestBehavior.AllowGet);
        }


        // GET: Messages
        public ActionResult MessagesPerPerson(string UserId)
        {
            MessagingViewModel mvm = new MessagingViewModel();

            var me = User.Identity.Name;

            mvm.me = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            mvm.neighbor = db.Users.FirstOrDefault(u => u.Id.ToUpper().Trim() == UserId.ToUpper().Trim());

            mvm.messages = db.Messages
                .Where(m => (m.fromUser.ToUpper().Trim() == me.ToUpper().Trim()
                || m.toUser.ToUpper().Trim() == me.ToUpper().Trim()) && ((m.fromUser + m.toUser).Replace(me, "").ToUpper().Trim() == UserId.ToUpper()))
                .OrderByDescending(m => m.timeStamp)
                .Take(10).ToList();


            return View(mvm);
        }


        // GET: Messages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // GET: Messages/Create
        public ActionResult Create()
        {
            Message message = new Message();
            message.fromUser = User.Identity.Name;
            message.timeStamp = DateTime.Now;
            return View(message);
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,fromUser,toUser,messageContent,timeStamp,isRead")] Message message)
        {
            if (ModelState.IsValid)
            {
                message.fromUser = User.Identity.Name;
                message.timeStamp = DateTime.Now;

                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(message);
        }


        public ActionResult Create_Ajax([Bind(Include = "toUser,messageContent")] Message message)
        {
            if (ModelState.IsValid)
            {
                message.fromUser = User.Identity.Name;
                message.timeStamp = DateTime.Now;
                message.isRead = false;

                db.Messages.Add(message);
                db.SaveChanges();
                return Json(message);
            }

            return View(message);
        }

        // GET: Messages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,fromUser,toUser,messageContent,timeStamp,isRead")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(message);
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
