using FlowerLink.Models.BLL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static FlowerLink.Models.BLL.checkoutBLL;
using FlowerLink.Models;
using System.Configuration;
using com.fss.plugin;
using FlowerLink.Models.Service;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using FlowerLink.Models.TapPayment;
using System.Net.Http;

namespace FlowerLink.Controllers
{
    public class OrderController : Controller
    {         
        public ActionResult Cart()
        {
            ViewBag.Banner = new bannerBLL().GetBanner("Cart");
            return View();
        }
        public ActionResult Checkout(int id = -1)
        {

            //ViewBag.Banner = new bannerBLL().GetBanner("Checkout");
            //int CustomerID = id;
            //if (CustomerID == 0)
            //{
            //    ViewBag.Featureditems = new itemService().GetAllFeatured();
            //    ViewBag.Gift = new giftService().GetAll();

            //    Session["CustomerID"] = 0;
            //    return View();
            //}
            //else
            //{
            //    if (Session["CustomerID"] != null && Convert.ToInt32(Session["CustomerID"]) != 0)
            //    {
            //        ViewBag.Featureditems = new itemService().GetAllFeatured();
            //        ViewBag.Gift = new giftService().GetAll();

            //        return View();
            //    }
            //    else
            //    {
            //        ViewBag.Featureditems = new itemService().GetAllFeatured();
            //        ViewBag.Gift = new giftService().GetAll();

            //        return RedirectToAction("Login_Register", "Account");
            //    }
            //}
            try
            {
                ViewBag.Banner = new bannerBLL().GetBanner("Checkout");
                ViewBag.Featureditems = new itemService().GetAllFeatured();
               
            }
            catch (Exception ex)
            { }
            return View();
        }
        public ActionResult UpSell()
        {
            ViewBag.Featureditems = new itemService().GetAllFeatured();
            return PartialView("_UpSellingModal", ViewBag.Featureditems);
        }
        public ActionResult OrderComplete(int OrderID = 0, string OrderNo = "")
        {
            checkoutBLL check = new checkoutBLL();
            if (OrderNo == "Reject")
            {
                ViewBag.OrderNo = "Reject";
                //check.OrderUpdate(OrderID, 103);//Rejected 
            }
            else
            {
                var data = new myorderBLL().GetDetails(OrderID);
                if (data.PaymentMethodTitle != "Cash on Delivery")
                {
                    check.OrderUpdate(OrderID, 101);//In progress
                }
                string ToEmail, SubJect, cc, Bcc;
                ToEmail = data.SenderEmail;
                SubJect = "[FlowerLink] ORDER RECEIVED : " + data.OrderNo;
                string Body = System.IO.File.ReadAllText(Server.MapPath("~/Template") + "\\" + "emailpattern.txt");
                string items = "";
                foreach (var item in data.OrderDetail)
                {
                    try
                    {
                        items += "<table border = '0' cellpadding = '0' cellspacing = '0' align = 'center' width = '100%' role = 'module' data - type = 'columns' style = 'padding:20px 20px 20px 30px;' bgcolor = '#FFFFFF'>"
                        + "<tbody>"
                        + "<tr role = 'module-content'>"
                        + "<td height = '100%' valign = 'top'>"
                        + "<table class='column' width='137' style='width:137px; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 0px;' cellpadding='0' cellspacing='0' align='left' border='0' bgcolor=''>"
                        + "<tbody>"
                        + "<tr>"
                        + "<td style = 'padding:0px;margin:0px;border-spacing:0;'><table class='wrapper' role='module' data-type='image' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='239f10b7-5807-4e0b-8f01-f2b8d25ec9d7'>"
                        + "<tbody>"
                        + "<tr>"
                        + "<td style = 'font-size:6px; line-height:10px; padding:0px 0px 0px 0px;' valign='top' align='left'>"
                        + "<img src = '" + System.Configuration.ConfigurationManager.AppSettings["Image"].ToString() + item.ItemImage + "' class='max-width' border='0' style='display:block;width: 108px;height: 108px;object-fit: contain;' alt='' >"
                        + "</td>"
                        + "</tr>"
                        + "</tbody>"
                        + "</table></td>"
                        + "</tr>"
                        + "</tbody>"
                        + "</table>"
                        + "<table class='column' style='display: contents; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 0px;' cellpadding='0' cellspacing='0' align='left' border='0' bgcolor=''>"
                        + "<tbody>"
                        + "<tr>"
                        + "<td style = 'padding:0px;margin:0px;border-spacing:0;' ><table class='module' role='module' data-type='text' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='f404b7dc-487b-443c-bd6f-131ccde745e2'>"
                        + "<tbody>"
                        + "<tr>"
                        + "<td style = 'padding:18px 0px 18px 0px; line-height:22px; text-align:inherit;' height='100%' valign='top' bgcolor='' role='module-content'><div>"
                        + "<div style = 'font-family: inherit; text-align: inherit'> " + item.ItemTitle + "</div>"
                        + "<div style = 'font-family: inherit; text-align: inherit'> Qty : " + item.Quantity + "</div>"
                        + "<div style = 'font-family: inherit; text-align: inherit'><span style='color: #000263'>BD " + item.Price + "</span></div>"
                        + "<div></div></div></td>"
                        + "</tr>"
                        + "</tbody>"
                        + "</table>"
                        + "</td>"
                        + "</tr>"
                        + "</tbody>"
                        + "</table>"
                        + "</td>"
                        + "</tr>"
                        + "</tbody>"
                        + "</table>";

                    }
                    catch (Exception ex)
                    {
                    }
                }
                string gifts = "";
                if (data.GiftDetail.Count > 0)
                {
                    foreach (var item in data.GiftDetail)
                    {
                        try
                        {
                            gifts += "<table border = '0' cellpadding = '0' cellspacing = '0' align = 'center' width = '100%' role = 'module' data - type = 'columns' style = 'padding:20px 20px 20px 30px;' bgcolor = '#FFFFFF'>"
                            + "<tbody>"
                            + "<tr role = 'module-content'>"
                            + "<td height = '100%' valign = 'top'>"
                            + "<table class='column' width='137' style='width:137px; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 0px;' cellpadding='0' cellspacing='0' align='left' border='0' bgcolor=''>"
                            + "<tbody>"
                            + "<tr>"
                            + "<td style = 'padding:0px;margin:0px;border-spacing:0;' ><table class='wrapper' role='module' data-type='image' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='239f10b7-5807-4e0b-8f01-f2b8d25ec9d7'>"
                            + "<tbody>"
                            + "<tr>"
                            + "<td style = 'font-size:6px; line-height:10px; padding:0px 0px 0px 0px;' valign='top' align='left'>"
                            + "<img src = '" + System.Configuration.ConfigurationManager.AppSettings["Image"].ToString() + item.GiftImage + "' class='max-width' border='0' style='display:block;width: 108px;height: 108px;object-fit: contain;' alt='' >"
                            + "</td>"
                            + "</tr>"
                            + "</tbody>"
                            + "</table></td>"
                            + "</tr>"
                            + "</tbody>"
                            + "</table>"
                            + "<table class='column' style='display: contents; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 0px;' cellpadding='0' cellspacing='0' align='left' border='0' bgcolor=''>"
                            + "<tbody>"
                            + "<tr>"
                            + "<td style = 'padding:0px;margin:0px;border-spacing:0;' ><table class='module' role='module' data-type='text' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='f404b7dc-487b-443c-bd6f-131ccde745e2'>"
                            + "<tbody>"
                            + "<tr>"
                            + "<td style = 'padding:18px 0px 18px 0px; line-height:22px; text-align:inherit;' height='100%' valign='top' bgcolor='' role='module-content'><div>"
                            + "<div style = 'font-family: inherit; text-align: inherit'> " + item.GiftTitle + "</div>"
                            + "<div style = 'font-family: inherit; text-align: inherit'><span style='color: #000263'>BD " + item.Price + "</span></div>"
                            + "<div></div></div></td>"
                            + "</tr>"
                            + "</tbody>"
                            + "</table>"
                            + "</td>"
                            + "</tr>"
                            + "</tbody>"
                            + "</table>"
                            + "</td>"
                            + "</tr>"
                            + "</tbody>"
                            + "</table>";

                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    gifts += "<table class='module' role='module' data-type='divider' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' datad-muid='f7373f10-9ba4-4ca7-9a2e-1a2ba700deb9.1'>"
                    + "<tbody>"
                    + "<tr>"
                    + "<td style='padding:20px 30px 0px 30px;' role='module-content' height='100%' valign='top' >"
                    + "<table border='0' cellpadding='0' cellspacing='0' align='center' width='100%' height='3px' style='line-height:3px; font-size:3px;'>"
                    + "<tbody>"
                    + "<tr>"
                    + "<td style='padding:0px 0px 3px 0px;background-color: #ffcc00'></td>"
                    + "</tr>"
                    + "</tbody>"
                    + "</table>"
                    + "</td>"
                    + "</tr>"
                    + "</tbody>"
                    + "</table>";
                }
                Body = Body.Replace("#ReceiverName#", data.CustomerName.ToString());
                Body = Body.Replace("#OrderNo#", data.OrderNo.ToString());
                Body = Body.Replace("#items#", items.ToString());
                Body = Body.Replace("#gifts#", gifts.ToString());
                DateTime dateTime = DateTime.UtcNow.AddMinutes(180);
                Body = Body.Replace("#Customer#", data.SenderName.ToString());
                Body = Body.Replace("#SelectedTime#", data.SelectedTime.ToString());
                Body = Body.Replace("#DeliveryDate#", data.DeliveryDate==null?"" : Convert.ToDateTime(data.DeliveryDate).ToString("dd/MMM/yyyy"));
                Body = Body.Replace("#OrderDate#", dateTime.ToString("dd/MMM/yyyy"));
                if (data.Address == null)
                {
                    Body = Body.Replace("#Address#", "Not Defined");
                }
                else { 
                Body = Body.Replace("#Address#", data.Address.ToString());
                }
                string PaymentType = "";
                if (data.PaymentMethodID != 1)
                {
                    PaymentType = "Online Payment";
                }
                else
                {
                    PaymentType = "Cash On Delivery";
                }
                Body = Body.Replace("#PaymentType#", PaymentType.ToString());
                Body = Body.Replace("#PaymentMethod#", data.PaymentMethodTitle.ToString());
                Body = Body.Replace("#TotalItems#", data.TotalItems.ToString());
                Body = Body.Replace("#SubTotal#", data.AmountTotal.ToString());
                Body = Body.Replace("#Tax#", data.Tax.ToString());
                Body = Body.Replace("#DeliveryAmount#", data.DeliveryAmount.ToString());
                Body = Body.Replace("#GrandTotal#", data.GrandTotal.ToString());

                cc = "";
                Bcc = ConfigurationManager.AppSettings["From"].ToString();
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
                    ViewBag.Status = "Order Invoice will be sent to your Email.";
                }
                catch (Exception ex)
                {
                    ViewBag.Status = "";
                }
                ViewBag.OrderNo = data.OrderNo;
            }

            return View();
        }
        //Coupon
        public JsonResult Coupon(string coupon)
        {
            couponBLL couponBLL = new couponBLL();
            ViewBag.coupon = couponBLL.Get(coupon);
            return Json(new { data = ViewBag.coupon }, JsonRequestBehavior.AllowGet);
        }
        //Order
        public async Task<JsonResult> PunchOrder(checkoutBLL data)
        {
            checkoutBLL _service = new checkoutBLL();
            //orderdetails
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(JArray.Parse(data.OrderDetailString));
            JArray jsonResponse = JArray.Parse(json);
            data.OrderDetail = jsonResponse.ToObject<List<OrderDetails>>();

            //gifts
            try
            {
                if (data.OrderGiftsString != "")
                {
                    string jsonGift = Newtonsoft.Json.JsonConvert.SerializeObject(JArray.Parse(data.OrderGiftsString));
                    JArray jsonResponseGift = JArray.Parse(jsonGift);
                    data.OrderGifts = jsonResponseGift.ToObject<List<OrderGiftDetails>>();
                }
            }
            catch (Exception ex)
            { }

            int rtn = _service.InsertOrder(data);

            if (data.PaymentMethodID == 2)//Credimax 
            {
                PaymentDetails details = new PaymentDetails();
                int LastOrderID = rtn;
                try
                {
                    var client = new RestClient("https://credimax.gateway.mastercard.com/api/rest/version/54/merchant/E10561950/session");
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.DefaultConnectionLimit = 9999;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12;
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Authorization", "Basic bWVyY2hhbnQuRTEwNTYxOTUwOjhhYTlhZmI5OTg0ODZhMjA0ZjI0ODY0YzIyOTY1OGNh");
                    request.AddHeader("Content-Type", "text/plain");
                    request.AddHeader("Cookie", "TS01f8f5b8=014700973636cae9b19e68becca9cffe02f1b9bf08b3571d5588f1c9f20e1e143356517226b07304069c1eb77d86ef59bc3b54c7a3; TS01f8f5b8=014700973629fbc5b1c98bef8215e78947ec712f67e7bc8aad2f02e93d9992564bd3e564fc4640688959a52d9f31076f56d0f37df1");
                    request.AddParameter("text/plain", "{\n    \"apiOperation\" : \"CREATE_CHECKOUT_SESSION\",\n    \"order\": {\n            \"amount\" : \"" + data.GrandTotal + "\",\n            \"currency\" : \"BHD\", \n            \"id\" : \"" + data.OrderNo + "\" \n        },\n        \"interaction\":{\n        \"operation\":\"PURCHASE\", \n        \"returnUrl\":\"https://flowerlink.net/Order/OrderComplete?OrderNo=" + data.OrderNo + "&OrderID=" + LastOrderID + "\", \n        \"cancelUrl\":\"https://flowerlink.net/Order/OrderComplete?OrderNo=Reject&OrderID=" + LastOrderID + "\", \n            \"merchant\": {\n                 \"name\": \"Flower link\",\n                 \"logo\": \"https://flowerlink.net/Content/assets/images/logo/logo2.png\"\n                 },\n        }\n}", ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    var s = response.Content;
                    dynamic dynamicObject = JsonConvert.DeserializeObject(s);
                    string sessionID = dynamicObject["session"]["id"].ToString();
                    details.OrderNo = data.OrderNo;
                    details.GrandTotal = Convert.ToInt32(data.GrandTotal);
                    details.Description = data.OrderNo;
                    details.sessionID = sessionID;
                    return Json(new { data = details }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new { data = details }, JsonRequestBehavior.AllowGet);
                }
            }
            if (data.PaymentMethodID == 3)
            {
                PaymentDetails details = new PaymentDetails();
                data.OrderID = rtn; // rtn => Current Order ID
                try
                {
                    var response = await ProcessPayment(data);
                    if (!string.IsNullOrEmpty(response))
                    {
                        return Json(new { data = "PayPal", success = true, message = "Payment processed.", url = response });
                    }
                    else
                    {
                        return Json(new { data = "PayPal", success = false, message = "Process failed." });
                    }
                }
                catch (Exception)
                {
                    return Json(new { data = details }, JsonRequestBehavior.AllowGet);
                }
            }
            if (data.PaymentMethodID == 4)//Benefit Pay
            {
                int LastOrderID = rtn;
                return Json(new { data = "BenefitPay", OrderID = LastOrderID }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = rtn }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MyOrders()
        {
            ViewBag.Banner = new bannerBLL().GetBanner("Other");
            if (Session["CustomerID"] != null && Convert.ToInt32(Session["CustomerID"]) != 0)
            {
                return View(new myorderBLL().GetAll(Convert.ToInt32(Session["CustomerID"])));
            }
            else
            {
                return RedirectToAction("Login_Register", "Account");
            }
        }
        public ActionResult OrderDetails(int OrderID)
        {
            ViewBag.Banner = new bannerBLL().GetBanner("Other");
            return View(new myorderBLL().GetDetails(OrderID));
        }
        public ActionResult BenefitPay(string OrderNo, int OrderID, double GrandTotal)
        {
            iPayBenefitPipe pipe = new iPayBenefitPipe();
            Session["OrderNo"] = OrderNo;
            // Do NOT change the values of the following parameters at all.
            pipe.setAction("1");
            pipe.setCurrency("048");
            pipe.setLanguage("EN");
            pipe.setType("D");

            // modify the following to reflect your "Alias Name", "resource.cgn" file path, "keystore.bin" file path.
            pipe.setAlias("prod200001056");
            string filepath = Server.MapPath("~/BenefitPay") + "\\";
            pipe.setResourcePath(filepath); //only the path that contains the file; do not write the file name
            pipe.setKeystorePath(filepath); //only the path that contains the file; do not write the file name

            // modify the following to reflect your pages URLs
            string responseurl = "https://flowerlink.net/Order/BenefitPayResponse?OrderID=" + OrderID;
            pipe.setResponseURL(responseurl.ToString());
            string errorurl = "https://flowerlink.net/Order/BenefitPayResponse";
            pipe.setErrorURL(errorurl.ToString());

            // set a unique track ID for each transaction so you can use it later to match transaction response and identify transactions in your system and “BENEFIT Payment Gateway” portal.
            pipe.setTrackId(OrderNo);

            // set transaction amount
            pipe.setAmt(GrandTotal.ToString());

            // The following user-defined fields (UDF1, UDF2, UDF3, UDF4, UDF5) are optional fields.
            // However, we recommend setting theses optional fields with invoice/product/customer identification information as they will be reflected in “BENEFIT Payment Gateway” portal where you will be able to link transactions to respective customers. This is helpful for dispute cases. 
            pipe.setUdf1("");

            pipe.setUdf2("FlowerLink");
            pipe.setUdf3("");
            pipe.setUdf4("");
            pipe.setUdf5("");

            int val = pipe.performPaymentInitializationHTTP();
            var Address = "";
            if (val == 0)
            {
                Address = pipe.getWebAddress();
                Response.Redirect(pipe.getWebAddress());
            }
            else
            {
                Address = "error";
                Response.Write("error: " + pipe.getError());
            }
            return View();
        }
        public ActionResult BenefitPayResponse(int OrderID = 0)
        {
            iPayBenefitPipe pipe = new iPayBenefitPipe();
            pipe.setAlias("prod200001056");
            string filepath = Server.MapPath("~/BenefitPay") + "\\";
            pipe.setResourcePath(filepath); //only the path that contains the file; do not write the file name
            pipe.setKeystorePath(filepath); //only the path that contains the file; do not write the file name
            string trandata = "";
            string paymentID = "";
            string result = "";
            string responseCode = "";
            string response = "";
            string transactionID = "";
            string referenceID = "";
            string trackID = "";
            string amount = "";
            string UDF1 = "";
            string UDF2 = "";
            string UDF3 = "";
            string UDF4 = "";
            string UDF5 = "";
            string authCode = "";
            string postDate = "";
            string errorCode = "";
            string errorText = "";

            if (Request.Form["trandata"] != null)
            {
                trandata = Request.Form["trandata"].ToString();
                int parse = pipe.parseEncryptedRequest(trandata);
                if (parse == 0)
                {
                    paymentID = pipe.getPaymentId();
                    result = pipe.getResult();
                    responseCode = pipe.getAuthRespCode();
                    transactionID = pipe.getTransId();
                    referenceID = pipe.getRef();
                    trackID = pipe.getTrackId();
                    amount = pipe.getAmt();
                    UDF1 = pipe.getUdf1();
                    UDF2 = pipe.getUdf2();
                    UDF3 = pipe.getUdf3();
                    UDF4 = pipe.getUdf4();
                    UDF5 = pipe.getUdf5();
                    authCode = pipe.getAuth();
                    postDate = pipe.getDate();
                    errorCode = pipe.getError();
                    errorText = pipe.getError_text();
                }
                else
                {
                    errorText = pipe.getError_text();
                }
            }
            else if (Request.Form["ErrorText"] != null)
            {
                paymentID = Request.Form["paymentid"];
                trackID = Request.Form["Trackid"];
                amount = Request.Form["amt"];
                UDF1 = Request.Form["UDF1"];
                UDF2 = Request.Form["UDF2"];
                UDF3 = Request.Form["UDF3"];
                UDF4 = Request.Form["UDF4"];
                UDF5 = Request.Form["UDF5"];
                errorText = Request.Form["ErrorText"];
            }
            else
            {
                errorText = "Unknown Exception";
            }
            // Remove any HTML/CSS/JavaScript from the page. Also, you MUST NOT write anything on the page EXCEPT the word "REDIRECT=" (in upper-case only) followed by a URL.
            // If anything else is written on the page then you will not be able to complete the process.
            if (result == "CAPTURED")
            {
                Response.Write("REDIRECT=https://flowerlink.net/Order/BenefitPayApproved?OrderID=" + OrderID );
            }
            else if (result == "NOT CAPTURED" || result == "CANCELED" || result == "DENIED BY RISK" || result == "HOST TIMEOUT")
            {
                if (result == "NOT CAPTURED")
                {
                    switch (responseCode)
                    {
                        case "05":
                            response = "Please contact issuer";
                            break;
                        case "14":
                            response = "Invalid card number";
                            break;
                        case "33":
                            response = "Expired card";
                            break;
                        case "36":
                            response = "Restricted card";
                            break;
                        case "38":
                            response = "Allowable PIN tries exceeded";
                            break;
                        case "51":
                            response = "Insufficient funds";
                            break;
                        case "54":
                            response = "Expired card";
                            break;
                        case "55":
                            response = "Incorrect PIN";
                            break;
                        case "61":
                            response = "Exceeds withdrawal amount limit";
                            break;
                        case "62":
                            response = "Restricted Card";
                            break;
                        case "65":
                            response = "Exceeds withdrawal frequency limit";
                            break;
                        case "75":
                            response = "Allowable number PIN tries exceeded";
                            break;
                        case "76":
                            response = "Ineligible account";
                            break;
                        case "78":
                            response = "Refer to Issuer";
                            break;
                        case "91":
                            response = "Issuer is inoperative";
                            break;
                        default:
                            // for unlisted values, please generate a proper user-friendly message
                            response = "Unable to process transaction temporarily. Try again later or try using another card.";
                            break;
                    }
                }
                else if (result == "CANCELED")
                {
                    response = "Transaction was canceled by user.";
                }
                else if (result == "DENIED BY RISK")
                {
                    response = "Maximum number of transactions has exceeded the daily limit.";
                }
                else if (result == "HOST TIMEOUT")
                {
                    response = "Unable to process transaction temporarily. Try again later.";
                }
                Response.Write("REDIRECT=https://flowerlink.net/Order/BenefitPayApproved");
            }
            else
            {
                //Unable to process transaction temporarily. Try again later or try using another card.
                Response.Write("REDIRECT=https://flowerlink.net/Order/BenefitPayApproved");
            }
            return View();
        }
        public ActionResult BenefitPayApproved(int OrderID = 0)
        {
            if (OrderID != 0)
            {
                return RedirectToAction("OrderComplete", "Order", new { OrderID = OrderID });
            }
            else
            {
                return RedirectToAction("OrderComplete", "Order", new { OrderNo = "Reject" });
            }
        }

        //[HttpPost]
        //[Route("account/loginregister")]
        public JsonResult LoginRegister(loginBLL data)
        {
            if (data.ContactNo != null)
            {
                data.Register();
                Session["LoginNote"] = "Login Now";
                return Json(new { Status = 1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                data = data.login();
                Session["LoginNote"] = null;
                Session["CustomerID"] = data.CustomerID;
                Session["CustomerEmail"] = data.Email;
                Session["CustomerContactNo"] = data.ContactNo;
                Session["CustomerName"] = string.Concat(data.FirstName, " ", data.LastName);
                Session["IsVerified"] = data.IsVerified;
                if (Convert.ToInt32(Session["IsVerified"]) != 0)
                {
                    if (Session["CustomerEmail"].ToString() != null)
                    {
                        Session["LoginNote"] = "Successfully Login";
                        if (Convert.ToInt32(Session["LoginRoute"]) == 1)
                        {
                            return Json(new { Status = 1 }, JsonRequestBehavior.AllowGet);

                        }
                        else
                        {
                            return Json(new { Status = 1 }, JsonRequestBehavior.AllowGet);


                        }
                    }
                    Session["LoginNote"] = "User is not verified";
                    return Json(new { Status = 1 }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    Session["CustomerName"] = null;
                    Session["LoginNote"] = "Invalid Email or Password";
                    return Json(new { Status = 1 }, JsonRequestBehavior.AllowGet);

                }
            }
        }

        public async Task<ActionResult> CardPaymentApproved( string tap_id)
        {
            if (!string.IsNullOrEmpty(tap_id)) // ???
            {

                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    //RequestUri = new Uri("https://api.tap.company/v2/charges/charge_id"),
                    RequestUri = new Uri("https://api.tap.company/v2/charges/"+ tap_id),
                    Headers =
                {
                    { "accept", "application/json" },
                    { "Authorization",  TapSecretKey},
                },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var BODY_jsonResponse = await response.Content.ReadAsStringAsync();

                    VerificationResponse verificationResponse = JsonConvert.DeserializeObject<VerificationResponse>(BODY_jsonResponse);

                    if (verificationResponse != null && (verificationResponse.Status == "CAPTURED"))
                    {
                        string[] orderInfo = verificationResponse.Reference.Order.Split('|');

                        string CrntOrderNo = orderInfo[0]; // This will be "Hello"
                        int CrntOrderID = int.Parse(orderInfo[1]);
                        return RedirectToAction("OrderComplete", "Order", new { OrderId= CrntOrderID, OrderNo = CrntOrderNo });
                    }
                    else
                        {
                        //ViewBag.Status = verificationResponse.Status;
                            return RedirectToAction("OrderComplete", "Order", new { OrderNo = "Reject" });
                    }
                    
                }
                
            }
            else
            {
                return RedirectToAction("OrderComplete", "Order", new { OrderNo = "Reject" });
            }
        }

private string TapSecretKey = Environment.GetEnvironmentVariable("TAP_SECRET_KEY");

        private async Task<String> ProcessPayment(checkoutBLL model)
        {
            try {

                string currentUrl = Request.Url.GetLeftPart(UriPartial.Path);
                string RedirectUrl = $"{Request.Url.Scheme}://{Request.Url.Authority}/Order/CardPaymentApproved";


                var paymentModel = new PaymentModel
                {
                    Amount = model.GrandTotal,
                    Currency = "BHD",// model.Currency,
                    CustomerInitiated = true,
                    ThreeDSecure = true,
                    SaveCard = false,
                    Description = "",
                    //StatementDescriptor= "",
                    Metadata = new Metadata // Custom data you want to associate with the transaction, like UDFs (user-defined fields)
                    {
                        Udf1 = "",// "test_data_1",
                        Udf2 = "",//"test_data_2",
                        Udf3 = "",//"test_data_3"
                    },
                    Reference = new Reference // Contains transaction and order IDs for tracking.
                    {
                        Transaction = "", // "txn_0001", // ???
                        TraceId =  "txn_0001", // ???
                        Order = $"{model.OrderNo}|{model.OrderID}" // model.OrderNo // "ord_0001" // 
                    },
                    Receipt = new Receipt
                    {
                        Email = true,
                        Sms = true
                    },
                    Customer = new Customer //  Customer details ???
                    {
                        FirstName = "Random", //*
                        MiddleName = "",
                        LastName = "",
                        Email = "test@test.com", //*
                        Phone = new Phone
                        {
                            CountryCode = "973",
                            Number = "00000000"
                        }
                    },
                    Merchant = new Merchant //  Identifies the merchant (your business) making the request.
                    {
                        Id = "44287408"
                    },
                    Source = new Source //* Typically includes the card token or ID generated from the frontend.
                    {
                        Id = "src_all" //"src_card"
                    },
                    AuthorizeDebit = false,
                    //Auto = new Auto
                    //{
                    //    Type = "VOID",
                    //    Time = 100
                    //},
                    Post = new Post // URL for redirection after payment completion.
                    {
                        Url = RedirectUrl 
                    },
                    Redirect = new Redirect // URL for sending a post-payment callback.
                    {
                        Url = RedirectUrl  
                    }
                };

                string jsonContent = JsonConvert.SerializeObject(paymentModel);
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://api.tap.company/v2/charges/"),
                    Headers =
            {
                { "accept", "application/json" },
                { "Authorization", TapSecretKey },
            },
                    Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
                        //Content = new StringContent("{\"amount\":1,\"currency\":\"KWD\",\"customer_initiated\":true,\"threeDSecure\":false,\"save_card\":false,\"description\":\"Test Description\",\"metadata\":{\"udf1\":\"Metadata 1\"},\"reference\":{\"transaction\":\"txn_01\",\"order\":\"ord_01\"},\"receipt\":{\"email\":true,\"sms\":true},\"customer\":{\"first_name\":\"test\",\"middle_name\":\"test\",\"last_name\":\"test\",\"email\":\"test@test.com\",\"phone\":{\"country_code\":965,\"number\":51234567}},\"merchant\":{\"id\":\"1234\"},\"source\":{\"id\":\"src_all\"},\"post\":{\"url\":\"http://your_website.com/post_url\"},\"redirect\":{\"url\":\"http://your_website.com/redirect_url\"}}")

                        {
                            Headers =
                {
                    ContentType = new MediaTypeHeaderValue("application/json")
                }
                    }
                };

                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var BODY_jsonResponse = await response.Content.ReadAsStringAsync();
                    PaymentResponse paymentResponse = JsonConvert.DeserializeObject<PaymentResponse>(BODY_jsonResponse);

                    if (paymentResponse != null && (paymentResponse.Status == "INITIATED"))
                    {
                        return paymentResponse.Transaction.Url;
                    }
                    else
                    {
                        return "";
                    }
                }
            } catch (Exception ex)
            {
                return "";
            }
            return "";
        }


    }
}