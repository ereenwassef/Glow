using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Glow.Models;

namespace Glow.Controllers
{
    public class AccountsController : Controller
    {
        private glowEntities db = new glowEntities();

        // GET: Accounts
        public ActionResult Index()
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                var accounts = db.Accounts.Include(a => a.Role);
            return View(accounts.ToList());
            }
            return RedirectToAction("Login", "Accounts");
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Home()
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Accounts");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginAccount(Account u, FormCollection fc)
        {            // this action is for handle post (login)
            if (ModelState.IsValid) // this is check validity
            {

                var v = db.Accounts.Where(a => a.Acc_Email.Equals(u.Acc_Email) && a.Acc_Password.Equals(u.Acc_Password)).FirstOrDefault();
                if (v != null)
                {
                    Session["LogedUserName"] = v.Acc_Email.ToString();
                    Session["RoleID"] = v.Role.Role_Name.ToString();
                    return RedirectToAction("Home");
                }
                else
                {
                    return RedirectToAction("login");
                }


            }
            return RedirectToAction("login");
        }

        [AllowAnonymous]
        public ActionResult SignOut()
        {
            Response.AddHeader("Cache-Control", "no-cache, no-store,must-revalidate");
            Response.AddHeader("Pragma", "no-cache");
            Response.AddHeader("Expires", "0");
            Session.Abandon();

            Session.Clear();
            Response.Cookies.Clear();
            Session.RemoveAll();


            Session["LogedUserName"] = null;
            Session["RoleID"] = null;
            return RedirectToAction("login", "Accounts");
        }


        [AllowAnonymous]
        public ActionResult SignOutUser()
        {
            Response.AddHeader("Cache-Control", "no-cache, no-store,must-revalidate");
            Response.AddHeader("Pragma", "no-cache");
            Response.AddHeader("Expires", "0");
            Session.Abandon();

            Session.Clear();
            Response.Cookies.Clear();
            Session.RemoveAll();


            Session["MembershipNo"] = null;
           
            return RedirectToAction("Index", "Home");
        }

        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {

                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
            }
            return RedirectToAction("Login", "Accounts");
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                ViewBag.Acc_Role = new SelectList(db.Roles, "id", "Role_Name");
            return View();
            }
            return RedirectToAction("Login", "Accounts");
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Acc_Name,Acc_Email,Acc_Password,Acc_Role,Acc_Mobile")] Account account)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {

                if (ModelState.IsValid)
            {
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Acc_Role = new SelectList(db.Roles, "id", "Role_Name", account.Acc_Role);
            return View(account);
            }
            return RedirectToAction("Login", "Accounts");
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            ViewBag.Acc_Role = new SelectList(db.Roles, "id", "Role_Name", account.Acc_Role);
            return View(account);
            }
            return RedirectToAction("Login", "Accounts");
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Acc_Name,Acc_Email,Acc_Password,Acc_Role,Acc_Mobile")] Account account)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Acc_Role = new SelectList(db.Roles, "id", "Role_Name", account.Acc_Role);
            return View(account);
            }
            return RedirectToAction("Login", "Accounts");
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
            }
            return RedirectToAction("Login", "Accounts");
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Accounts");
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
