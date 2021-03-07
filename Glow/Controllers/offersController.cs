using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Glow.Models;
using System.IO;

namespace Glow.Controllers
{
    public class offersController : Controller
    {
        private glowEntities db = new glowEntities();

        // GET: offers
        public ActionResult Index()
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                return View(db.offers.ToList().OrderByDescending(a => a.id));
            }
            return RedirectToAction("Login", "Accounts");

        }

        public ActionResult SlideShow()
        {
            var SlideShow = db.SLideshows;

            return PartialView("_slideshow", SlideShow);

        }

        public ActionResult News_footer()
        {
            var News = db.News;

            return PartialView("_RecentNewsFooter", News);
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


        public ActionResult Offers()
        {
            return View(this.getOffers(1));

        }
        [HttpPost]
        public ActionResult Offers(int currentPageIndex)
        {
            return View(this.getOffers(currentPageIndex));

        }

        private offer getOffers(int currentPage)
        {
            int maxrow = 8;
            using (glowEntities entity = new glowEntities())
            {
                offer off = new offer();
                off.offers = (from offer in entity.offers select offer)
                    .OrderBy(o => o.id)
                    .Skip((currentPage - 1) * maxrow)
                    .Take(maxrow).ToList();
                decimal pageCount = (decimal)(entity.offers.Count()) / (maxrow);

                off.PageCount = (int)Math.Ceiling(pageCount);
               
                off.CurrentPageIndex = currentPage;
                return off;
            }
        }

        public ActionResult OffersDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            offer offer = db.offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);

        }


        // GET: offers/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            offer offer = db.offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }
            return RedirectToAction("Login", "Accounts");
    }

        // GET: offers/Create
        public ActionResult Create()
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Accounts");
        }

        // POST: offers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(offer offer , List<HttpPostedFileBase> namefile, FormCollection fc)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                if (ModelState.IsValid)
            {
                foreach (HttpPostedFileBase file in namefile)
                {

                    offer.offerImage = namefile.ToString();
                    file.SaveAs(Server.MapPath(@"/images/" + file.FileName));
                    offer.offerImage = @"/images/" + file.FileName;

                    db.offers.Add(offer);
                    db.SaveChanges();
                   
                }
            }

            return View(offer);
        }
            return RedirectToAction("Login", "Accounts");
    }

        // GET: offers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            offer offer = db.offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
            }
            return RedirectToAction("Login", "Accounts");
        }

        // POST: offers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,offername_Ar,offername,offertitle_Ar,offertitle,offercontent_Ar,offercontent,offerImage,offerDisplay")] offer offer, FormCollection fc, HttpPostedFileBase file)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                if (ModelState.IsValid)
            {
                var allowedExtensions = new[] {
              ".Jpg", ".png", ".jpg", ".jpeg"  };

                if (file == null)
                {
                    offer.offerImage = fc["imgpath"].ToString();
                    db.Entry(offer).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        file.SaveAs(Server.MapPath(@"/images/" + file.FileName));
                        offer.offerImage = @"/images/" + file.FileName;
                        db.Entry(offer).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        TempData["message"] = "Please choose only Image file with Extension (.png)";
                        return RedirectToAction("Edit");
                    }
                }
            }
            return View(offer);
        }
            return RedirectToAction("Login", "Accounts");
    }

        // GET: offers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            offer offer = db.offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
            }
            return RedirectToAction("Login", "Accounts");
        }

        // POST: offers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                offer offer = db.offers.Find(id);

            string path = Server.MapPath(offer.offerImage);
            db.offers.Remove(offer);
           
            db.SaveChanges();
            System.IO.File.Delete(path);
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
