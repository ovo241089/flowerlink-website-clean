using BAL.Repositories;
using FlowerLinkAdmin._Models;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FlowerLinkAdmin.BLL._Services
{
    public class ordersService : baseService
    {
        ordersDB _service;
        public ordersService()
        {
            _service = new ordersDB();
        }

        public List<OrdersBLL> GetAll(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                return _service.GetAll(FromDate, ToDate);
            }
            catch (Exception ex)
            {
                return new List<OrdersBLL>();
            }
        }

        public RspOrderDetail Get(int id)
        {
            try
            {
                RspOrderDetail rsp = new RspOrderDetail();
                var lstOD = new List<OrderDetailBLL>();
                var lstODG = new List<OrderDetailBLL>();
                var lstODM = new List<OrderModifiersBLL>();
                var oc = new OrderCheckoutBLL();
                var ocustomer = new OrderCustomerBLL();
                var bll = new List<OrdersBLL>();
                var ds = _service.Get(id);
                var _dsOrders = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<OrdersBLL>>();
                var _dsorderdetail = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[1])).ToObject<List<OrderDetailBLL>>();
                var _dsorderpayment = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[2])).ToObject<List<OrderCheckoutBLL>>();

                var _dsOrderCustomerData = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[3])).ToObject<List<OrderCustomerBLL>>();
                var _dsorderdetailgift = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[4])).ToObject<List<OrderDetailBLL>>();
                foreach (var i in _dsOrders)
                {
                    lstOD = new List<OrderDetailBLL>();
                    foreach (var j in _dsorderdetail.Where(x => x.OrderID == i.OrderID))
                    {
                        lstODG = new List<OrderDetailBLL>();
                        foreach (var k in _dsorderdetailgift.Where(x => x.OrderDetailID == j.OrderDetailID))
                        {
                            lstODG.Add(new OrderDetailBLL
                            {
                                StatusID = k.StatusID,
                                Cost = k.Cost,
                                Price = k.Price,
                                Quantity = k.Quantity,
                                OrderDetailID = k.OrderDetailID,
                                LastUpdateDT = k.LastUpdateDT,
                                LastUpdateBy = k.LastUpdateBy,
                                ItemID = k.ItemID,
                                Title = k.Title,
                                Image = k.Image,
                                //OrderDetailModifiers = lstODM,
                                OrderID = k.OrderID,
                                OrderMode = k.OrderMode,
                                OrderDetailGifts = null

                            });
                        }
                        lstOD.Add(new OrderDetailBLL
                        {
                            StatusID = j.StatusID,
                            Cost = j.Cost,
                            Price = j.Price,
                            Quantity = j.Quantity,
                            OrderDetailID = j.OrderDetailID,
                            LastUpdateDT = j.LastUpdateDT,
                            LastUpdateBy = j.LastUpdateBy,
                            ItemID = j.ItemID,
                            Title = j.Title,
                            Image = j.Image,
                            //OrderDetailModifiers = lstODM,
                            OrderID = j.OrderID,
                            OrderMode = j.OrderMode,
                            OrderDetailGifts = lstODG
                        });
                    }

                    var PayBy = _dsorderpayment.FirstOrDefault() == null ? 0 : _dsorderpayment.FirstOrDefault().PaymentMethodID;
                    bll.Add(new OrdersBLL
                    {
                        StatusID = i.StatusID,
                        LastUpdatedDate = i.LastUpdatedDate,
                        LastUpdatedBy = i.LastUpdatedBy,
                        OrderID = i.OrderID,
                        CustomerID = i.CustomerID,
                        OrderDate = i.OrderDate,
                        OrderNo = i.OrderNo,
                        AmountTotal = i.AmountTotal,
                        CouponID = i.CouponID,
                        DeliveryAmount = i.DeliveryAmount,
                        DiscountAmount = i.DiscountAmount,
                        GrandTotal = i.GrandTotal,
                        Tax = i.Tax,
                        TotalItems = i.TotalItems,
                        PaymentMethod = PayBy == 1 ? "Cash on delivery" : PayBy == 2 ? "Credimax Payment" : PayBy == 4 ? "Benifit Pay" : PayBy == 3 ? "Pay Pal" : "Cash"
                    });

                    rsp.Order = bll.FirstOrDefault();
                    rsp.OrderDetails = lstOD;
                    rsp.OrderCheckouts = _dsorderpayment.Where(x => x.OrderID == i.OrderID).FirstOrDefault();
                    rsp.CustomerOrders = _dsOrderCustomerData.Where(x => x.OrderID == i.OrderID).FirstOrDefault();
                }

                return rsp;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int Insert(OrdersBLL data, IWebHostEnvironment _env)
        {
            try
            {
                //data.Image = UploadImage(data.Image, "Orders", _env);
                //data.LastUpdatedDate = _UTCDateTime_SA();
                //var result = _service.Insert(data);
                return 0;
                //return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int UpdateInProcess(OrdersBLL obj, IWebHostEnvironment _env, string Body)
        {
            string msg = "";
            msg = obj.StatusMsg;
            string contentRootPath = _env.ContentRootPath;

            string path = "/ClientApp/dist/assets/Upload/";
            string filePath = contentRootPath + path;

            try
            {
                var data = Get(obj.OrderID);

                string ToEmail, SubJect;
                ToEmail = data.CustomerOrders.SenderEmail;
                SubJect = "Your order on FlowerLink - " + data.Order.OrderNo;

                Body = Body.Replace("#SenderName#", data.CustomerOrders.SenderName);
                SendEmail("Flowerlink || Order: " + data.Order.OrderNo, Body, data.CustomerOrders.SenderEmail);
            }
            catch { }
            return 1;
        }
        public int UpdatePaymentFailed(OrdersBLL obj, IWebHostEnvironment _env, string Body)
        {
            string msg = "";
            msg = obj.StatusMsg;
            string contentRootPath = _env.ContentRootPath;

            string path = "/ClientApp/dist/assets/Upload/";
            string filePath = contentRootPath + path;

            try
            {
                var data = Get(obj.OrderID);

                string ToEmail, SubJect;
                ToEmail = data.CustomerOrders.SenderEmail;
                SubJect = "Your order on FlowerLink - " + data.Order.OrderNo;

                Body = Body.Replace("#SenderName#", data.CustomerOrders.SenderName);
                SendEmail("Flowerlink || Order: " + data.Order.OrderNo, Body, data.CustomerOrders.SenderEmail);
            }
            catch { }
            return 1;
        }
        public int UpdateAcceptOrder(OrdersBLL obj, IWebHostEnvironment _env, string Body)
        {
            string msg = "";
            msg = obj.StatusMsg;
            string contentRootPath = _env.ContentRootPath;

            string path = "/ClientApp/dist/assets/Upload/";
            string filePath = contentRootPath + path;

            try
            {
                var data = Get(obj.OrderID);

                string ToEmail, SubJect, cc, Bcc;
                ToEmail = data.CustomerOrders.SenderEmail;
                SubJect = "Your order on FlowerLink - " + data.Order.OrderNo;

                string items = "";

                foreach (var item in data.OrderDetails)
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
                        + "<td style = 'padding:0px;margin:0px;border-spacing:0;' ><table class='wrapper' role='module' data-type='image' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='239f10b7-5807-4e0b-8f01-f2b8d25ec9d7'>"
                        + "<tbody>"
                        + "<tr>"
                        + "<td style = 'font-size:6px; line-height:10px; padding:0px 0px 0px 0px;' valign='top' align='left'>"
                        + "<img src = '" + filePath + "/Item/" + item.Image + "' class='max-width' border='0' style='display:block;width: 108px;height: 108px;object-fit: contain;' alt='' >"
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
                        + "<div style = 'font-family: inherit; text-align: inherit'> " + item.Title + "</div>"
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

                        string gifts = "";

                        foreach (var gift in item.OrderDetailGifts)
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
                                + "<img src = '" + filePath + "/Gift/" + gift.Image + "' class='max-width' border='0' style='display:block;width: 108px;height: 108px;object-fit: contain;' alt='' >"
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
                                + "<div style = 'font-family: inherit; text-align: inherit'> " + gift.Title + "</div>"
                                + "<div style = 'font-family: inherit; text-align: inherit'> Qty : " + gift.Quantity + "</div>"
                                + "<div style = 'font-family: inherit; text-align: inherit'><span style='color: #000263'>BD " + gift.Price + "</span></div>"
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
                    catch (Exception ex)
                    {
                    }
                }


                Body = Body.Replace("#OrderNo#", data.Order.OrderNo.ToString());
                Body = Body.Replace("#items#", items.ToString());
                Body = Body.Replace("#ReceiverName#", data.CustomerOrders.CustomerName);
                Body = Body.Replace("#SenderName#", data.CustomerOrders.SenderName);
                Body = Body.Replace("#SelectedTime#", data.CustomerOrders.SelectedTime);
                Body = Body.Replace("#DeliveryDate#", Convert.ToDateTime(data.CustomerOrders.DeliveryDate).ToString("dd/MMM/yyyy"));
                Body = Body.Replace("#OrderDate#", Convert.ToDateTime(data.Order.OrderDate).ToString("dd/MMM/yyyy"));
                Body = Body.Replace("#Address#", data.CustomerOrders.Address.ToString());
                Body = Body.Replace("#TotalItems#", data.OrderDetails.Count.ToString());
                Body = Body.Replace("#SubTotal#", data.Order.AmountTotal.ToString());
                Body = Body.Replace("#Tax#", data.Order.Tax.ToString());
                Body = Body.Replace("#DeliveryAmount#", data.Order.DeliveryAmount.ToString());
                Body = Body.Replace("#GrandTotal#", data.Order.GrandTotal.ToString());
                Body = Body.Replace("#PaymentType#", data.Order.PaymentMethod);
                //Body = Body.Replace("#statusmsg#", msg);
                SendEmail("Flowerlink || Order: " + data.Order.OrderNo, Body, data.CustomerOrders.SenderEmail);
                //data.CustomerOrders.SenderEmail
                SendEmail("Order Status Update - " + data.Order.OrderNo, Body, "info@flowerlink.net");
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }
        public int UpdateDeliveryFailed(OrdersBLL obj, IWebHostEnvironment _env, string Body)
        {
            string msg = "";
            msg = obj.StatusMsg;
            string contentRootPath = _env.ContentRootPath;

            string path = "/ClientApp/dist/assets/Upload/";
            string filePath = contentRootPath + path;

            try
            {
                var data = Get(obj.OrderID);

                string ToEmail, SubJect;
                ToEmail = data.CustomerOrders.SenderEmail;
                SubJect = "Your order on FlowerLink - " + data.Order.OrderNo;

                Body = Body.Replace("#SenderName#", data.CustomerOrders.SenderName);
                SendEmail("Flowerlink || Order: " + data.Order.OrderNo, Body, data.CustomerOrders.SenderEmail);
            }
            catch { }
            return 1;
        }

        public int Update(OrdersBLL obj, IWebHostEnvironment _env)
        {
            try
            {
                string contentRootPath = _env.ContentRootPath;

                string path = "/ClientApp/dist/assets/Upload/";
                string filePath = contentRootPath + path;

                string Body = "";
                if (obj.StatusID == 102 || obj.StatusID == 100)
                {
                    Body = System.IO.File.ReadAllText(contentRootPath + "\\Template\\orderstatus.txt");
                    UpdateAcceptOrder(obj, _env, Body);
                }
                else if (obj.StatusID == 101)
                {
                    Body = System.IO.File.ReadAllText(contentRootPath + "\\Template\\inprocess.txt");
                    UpdateInProcess(obj, _env, Body);
                }
                else if (obj.StatusID == 104)
                {
                    Body = System.IO.File.ReadAllText(contentRootPath + "\\Template\\payment-failed.txt");
                    UpdatePaymentFailed(obj, _env, Body);
                }
                else if (obj.StatusID == 105)
                {
                    Body = System.IO.File.ReadAllText(contentRootPath + "\\Template\\delivery-failed.txt");
                    UpdateDeliveryFailed(obj, _env, Body);
                }

                obj.LastUpdatedDate = _UTCDateTime_SA();
                var result =_service.Update(obj);

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Delete(OrdersBLL data)
        {
            try
            {
                data.LastUpdatedDate = _UTCDateTime_SA();
                var result = _service.Delete(data);

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public void SendEmail(string _SubjectEmail, string _BodyEmail, string _To)
        {
            try
            {
                using (System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage())
                {
                    mail.From = new MailAddress("info@flowerlink.net", "Flower Link");
                    mail.To.Add(_To);
                    mail.Subject = _SubjectEmail;
                    mail.Body = _BodyEmail;
                    mail.IsBodyHtml = true;
                    using (SmtpClient smtp = new SmtpClient("mail.flowerlink.net", 25))
                    {
                        smtp.Credentials = new NetworkCredential("info@flowerlink.net", "Fl0w3rl!nk@123");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
