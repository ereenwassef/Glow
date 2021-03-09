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
    public class PlayersController : Controller
    {
        private glowEntities db = new glowEntities();

        // GET: Players
        public ActionResult Index(string SearchBy, int?search)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                Player_Info model = new Player_Info();

            if (SearchBy == "MembershipNo")
            {
                return View(db.Players.Where(p => p.PL_Membership == search).ToList());
            }
            else if (SearchBy == "Mobile")
            {
                return View(db.Players.Where(p => p.PL_Mobile == search).ToList());
            }
            else
            {
                return View(db.Players.ToList());
            }
            }
            return RedirectToAction("Login", "Accounts");
        }



        // GET: Players/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }
            return RedirectToAction("Login", "Accounts");
    }

        // GET: Players/Create
        public ActionResult Create()
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                ViewBag.captionname = new SelectList(db.Captions, "Cap_ID", "Cap_Name");
           
            return View();
            }
            return RedirectToAction("Login", "Accounts");
        }

        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Player_Info model)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                if (ModelState.IsValid)
            {
                Player pl = new Player();
                pl.PL_Membership = model.PL_Membership;
                pl.PL_Mobile = model.PL_Mobile;

                db.Players.Add(pl);
                db.SaveChanges();

                int membershipNo = pl.PL_Membership;
                Info inf = new Info();
                inf.Inf_MembershipNo = membershipNo;
                inf.Inf_Startdate = model.Inf_Startdate;
                inf.Inf_Enddate = model.Inf_Enddate;
               // inf.Inf_CaptionName = model.Inf_CaptionName.Value;
                inf.Inf_FrezzDay = model.Inf_FrezzDay;
                inf.Inf_Cancel = false;
                db.Infoes.Add(inf);
                db.SaveChanges();


                return RedirectToAction("Index");
            }

            return View(model);
        }
            return RedirectToAction("Login", "Accounts");
    }



        public ActionResult Edit(int? id)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                ViewBag.captionname = new SelectList(db.Captions, "Cap_ID", "Cap_Name");

            /*

               var info = (from g in db.Players
                               from v in db.Infoes

                               from ga in g.Infoes
                               where ga.Inf_MembershipNo == id &&
                               v.Inf_MembershipNo == g.PL_Membership  
                               select new Player_Info
                               {
                                   Inf_Startdate = v.Inf_Startdate,
                                   Inf_MembershipNo = g.PL_Membership
                               });
              */

            var info = from p in db.Players 
                       join i in db.Infoes on p.PL_Membership equals i.Inf_MembershipNo
                       where p.PL_Membership == id
                       orderby p.PL_Membership descending
                       select new Player_Info {PL_Membership = p.PL_Membership , PL_Mobile = p.PL_Mobile, Inf_Startdate = i.Inf_Startdate , Inf_Enddate = i.Inf_Enddate , Inf_CaptionName = i.Inf_CaptionName , Inf_FrezzDay = i.Inf_FrezzDay,Inf_Cancel = i.Inf_Cancel};
           var model = info.FirstOrDefault();

            return View(model);
            }
            return RedirectToAction("Login", "Accounts");

        }

/*
        // GET: Players/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        } */

        // POST: Players/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Player_Info model)
        {
            // [Bind(Include = "PL_Membership,PL_Mobile,Inf_Startdate,Inf_Enddate,Inf_CaptionName,Inf_FrezzDay")]

            if (ModelState.IsValid)
            {
               
                var player = db.Players.Where(z => z.PL_Membership == model.PL_Membership).FirstOrDefault();
                player.PL_Membership = model.PL_Membership;
                player.PL_Mobile = model.PL_Mobile;

                int membershipNo = player.PL_Membership;



              //  Info inf = new Info();
                var info = db.Infoes.Where(z => z.Inf_MembershipNo == model.PL_Membership).FirstOrDefault();
                info.Inf_MembershipNo = membershipNo;
                info.Inf_Startdate = model.Inf_Startdate;
                info.Inf_Enddate = model.Inf_Enddate;
                info.Inf_CaptionName = model.Inf_CaptionName.Value;
                info.Inf_FrezzDay = model.Inf_FrezzDay;
                info.Inf_Cancel = model.Inf_Cancel;
                db.Entry(info).State = EntityState.Modified;
                db.Entry(player).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Players/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Player player = db.Players.Find(id);
            db.Players.Remove(player);
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
