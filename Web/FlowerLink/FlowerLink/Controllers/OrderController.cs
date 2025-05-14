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
using System.Threading.Tasks;
using FlowerLink.Models.TapPayment;
using System.Net.Http;
using FlowerLink.Models.ViewModels;

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
                SendOrderConfirmationEmail(data);
                ViewBag.OrderNo = data.OrderNo;
            }

            return View();
        }

        private void SendOrderConfirmationEmail(myorderBLL.OrderMaster data)
        {
            try
            {
                string ToEmail = data.SenderEmail;
                string SubJect = "[FlowerLink] ORDER RECEIVED : " + data.OrderNo;
                string Body = System.IO.File.ReadAllText(Server.MapPath("~/Template") + "\\" + "emailpattern.txt");
                string items = "";
                foreach (var item in data.OrderDetail)
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
                string gifts = "";
                if (data.GiftDetail.Count > 0)
                {
                    foreach (var item in data.GiftDetail)
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
                Body = Body.Replace("#Address#", data.Address?.ToString() ?? "Not Defined");
                string PaymentType = (data.PaymentMethodID != 1) ? "Online Payment" : "Cash On Delivery";
                Body = Body.Replace("#PaymentType#", PaymentType);
                Body = Body.Replace("#PaymentMethod#", data.PaymentMethodTitle.ToString());
                Body = Body.Replace("#TotalItems#", data.TotalItems.ToString());
                Body = Body.Replace("#SubTotal#", data.AmountTotal.ToString());
                Body = Body.Replace("#Tax#", data.Tax.ToString());
                Body = Body.Replace("#DeliveryAmount#", data.DeliveryAmount.ToString());
                Body = Body.Replace("#GrandTotal#", data.GrandTotal.ToString());

                string cc = "";
                string Bcc = ConfigurationManager.AppSettings["From"].ToString();
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
                ViewBag.Status = "Error sending email: " + ex.Message; // Log or handle error
            }
        }

        private void SendOrderStatusUpdateEmail(int orderId, string newStatus)
        {
            try
            {
                var orderBLL = new myorderBLL();
                var orderDetails = orderBLL.GetDetails(orderId);

                if (orderDetails == null || string.IsNullOrEmpty(orderDetails.SenderEmail))
                {
                    // Log: Cannot send email, order details or email not found.
                    return;
                }

                string toEmail = orderDetails.SenderEmail;
                string subject = $"[FlowerLink] Order Status Update: {orderDetails.OrderNo} is now {newStatus}";
                
                // Basic Email Body - Consider creating a new template or enhancing the existing one.
                string body = $"<p>Dear {orderDetails.SenderName},</p>"
                            + $"<p>The status of your FlowerLink order <strong>{orderDetails.OrderNo}</strong> has been updated to: <strong>{newStatus}</strong>.</p>"
                            + $"<p>Order Date: {orderDetails.OrderDate:dd/MMM/yyyy}</p>"
                            + $"<p>You can view your order details and complete history here: <a href='{Url.Action("TrackOrder", "Order", new { orderId = orderDetails.OrderID }, Request.Url.Scheme)}'>Track Your Order</a></p>"
                            + $"<p>Thank you for choosing FlowerLink!</p>";

                string cc = "";
                string bcc = ConfigurationManager.AppSettings["From"].ToString(); // Optional: BCC admin

                WebMail.SmtpServer = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                WebMail.SmtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"].ToString());
                WebMail.SmtpUseDefaultCredentials = true;
                WebMail.EnableSsl = false;
                WebMail.UserName = ConfigurationManager.AppSettings["UserName"].ToString();
                WebMail.From = ConfigurationManager.AppSettings["From"].ToString();
                WebMail.Password = ConfigurationManager.AppSettings["Password"].ToString();
                WebMail.Send(to: toEmail, subject: subject, body: body, cc: cc, bcc: bcc, isBodyHtml: true);
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using a logging library or System.Diagnostics.Trace)
                // Consider how to handle email sending failures, e.g., retry mechanism or admin notification.
                Console.WriteLine($"Error sending status update email for OrderID {orderId}: {ex.Message}");
            }
        }


        public JsonResult Coupon(string coupon)
        {
            couponBLL couponBLL = new couponBLL();
            ViewBag.coupon = couponBLL.Get(coupon);
            return Json(new { data = ViewBag.coupon }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> PunchOrder(checkoutBLL data)
        {
            checkoutBLL _service = new checkoutBLL();
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(JArray.Parse(data.OrderDetailString));
            JArray jsonResponse = JArray.Parse(json);
            data.OrderDetail = jsonResponse.ToObject<List<OrderDetails>>();

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
                    var client = new RestClient(ConfigurationManager.AppSettings["CrediMaxAPI"].ToString());
                    var request = new RestRequest("", Method.Post);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Authorization", ConfigurationManager.AppSettings["CrediMaxKey"].ToString());
                    var body = new Root();
                    body.apiOperation = "CREATE_CHECKOUT_SESSION";
                    body.order = new Order();
                    body.order.id = LastOrderID.ToString();
                    body.order.amount = data.GrandTotal.ToString();
                    body.order.currency = "BHD";
                    body.interaction = new Interaction();
                    body.interaction.operation = "PURCHASE";
                    body.interaction.returnUrl = ConfigurationManager.AppSettings["CurrentURL"].ToString() + "/Order/OrderComplete?OrderID=" + LastOrderID;
                    body.interaction.cancelUrl = ConfigurationManager.AppSettings["CurrentURL"].ToString() + "/Order/OrderComplete?OrderID=" + LastOrderID + "&OrderNo=Reject";
                    body.interaction.merchant = new Merchant();
                    body.interaction.merchant.name = "FlowerLink";
                    var jsonbody = new JavaScriptSerializer().Serialize(body);
                    request.AddParameter("application/json", jsonbody, ParameterType.RequestBody);
                    RestResponse response = await client.ExecuteAsync(request);
                    details = JsonConvert.DeserializeObject<PaymentDetails>(response.Content);
                    return Json(new { data = details, orderID = LastOrderID }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { data = details, orderID = LastOrderID }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (data.PaymentMethodID == 3)//Tap Payment
            {
                int LastOrderID = rtn;
                try
                {
                    string apiKey = ConfigurationManager.AppSettings["TapSecretKey"].ToString(); 
                    string tapApiUrl = ConfigurationManager.AppSettings["TapAPIUrl"].ToString();

                    var paymentData = new TapPaymentRequest
                    {
                        amount = data.GrandTotal,
                        currency = "BHD",
                        threeDSecure = true,
                        save_card = false,
                        description = "FlowerLink Order #" + LastOrderID,
                        statement_descriptor = "FlowerLink",
                        metadata = new Dictionary<string, string>
                        {
                            { "order_id", LastOrderID.ToString() }
                        },
                        receipt = new Receipt
                        {
                            email = true,
                            sms = true
                        },
                        customer = new Customer
                        {
                            first_name = data.SenderName, 
                            email = data.SenderEmail,
                            phone = new Phone
                            {
                                country_code = "973", 
                                number = data.SenderContact
                            }
                        },
                        source = new Source { id = "src_all" }, 
                        post = new Post { url = ConfigurationManager.AppSettings["CurrentURL"].ToString() + "/Order/TapPaymentCallback" }, 
                        redirect = new Redirect { url = ConfigurationManager.AppSettings["CurrentURL"].ToString() + "/Order/TapPaymentCallback" } 
                    };

                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                        var jsonRequest = JsonConvert.SerializeObject(paymentData);
                        var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                        var response = await httpClient.PostAsync(tapApiUrl, httpContent);
                        var responseString = await response.Content.ReadAsStringAsync();
                        var tapResponse = JsonConvert.DeserializeObject<TapPaymentResponse>(responseString);

                        if (tapResponse != null && tapResponse.transaction != null && !string.IsNullOrEmpty(tapResponse.transaction.url))
                        {
                            return Json(new { data = new { session = new { id = tapResponse.id }, redirectUrl = tapResponse.transaction.url }, orderID = LastOrderID }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { data = new { error = "Tap Payment initiation failed", details = tapResponse }, orderID = LastOrderID }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { data = new { error = ex.Message }, orderID = LastOrderID }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                // For non-online payments (e.g., Cash on Delivery), send confirmation email here
                if (rtn > 0) // Check if order insertion was successful
                {
                    var orderDataForEmail = new myorderBLL().GetDetails(rtn);
                    if (orderDataForEmail != null) SendOrderConfirmationEmail(orderDataForEmail);
                }
                return Json(new { data = "Success", orderID = rtn }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost] 
        public async Task<ActionResult> TapPaymentCallback()
        {
            try
            {
                string chargeId = Request.QueryString["tap_id"]; 

                if (string.IsNullOrEmpty(chargeId))
                {
                    TempData["ErrorMessage"] = "Tap Payment callback received without a charge ID.";
                    return RedirectToAction("Checkout"); 
                }
                
                string apiKey = ConfigurationManager.AppSettings["TapSecretKey"].ToString();
                string tapRetrieveChargeUrl = $"https://api.tap.company/v2/charges/{chargeId}";
                
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                    var response = await httpClient.GetAsync(tapRetrieveChargeUrl);
                    var responseString = await response.Content.ReadAsStringAsync();
                    var tapCharge = JsonConvert.DeserializeObject<TapChargeVerificationResponse>(responseString);

                    if (tapCharge != null && tapCharge.status == "CAPTURED") 
                    {
                        int orderIdFromMetadata = 0;
                        if (tapCharge.metadata != null && tapCharge.metadata.TryGetValue("order_id", out string orderIdStr))
                        {
                            int.TryParse(orderIdStr, out orderIdFromMetadata);
                        }

                        if (orderIdFromMetadata > 0)
                        {
                            // Payment is successful and verified, proceed to OrderComplete
                            // Order confirmation email is sent from OrderComplete after status update
                            return RedirectToAction("OrderComplete", new { OrderID = orderIdFromMetadata });
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Tap Payment verification failed: Order ID mismatch.";
                            return RedirectToAction("Checkout"); 
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Tap Payment verification failed. Status: " + (tapCharge?.status ?? "Unknown");
                        return RedirectToAction("Checkout"); 
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred during Tap Payment processing: " + ex.Message;
                return RedirectToAction("Checkout"); 
            }
        }

        // ***** START: Order Tracking Actions & API Endpoints *****

        public ActionResult TrackOrder()
        {
            ViewBag.Banner = new bannerBLL().GetBanner("TrackOrder"); 
            return View(); 
        }

        [Authorize] 
        public ActionResult OrderHistory()
        {
            if (Session["CustomerID"] == null)
            {
                return RedirectToAction("Login_Register", "Account", new { id = 1 }); 
            }
            int customerId = Convert.ToInt32(Session["CustomerID"]);
            ViewBag.Banner = new bannerBLL().GetBanner("OrderHistory"); 
            return View(); 
        }


        [HttpGet]
        public JsonResult GetOrderStatus(int orderId, string email = null) 
        {
            var orderBLL = new myorderBLL(); 
            var orderDetails = orderBLL.GetDetails(orderId); 

            if (orderDetails == null)
            {
                return Json(new { success = false, message = "Order not found." }, JsonRequestBehavior.AllowGet);
            }

            bool authorized = false;
            if (Session["CustomerID"] != null)
            {
                int sessionCustomerId = Convert.ToInt32(Session["CustomerID"]);
                if (orderDetails.CustomerID == sessionCustomerId) 
                {
                    authorized = true;
                }
            }
            else if (!string.IsNullOrEmpty(email))
            {
                if (string.Equals(orderDetails.SenderEmail, email, StringComparison.OrdinalIgnoreCase))
                {
                    authorized = true;
                }
            }

            if (!authorized)
            {
                return Json(new { success = false, message = "Order not found or access denied." }, JsonRequestBehavior.AllowGet);
            }
            
            var statusViewModel = new OrderStatusViewModel 
            {
                OrderID = orderDetails.OrderID,
                OrderNo = orderDetails.OrderNo,
                OrderDate = orderDetails.OrderDate, 
                CurrentStatus = orderDetails.OrderStatusName, 
                TotalAmount = orderDetails.GrandTotal,
                Items = orderDetails.OrderDetail?.Select(item => new OrderItemViewModel 
                                                                { 
                                                                    ProductName = item.ItemTitle, 
                                                                    Quantity = item.Quantity, 
                                                                    UnitPrice = item.Price,
                                                                    TotalPrice = item.Quantity * item.Price 
                                                                }).ToList() ?? new List<OrderItemViewModel>(),
                History = new List<OrderStatusHistoryViewModel> 
                {
                    new OrderStatusHistoryViewModel { Status = "Placed", Timestamp = orderDetails.OrderDate, Notes = "Order successfully placed." },
                    new OrderStatusHistoryViewModel { Status = orderDetails.OrderStatusName, Timestamp = DateTime.Now, Notes = "Current status." } 
                }
            };

            return Json(new { success = true, data = statusViewModel }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize] 
        public JsonResult GetCustomerOrderHistory()
        {
            if (Session["CustomerID"] == null)
            {
                return Json(new { success = false, message = "User not logged in." }, JsonRequestBehavior.AllowGet);
            }
            int customerId = Convert.ToInt32(Session["CustomerID"]);

            var orderBLL = new myorderBLL();
            var customerOrders = orderBLL.GetMyOrders(customerId); 

            if (customerOrders == null || customerOrders.Count == 0)
            {
                return Json(new { success = true, data = new List<object>(), message = "No orders found for this customer." }, JsonRequestBehavior.AllowGet);
            }

            var orderHistoryViewModel = customerOrders.Select(order => new 
            {
                order.OrderID,
                order.OrderNo,
                OrderDate = order.OrderDate.ToString("yyyy-MM-dd HH:mm"), 
                Status = order.OrderStatusName, 
                order.GrandTotal
            }).ToList();

            return Json(new { success = true, data = orderHistoryViewModel }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateOrderStatus(OrderStatusUpdateModel statusUpdate) 
        {
            if (Session["UserType"] == null || Session["UserType"].ToString() != "Admin") 
            {
                return Json(new { success = false, message = "Unauthorized action." });
            }

            if (statusUpdate == null || statusUpdate.OrderID <= 0 || statusUpdate.NewStatusID <= 0)
            {
                return Json(new { success = false, message = "Invalid data provided for status update." });
            }

            checkoutBLL check = new checkoutBLL();
            bool success = check.OrderUpdate(statusUpdate.OrderID, statusUpdate.NewStatusID); 

            if (success)
            {
                // Get the string representation of the new status
                // This requires a method to map NewStatusID to status name, e.g., from a BLL or lookup table
                string newStatusName = check.GetOrderStatusNameById(statusUpdate.NewStatusID); // Assuming this method exists
                if (string.IsNullOrEmpty(newStatusName)) newStatusName = "Updated"; // Fallback

                SendOrderStatusUpdateEmail(statusUpdate.OrderID, newStatusName);
                return Json(new { success = true, message = "Order status updated successfully." });
            }
            else
            {
                return Json(new { success = false, message = "Failed to update order status." });
            }
        }

        // ***** END: Order Tracking Actions & API Endpoints *****

    }
}

