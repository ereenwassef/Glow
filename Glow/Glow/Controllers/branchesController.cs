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
    public class branchesController : Controller
    {
        private glowEntities db = new glowEntities();

        // GET: branches
        public ActionResult Index()
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                return View(db.branches.ToList());
            }
            return RedirectToAction("Login", "Accounts");
        }

        public ActionResult Branches()
        {
            return View(db.branches.ToList());
        }

        public ActionResult Login()
        {
          
            return RedirectToAction("Login","Accounts");
        }
        public ActionResult contacts()
        {
            //  var Contact = db.contacts.FirstOrDefault();
            ViewBag.Con_phone = db.contacts.Select(a => a.Con_Phone).FirstOrDefault();
            return PartialView("_Contacts", ViewBag.Con_phone);
        }

        public ActionResult contacts_Email()
        {
            //  var Contact = db.contacts.FirstOrDefault();
            ViewBag.Con_Email = db.contacts.Select(a => a.Con_Email).FirstOrDefault();
            return PartialView("_Contacts_Email", ViewBag.Con_Email);
        }



        public ActionResult Branchess()
        {
            //  var Contact = db.contacts.FirstOrDefault();
            var item = db.branches.ToList();
       /*     ViewBag.Branch_name = item.Branch_name;
            ViewBag.Branch_nameAr = item.Branch_nameAr;
            ViewBag.Branch_location = item.Branch_location;

            ViewBag.Branch_phone1 = item.Branch_phone1;
            ViewBag.Branch_phone2 = item.Branch_phone2;
            ViewBag.Branch_phone3 = item.Branch_phone3;
            ViewBag.Branch_phone4 = item.Branch_phone4;
            ViewBag.Branch_Whatsapp = item.Branch_Whatsapp;

    */
            return PartialView("_Branches", item);
        }






        // GET: branches/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
               
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            branch branch = db.branches.Find(id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            return View(branch);
            }
            return RedirectToAction("Login", "Accounts");
        }

        // GET: branches/Create
        public ActionResult Create()
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Accounts");
        }

        // POST: branches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Branch_id,Branch_name,Branch_nameAr,Branch_location,Branch_phone1,Branch_phone2,Branch_phone3,Branch_phone4,Branch_Whatsapp,Branch_Email")] branch branch)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {

                if (ModelState.IsValid)
            {
                db.branches.Add(branch);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(branch);
            }
            return RedirectToAction("Login", "Accounts");
        }

        // GET: branches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {

                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            branch branch = db.branches.Find(id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            return View(branch);
            }
            return RedirectToAction("Login", "Accounts");
        }

        // POST: branches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Branch_id,Branch_name,Branch_location,Branch_nameAr,Branch_phone1,Branch_phone2,Branch_phone3,Branch_phone4,Branch_Whatsapp,Branch_Email")] branch branch)
        {

         if (Session["LogedUserName"] != null && Session["RoleID"] != null)
          {
                if (ModelState.IsValid)
            {
                db.Entry(branch).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(branch);
         }
            return RedirectToAction("Login", "Accounts");
        }

        // GET: branches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            branch branch = db.branches.Find(id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            return View(branch);
            }
            return RedirectToAction("Login", "Accounts");
        }

        // POST: branches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                branch branch = db.branches.Find(id);
            db.branches.Remove(branch);
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
