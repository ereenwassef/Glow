using System;
using System.Net;
using System.Web.Mvc;
using Glow.Models;
using System.Net.Mail;
using System.Linq;
namespace Glow.Controllers
{


    public class ContactUSController : Controller
    {
        private glowEntities db = new glowEntities();
        // GET: ContactUS
        public ActionResult Index()
        {
            return View();
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

        public ActionResult ContactUs()
        {
            return View();
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



        public ActionResult contacts_US()
        {
            //  var Contact = db.contacts.FirstOrDefault();
            var item = db.contacts.FirstOrDefault();
            ViewBag.Con_AddressAr = item.con_addressAR;
            ViewBag.Con_Address = item.con_address;
            ViewBag.Con_Phone = item.Con_Phone;
            ViewBag.Con_Email = item.Con_Email;
            ViewBag.Con_WhatsApp = item.Con_WhatsApp;
            return PartialView("_ContuctUs", item);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult send(FormCollection fc, Sendmail sm)
        {

                // mail Address from where you send the mail
                var fromAddress = "xxxx";
                // any address where the email will be sending
                var toAddress = "xxxx";
                //Password of your mail address
                const string fromPassword = "xxxx";
                var messages = new MailMessage();
                // Passing the values and make a email formate to display
                var Subject = sm.subject;
                var body = "From: " + sm.Fullname + "\n";
                body += "Email: " + sm.Email + "\n";
                body += "Phone : " + sm.MobilNo + "\n";
                body += "Question: \n" + sm.Detail + "\n";

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


        protected void sendmail(object sender , EventArgs e)
        {
            Sendmail sm = new Sendmail();
            try
            {
                SmtpClient client = new SmtpClient("smtp.xxx.com", 587);
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("xxxx", "xxxx");
                MailMessage mail = new MailMessage();
                mail.To.Add("xxx@xxxx.com");
                mail.From = new MailAddress("xxx@xxxx.com");
                mail.Subject = sm.subject;
                mail.Body = sm.Detail;
                client.Send(mail);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "alert('Message has been sent successfully');", true);
                Response.Write("Message has been sent successfully");

            }
            catch (Exception ex)
            {

                Response.Write("Couldnot send message" + ex.Message);
            }
        }


    }
}