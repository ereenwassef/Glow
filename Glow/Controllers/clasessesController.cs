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
using PagedList;

namespace Glow.Controllers
{
    public class clasessesController : Controller
    {
        private glowEntities db = new glowEntities();

        // GET: clasesses
        public ActionResult Index()
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                return View(db.clasesses.ToList().OrderByDescending(a => a.id));
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



        public ActionResult Classes()
        {
            return View(this.getclasses(1));

        }
        [HttpPost]
        public ActionResult Classes(int currentPageIndex)
        {
            return View(this.getclasses(currentPageIndex));

        }

        private clasess getclasses(int currentPage)
        {
            int maxrow = 8;
            using (glowEntities entity = new glowEntities())
            {
                clasess cls = new clasess();
                cls.clls = (from clasess in entity.clasesses select clasess)
                    .OrderBy(c => c.id)
                    .Skip((currentPage - 1) * maxrow)
                    .Take(maxrow).ToList();
                decimal pageCount = (decimal)(entity.clasesses.Count()) / (maxrow);
               
                cls.PageCount = (int)Math.Ceiling(pageCount);
                //ViewBag.pagect = cls.PageCount;
                cls.CurrentPageIndex = currentPage;
                return cls;
            }
        }

        public ActionResult ClassDetails(int? id)
        {
           
        
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            clasess clas = db.clasesses.Find(id);
            if (clas == null)
            {
                return HttpNotFound();
            }
            return View(clas);
          

        }

        // GET: clasesses/Details/5
        public ActionResult Details(int? id)
        {
        if (Session["LogedUserName"] != null && Session["RoleID"] != null)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            clasess clasess = db.clasesses.Find(id);
            if (clasess == null)
            {
                return HttpNotFound();
            }
            return View(clasess);
            }
            return RedirectToAction("Login", "Accounts");
        }

        // GET: clasesses/Create
        public ActionResult Create()
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Accounts");
        }

        // POST: clasesses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(clasess clasess , List<HttpPostedFileBase> namefile, FormCollection fc)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                if (ModelState.IsValid)
            {
                foreach (HttpPostedFileBase file in namefile)
                {
                    clasess.CLass_image = namefile.ToString();
                    file.SaveAs(Server.MapPath(@"/images/" + file.FileName));
                    clasess.CLass_image = @"/images/" + file.FileName;

                    db.clasesses.Add(clasess);
                    db.SaveChanges();
                    
                }
            }

            return View(clasess);
            }
            return RedirectToAction("Login", "Accounts");
        }

        // GET: clasesses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            clasess clasess = db.clasesses.Find(id);
            if (clasess == null)
            {
                return HttpNotFound();
            }
            return View(clasess);
            }
            return RedirectToAction("Login", "Accounts");
        }

        // POST: clasesses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Class_Name,Class_Name_ar,Class_title,Class_title_ar,Class_Content,Class_Content_ar,CLass_image,ClassDisplay")] clasess clasess, FormCollection fc, HttpPostedFileBase file)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                if (ModelState.IsValid)
            {
                var allowedExtensions = new[] {
              ".Jpg", ".png", ".jpg", ".jpeg" , ".PNG"   };

                if (file == null)
                {
                    clasess.CLass_image = fc["imgpath"].ToString();
                    db.Entry(clasess).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        file.SaveAs(Server.MapPath(@"/images/" + file.FileName));
                        clasess.CLass_image = @"/images/" + file.FileName;
                        db.Entry(clasess).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        TempData["message"] = "Please choose only Image file with Extension (.png)";
                        return RedirectToAction("Edit");
                    }
                }

            }
            return View(clasess);
            }
            return RedirectToAction("Login", "Accounts");
        }

        // GET: clasesses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            clasess clasess = db.clasesses.Find(id);
            if (clasess == null)
            {
                return HttpNotFound();
            }
            return View(clasess);
            }
            return RedirectToAction("Login", "Accounts");
        }

        // POST: clasesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                clasess clasess = db.clasesses.Find(id);
            string path = Server.MapPath(clasess.CLass_image);
            System.IO.File.Delete(path);
            db.clasesses.Remove(clasess);
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
