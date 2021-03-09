using Glow.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Glow.Controllers
{
    public class HomeController : Controller
    {
        private glowEntities db = new glowEntities();
        public ActionResult Index()
        {
            return View();
        }

      
        public ActionResult Change(string languageAbbrevation)
        {
            if (languageAbbrevation != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(languageAbbrevation);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageAbbrevation);
            }
            HttpCookie cookies = new HttpCookie("Home");
            cookies.Value = languageAbbrevation;
            Response.Cookies.Add(cookies);

            return View("Index");
        }
        public ActionResult Membership()
        {

            return View();
        }

       
        public ActionResult OpenPopup()

        {
            string v = Request.QueryString["param1"];
            ViewBag.member = v;
            return PartialView("_joinus") ;

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult send(FormCollection fc, Sendmail sm)
        {
            string v = Request.QueryString["param1"];
            ViewBag.member = v;

            // mail Address from where you send the mail
            var fromAddress = "xxxx";
            // any address where the email will be sending
            var toAddress = "xxx";
            //Password of your mail address
            const string fromPassword = "xxxx";
            var messages = new MailMessage();
            // Passing the values and make a email formate to display
            var Subject = sm.subject;
            var body = "From: " + sm.Fullname + "\n";
            body += "Email: " + sm.Email + "\n";
            body += "Phone : " + sm.MobilNo + "\n";
            body += "Question: \n" + v + "\n";

            // smtp settings
            var smtp = new System.Net.Mail.SmtpClient();
            {
                smtp.Host = "smtp-mail.outlook.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                //await smtp.SendMailAsync(messages);
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                smtp.Timeout = 70000;
            }
            // Passing values to smtp object
            smtp.Send(fromAddress, toAddress, Subject, body);
            //ScriptManager.RegisterStartupScript(messages , this.GetType(), "popup", "alert('Message has been sent successfully');", true);
            Response.Write("Message has been sent successfully");
            //return RedirectToAction("Index");
            //ModelState.Clear();

            return RedirectToAction("ContactUs");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginUser(User u,string username, string Password)
        {   // this action is for handle post (login)
            if (ModelState.IsValid) // this is check validity
            {
                string use = username;
                string pass = Password;
                   var v = db.Users.Where(a => a.Us_UserName.Equals(u.Us_UserName) && a.Us_Password.Equals(u.Us_Password)).FirstOrDefault();
                if (v != null)
                {
                    Session["LogedUsers"] = v.Us_UserName.ToString();
                    Session["MembershipNo"] = v.Us_Membership.ToString();

                    int id = Convert.ToInt32(Session["MembershipNo"]);
                 //   return RedirectToAction("Account", Session["MembershipNo"]);

                    return RedirectToAction("Account","Users", Session["MembershipNo"]);
                }

            }

            return RedirectToAction("");
        }



        public ActionResult About()
        {


            return View();
        }

        public ActionResult Classes()
        {
            var Classes =  db.clasesses.Where(a=>a.ClassDisplay == true);

            return PartialView("_classes", Classes);
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

        public ActionResult SlideShow()
        {
            var SlideShow = db.SLideshows;

            return PartialView("_slideshow", SlideShow);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult offers()
        {
            var offer = db.offers.Where(a => a.offerDisplay == true);

            return PartialView("_Offers", offer);
        }

        public ActionResult News()
        {
            var News = db.News.Where(a => a.NewsDisplay == true);

            return PartialView("_news", News);
        }
        public ActionResult News_footer()
        {
            var News = db.News;

            return PartialView("_RecentNewsFooter", News);
        }



        [HttpPost]
        public ActionResult BMI(decimal weight , decimal height )
        {
            if (ModelState.IsValid)
            {
                decimal Bmi =  (( Convert.ToDecimal (weight / height)  ) / height ) * 10000 ;
              

                ViewBag.BMI = decimal.Round(Bmi , 2 );
                return PartialView("_Bmi");
            }
            return PartialView("_Bmi");
        }
    }
}