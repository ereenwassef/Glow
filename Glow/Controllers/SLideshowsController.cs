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
    public class SLideshowsController : Controller
    {
        private glowEntities db = new glowEntities();

        // GET: SLideshows
        public ActionResult Index()
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                return View(db.SLideshows.ToList());
            }
            return RedirectToAction("Login", "Accounts");
        }

        // GET: SLideshows/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SLideshow sLideshow = db.SLideshows.Find(id);
            if (sLideshow == null)
            {
                return HttpNotFound();
            }
            return View(sLideshow);
        }
            return RedirectToAction("Login", "Accounts");
    }

        // GET: SLideshows/Create
        public ActionResult Create()
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Accounts");
        }

        // POST: SLideshows/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SLideshow sLideshow , List<HttpPostedFileBase> namefile, FormCollection fc)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                if (ModelState.IsValid)
            {
                foreach (HttpPostedFileBase file in namefile)
                {
                    sLideshow.slide_image = namefile.ToString();
                    file.SaveAs(Server.MapPath("/images/" + file.FileName));
                    sLideshow.slide_image = "/images/" + file.FileName;
                    db.SLideshows.Add(sLideshow);
                    db.SaveChanges();
                }
              
             
            }

            return View(sLideshow);
        }
            return RedirectToAction("Login", "Accounts");
    }

        // GET: SLideshows/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SLideshow sLideshow = db.SLideshows.Find(id);
            if (sLideshow == null)
            {
                return HttpNotFound();
            }
            return View(sLideshow);
            }
            return RedirectToAction("Login", "Accounts");
        }

        // POST: SLideshows/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,slide_Txt,slide_Txt_ar,slide_Content,slide_Content_ar,slide_Slogin,slide_Slogin_ar,slide_Url,slide_image")] SLideshow sLideshow, FormCollection fc, HttpPostedFileBase file)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                if (ModelState.IsValid)
            {
                var allowedExtensions = new[] {
              ".Jpg", ".png", ".jpg", ".jpeg"  };

                if (file == null)
                {
                    sLideshow.slide_image = fc["imgpath"].ToString();
                    db.Entry(sLideshow).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        file.SaveAs(Server.MapPath("/images/" + file.FileName));
                        sLideshow.slide_image = "/images/" + file.FileName;
                        db.Entry(sLideshow).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        TempData["message"] = "Please choose only Image file with Extension (.png)";
                        return RedirectToAction("Edit");
                    }
                }
            }
            return View(sLideshow);
        }
            return RedirectToAction("Login", "Accounts");
    }

        // GET: SLideshows/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SLideshow sLideshow = db.SLideshows.Find(id);
            if (sLideshow == null)
            {
                return HttpNotFound();
            }
            return View(sLideshow);
            }
            return RedirectToAction("Login", "Accounts");
        }

        // POST: SLideshows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                SLideshow sLideshow = db.SLideshows.Find(id);
         //   string path = Server.MapPath(sLideshow.slide_image);
          string pa = HttpContext.Server.MapPath(sLideshow.slide_image);
            System.IO.File.Delete(pa);

            db.SLideshows.Remove(sLideshow);
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
