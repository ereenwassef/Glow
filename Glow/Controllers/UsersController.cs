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
    public class UsersController : Controller
    {
        private glowEntities db = new glowEntities();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }


        public ActionResult Account(int? id)
        {
            id = Convert.ToInt32(Session["MembershipNo"]);
            /*   id = Convert.ToInt32(Session["MembershipNo"]);
                  if (id == null)
                  {
                      return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                  }
                  User user = db.Users.Find(id);
                  if (user == null)
                  {
                      return HttpNotFound();
                  }
                 */
            return View()       ;
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
        public ActionResult info(int? id)
        {
            id = Convert.ToInt32(Session["MembershipNo"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }


            return PartialView("_info", user);
        }

        public ActionResult MembershipInfo(int? id)
        {
            id = Convert.ToInt32(Session["MembershipNo"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Info info = db.Infoes.Where(x=>x.Inf_MembershipNo==id.Value).FirstOrDefault();
            if (info == null)
            {
                return HttpNotFound();
            }

            return PartialView("_member", info);
        }


        public ActionResult FitnessProfiles(int? id)
        {
            id = Convert.ToInt32(Session["MembershipNo"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            FitnessProfile fitness = db.FitnessProfiles.Where(x => x.MemberShipNo == id.Value).FirstOrDefault();
            if (fitness == null)
            {
                return HttpNotFound();
            }
            return PartialView("_FitnessProfile", fitness);

        }

     



        public ActionResult PrivateComment(int? id)
        {
            id = Convert.ToInt32(Session["MembershipNo"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PrivateComment priv = db.PrivateComments.Where(x => x.PC_MembershipNo == id.Value).FirstOrDefault();
            if (priv == null)
            {
                return HttpNotFound();
            }
             return PartialView("_PrivateComment", priv);
           
        }
        /*
                // POST: Users/Edit/5
                // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
                // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public ActionResult PrivateComment([Bind(Include = "PC_Comment")] PrivateComment priv)
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(priv).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Account");
                    }
                    return PartialView("_PrivateComment", priv);
                }
                */


        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Account([Bind(Include = "Us_Membership,Us_FirstName,Us_LastName,Us_UserName,Us_Password,Us_Mobile,Us_Image")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Account");
            }
            return View(user);
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


            Session["LogedUsers"] = null;
            //Session["RoleID"] = null;
            return RedirectToAction("login", "Accounts");
        }





        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Us_Membership,Us_FirstName,Us_LastName,Us_UserName,Us_Password,Us_Mobile,Us_Image,")] User user , FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                var c = db.Players.Where(x => x.PL_Membership == user.Us_Membership && x.PL_Mobile == user.Us_Mobile).FirstOrDefault();
                if (c != null)
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    //  return RedirectToAction("Index");
                    Session["MembershipNo"] = user.Us_Membership.ToString();
                    Session["LogedUsers"] = user.Us_UserName.ToString();
                    int id = Convert.ToInt32(Session["MembershipNo"]);
                    return RedirectToAction("Account", "Users", Session["MembershipNo"] );
                }
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Us_Membership,Us_FirstName,Us_LastName,Us_UserName,Us_Password,Us_Mobile,Us_Image")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
