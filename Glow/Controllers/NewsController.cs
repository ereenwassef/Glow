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
using System.Web.Helpers;

namespace Glow.Controllers
{
    public class NewsController : Controller
    {
        private glowEntities db = new glowEntities();

        // GET: News
        public ActionResult Index()
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                return View(db.News.ToList().OrderByDescending(a=>a.id));
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


        public ActionResult News()
        {
            return View(this.getNews(1));

        }
        [HttpPost]
        public ActionResult News(int currentPageIndex)
        {
            return View(this.getNews(currentPageIndex));

        }

        private News getNews(int currentPage)
        {
            int maxrow = 8;
            using (glowEntities entity = new glowEntities())
            {
                News ne = new News();
                ne.newss = (from News in entity.News select News)
                    .OrderBy(n => n.id)
                    .Skip((currentPage - 1) * maxrow)
                    .Take(maxrow).ToList();
                decimal pageCount = (decimal)(entity.News.Count()) / (maxrow);

                ne.PageCount = (int)Math.Ceiling(pageCount);

                ne.CurrentPageIndex = currentPage;
                return ne;
            }
        }

        public ActionResult NewsDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            News ne = db.News.Find(id);
            if (ne == null)
            {
                return HttpNotFound();
            }
            return View(ne);

        }

        // GET: News/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
            }
            return RedirectToAction("Login", "Accounts");
        }

        // GET: News/Create
        public ActionResult Create()
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                return View();
        }
            return RedirectToAction("Login", "Accounts");
    }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(News news, List<HttpPostedFileBase> namefile, FormCollection fc)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                if (ModelState.IsValid)
            {
                foreach (HttpPostedFileBase file in namefile)
                {
                    news.NewsImage = namefile.ToString();
                    file.SaveAs(Server.MapPath(@"/images/" + file.FileName));
                    news.NewsImage = @"/images/" + file.FileName;

                    db.News.Add(news);
                    db.SaveChanges();
                    
                }
            }

            return View(news);
            }
            return RedirectToAction("Login", "Accounts");
        }

        // GET: News/Edit/5
        [ValidateInput(false)]

        public ActionResult Edit(int? id)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
              
                return View(news);
        }
            return RedirectToAction("Login", "Accounts");
    }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Newsname_Ar,Newsname,Newstitle_Ar,Newstitle,Newscontent_Ar,Newscontent,NewsImage,NewsDate,NewsDisplay")] News news, FormCollection fc, HttpPostedFileBase file)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                if (ModelState.IsValid)
            {
                var allowedExtensions = new[] {
              ".Jpg", ".png", ".jpg", ".jpeg" , ".PNG"   };

                if (file == null)
                {
                    news.NewsImage = fc["imgpath"].ToString();
                    db.Entry(news).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        file.SaveAs(Server.MapPath(@"/images/" + file.FileName));
                        news.NewsImage = @"/images/" + file.FileName;
                        db.Entry(news).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        TempData["message"] = "Please choose only Image file with Extension (.png)";
                        return RedirectToAction("Edit");
                    }
                }
            }
            return View(news);
            }
            return RedirectToAction("Login", "Accounts");
        }

        // GET: News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }
            return RedirectToAction("Login", "Accounts");
    }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["LogedUserName"] != null && Session["RoleID"] != null)
            {
                News news = db.News.Find(id);
            string path = Server.MapPath(news.NewsImage);
            System.IO.File.Delete(path);

            db.News.Remove(news);
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
