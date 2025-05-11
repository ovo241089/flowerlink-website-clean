using FlowerLink.Models.BLL;
using FlowerLink.Models.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using CaptchaMvc.HtmlHelpers;

namespace FlowerLink.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()

        {
            var settng = new settingBLL().GetSettings();
            
            if (settng.IsMaintenance ==1)
            {
                return RedirectToAction("Maintenance");
            }
            else {
                var data = new flashSaleService().GetFlashSale();

                //ViewBag.FlashSale = data.FromDate.ToString("dd-mm-YYYY");
                if (data!=null)
                {
                    ViewBag.FlashSaleStatus = data.StatusID;
                    ViewBag.FlashSaleStart = data.FromDate.HasValue
                    ? data.FromDate.Value.ToString("yyyy/MM/dd")
                    : "<not available>";

                    ViewBag.FlashSaleEnd = data.ToDate.HasValue
                   ? data.ToDate.Value.ToString("yyyy/MM/dd")
                     : "<not available>";
                    ViewBag.FlashSaleJunc = data.flashSaleJuncBLL.ToList();
                }
                else
                {
                    ViewBag.FlashSaleStatus = 0;
                    ViewBag.FlashSaleJunc = null;
                }
                //desktop
                ViewBag.TenItems = new itemService().GetSelecteditems();
                //Mob
                ViewBag.itemList = new itemService().GetAll(); 


            ViewBag.Featureditems = new itemService().GetAllFeatured();
            ViewBag.Category = new categoryBLL().GetAll();
            //ViewBag.SubCategory = new subcategoryBLL().GetAll();
            //ViewBag.Color = new colorBLL().GetAll();
            ViewBag.Deal = new dealBLL().GetAll();
            
            ViewBag.Banner = new bannerBLL().GetBanner("Home");                      
            }
            return View();
        }
        public ActionResult Maintenance()
        {           
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Banner = new bannerBLL().GetBanner("About");
            return View();
        }
        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Banner = new bannerBLL().GetBanner("Contact");
            return View();
        }
        [HttpPost]
        public ActionResult Contact(contactBLL obj)
        {
            ViewBag.Banner = new bannerBLL().GetBanner("Contact");
            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                ViewBag.Contact = "";
                string ToEmail, SubJect, cc, Bcc;
                cc = "";
                Bcc = "";
                ToEmail = ConfigurationManager.AppSettings["From"].ToString();
                SubJect = "New Query From Customer";
                string Body = System.IO.File.ReadAllText(Server.MapPath("~/Template") + "\\" + "contact.txt");
                DateTime dateTime = DateTime.UtcNow.Date;
                Body = Body.Replace("#Date#", dateTime.ToString("dd/MMM/yyyy"));
                Body = Body.Replace("#Name#", obj.Name.ToString());
                Body = Body.Replace("#Email#", obj.Email.ToString());
                Body = Body.Replace("#Contact#", obj.Phone.ToString());
                Body = Body.Replace("#Subject#", obj.Subject.ToString());
                Body = Body.Replace("#Message#", obj.Message.ToString());
                try
                {
                    WebMail.SmtpServer = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                    WebMail.SmtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"].ToString());
                    WebMail.SmtpUseDefaultCredentials = true;
                    WebMail.EnableSsl = false;
                    WebMail.UserName = ConfigurationManager.AppSettings["UserName"].ToString();

                    WebMail.From = ConfigurationManager.AppSettings["From"].ToString();
                    WebMail.Password = ConfigurationManager.AppSettings["Password"].ToString();
                    WebMail.Send(to: ToEmail, subject: SubJect, body: Body, cc: cc, bcc: Bcc, isBodyHtml: true);
                    ViewBag.Contact = "Your Query is received. Our support department contact you soon.";
                }
                catch (Exception ex)
                {
                    ViewBag.Contact = "";
                }
            }
            else
            {
                ViewBag.ErrMessage = "Error: Captcha is not valid.";
            }
            return View();
        }
        public JsonResult Subscribe(string email)
        {
            string ToEmail, SubJect, cc, Bcc;
            cc = "";
            Bcc = "";
            ToEmail = ConfigurationManager.AppSettings["From"].ToString();
            SubJect = "New Subscribtion at FlowerLink";
            string Body = System.IO.File.ReadAllText(Server.MapPath("~/Template") + "\\" + "newsletter.txt");
            Body = Body.Replace("#email#", email.ToString());
            try
            {
                WebMail.SmtpServer = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                WebMail.SmtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"].ToString());
                WebMail.SmtpUseDefaultCredentials = true;
                WebMail.EnableSsl = false;
                WebMail.UserName = ConfigurationManager.AppSettings["UserName"].ToString();

                WebMail.From = ConfigurationManager.AppSettings["From"].ToString();
                WebMail.Password = ConfigurationManager.AppSettings["Password"].ToString();
                WebMail.Send(to: ToEmail, subject: SubJect, body: Body, cc: cc, bcc: Bcc, isBodyHtml: true);
                ViewBag.Status = "";
            }
            catch (Exception ex)
            {
                ViewBag.Status = "";
            }

            return Json(email, JsonRequestBehavior.AllowGet);
        }
        //Get Setting Details
        public ActionResult GetSetting()
        {
            return Json(new settingBLL().GetSettings(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Policy()
        {
            return View();
        }
    }
}