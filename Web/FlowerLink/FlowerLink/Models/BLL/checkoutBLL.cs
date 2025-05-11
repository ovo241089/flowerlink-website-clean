using FlowerLink.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WebAPICode.Helpers;

namespace FlowerLink.Models.BLL
{
    public class checkoutBLL
    {
        public int PaymentMethodID { get; set; }
        public int OrderID { get; set; }
        public string OrderNo { get; set; }
        public int CustomerID { get; set; }
        public double AmountTotal { get; set; }
        public double GrandTotal { get; set; }
        public double Tax { get; set; }
        public double DeliveryAmount { get; set; }
        public double DiscountAmount { get; set; }
        public int TotalItems { get; set; }
        public int StatusID { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public int CouponID { get; set; }

        //card info
        
        public string Tomsg { get; set; }
        public string Message { get; set; }
        public string Frommsg { get; set; }
        public string Link { get; set; }
        

        /*Cust Order Info*/

        public string CouponCode { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string SenderContact { get; set; }
        public int CustOrderInfoID { get; set; }
        /*public string OrderID { get; set; }*/
        public string Address { get; set; }
        public string NearestPlace { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ContactNo { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public Nullable<System.DateTime> DeliveryTime { get; set; }
        public string CustomerName { get; set; }
        /*public int CustomerID { get; set; }*/
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int? IsIdentitySecret { get; set; }
        public int? IsUnknownAddress { get; set; }
        public string PlaceType { get; set; }
        public string Email { get; set; }
        public string CardNotes { get; set; }
        public string SelectedTime { get; set; }
        public List<OrderDetails> OrderDetail = new List<OrderDetails>();
        public string OrderDetailString { get; set; }

        public List<OrderGiftDetails> OrderGifts = new List<OrderGiftDetails>();
        public string OrderGiftsString { get; set; }
        
        /*Order Details*/
        public class OrderDetails
        {
            public int OrderDetailID { get; set; }
            public int OrderID { get; set; }
            public int ItemID { get; set; }
            public string Title { get; set; }
            public string Image { get; set; }
            public int GiftID { get; set; }
            public int Qty { get; set; }
            public double Price { get; set; }
            public double Cost { get; set; }
            public double DiscountAmount { get; set; }
            public Nullable<System.DateTime> LastUpdatedDate { get; set; }
            public int LastUpdatedBy { get; set; }
            public int DealID { get; set; }
            public int Key { get; set; }
        }
        /*Order Details*/
        public class OrderGiftDetails
        {
            public int OrderDetailID { get; set; }  
            public int ItemID { get; set; }
            public string Title { get; set; }
            public string Image { get; set; }
            public int GiftID { get; set; }
            public int Quantity { get; set; }
            public double DisplayPrice { get; set; }
            public double Cost { get; set; }
            public double DiscountAmount { get; set; }
            public Nullable<System.DateTime> LastUpdatedDate { get; set; }
            public int LastUpdatedBy { get; set; }
            public int ItemKey { get; set; }
            
        }
        public int InsertOrder(checkoutBLL data)
        {
            
            try
            {
                int rtn = 0;

                SqlParameter[] p = new SqlParameter[32];
                //ORDER MASTER
                p[0] = new SqlParameter("@OrderNo", data.OrderNo);
                if (data.CustomerID == 0)
                {
                    p[1] = new SqlParameter("@CustomerID", DBNull.Value);
                }
                else
                {
                    p[1] = new SqlParameter("@CustomerID", data.CustomerID);
                }
               
                if (data.PaymentMethodID == 1)
                {
                    p[2] = new SqlParameter("@StatusID", 101);
                }
                else
                {
                    p[2] = new SqlParameter("@StatusID", 103);
                }
                p[3] = new SqlParameter("@OrderDate", data.OrderDate);
                p[4] = new SqlParameter("@LastUpdatedDate", data.LastUpdatedDate);
                p[5] = new SqlParameter("@LastUpdatedBy", data.LastUpdatedBy);
                p[6] = new SqlParameter("@CouponID", data.CouponID);
                
                p[7] = new SqlParameter("@AmountTotal", data.GrandTotal);
                p[8] = new SqlParameter("@GrandTotal", data.GrandTotal);
                p[9] = new SqlParameter("@Tax", data.Tax);
                p[10] = new SqlParameter("@DiscountAmount", data.DiscountAmount);
                p[11] = new SqlParameter("@DeliveryAmount",data.DeliveryAmount);
                p[12] = new SqlParameter("@TotalItems", data.TotalItems);

                //CUSTOMER ORDER INFO
                p[13] = new SqlParameter("@Address", data.Address);
                p[14] = new SqlParameter("@NearestPlace", data.NearestPlace);
                p[15] = new SqlParameter("@Country", data.Country);
                p[16] = new SqlParameter("@City", data.City);
                p[17] = new SqlParameter("@ContactNo", data.ContactNo);
                p[18] = new SqlParameter("@DeliveryDate", data.DeliveryDate);
                p[19] = new SqlParameter("@SelectedTime", data.SelectedTime);
                p[20] = new SqlParameter("@CustomerName", data.CustomerName);
                p[21] = new SqlParameter("@Latitude", data.Latitude);
                p[22] = new SqlParameter("@Longitude", data.Longitude);
                p[23] = new SqlParameter("@PlaceType", data.PlaceType);
                p[24] = new SqlParameter("@Email", data.Email);
                p[25] = new SqlParameter("@CardNotes", data.CardNotes);

                p[26] = new SqlParameter("@SenderName", data.SenderName);
                p[27] = new SqlParameter("@SenderEmail", data.SenderEmail); 
                p[28] = new SqlParameter("@SenderContact", data.SenderContact);
                p[29] = new SqlParameter("@CouponCode", data.CouponCode);
                p[30] = new SqlParameter("@IsIdentitySecret", data.IsIdentitySecret);
                p[31] = new SqlParameter("@IsUnknownAddress", data.IsUnknownAddress);
                int OrderID = int.Parse(new DBHelper().GetTableFromSP("sp_InsertOrder_v2", p).Rows[0]["ID"].ToString());
                rtn = OrderID;
                //Payment 
                SqlParameter[] pay = new SqlParameter[14];
                pay[0] = new SqlParameter("@OrderID", OrderID);
                pay[1] = new SqlParameter("@PaymentMethodID", data.PaymentMethodID);
                pay[2] = new SqlParameter("@PaymentStatus", DBNull.Value);
                pay[3] = new SqlParameter("@CardType", DBNull.Value);                
                pay[4] = new SqlParameter("@Amount", data.GrandTotal);
                pay[5] = new SqlParameter("@DiscountAmount", DBNull.Value);
                pay[6] = new SqlParameter("@CardNo", DBNull.Value);
                pay[7] = new SqlParameter("@CVC", DBNull.Value);
                pay[8] = new SqlParameter("@ExpiryDate", DBNull.Value);
                pay[9] = new SqlParameter("@RefNo", DBNull.Value);
                pay[10] = new SqlParameter("@CreationDate", DBNull.Value);
                pay[11] = new SqlParameter("@CreationID", DBNull.Value);
                pay[12] = new SqlParameter("@UpdatedDate", DBNull.Value);
                pay[13] = new SqlParameter("@UpdatedID", DBNull.Value);
                (new DBHelper().ExecuteNonQueryReturn)("sp_InsertPayment", pay);

                //Card 
                SqlParameter[] card = new SqlParameter[8];
                card[0] = new SqlParameter("@OrderID", OrderID);
                if (data.CustomerID == 0)
                {
                    card[1] = new SqlParameter("@CustomerID", DBNull.Value);
                }
                else
                {
                    card[1] = new SqlParameter("@CustomerID", data.CustomerID);
                }
                card[2] = new SqlParameter("@Tomsg", data.Tomsg);
                card[3] = new SqlParameter("@Message", data.Message);
                card[4] = new SqlParameter("@Frommsg", data.Frommsg);
                card[5] = new SqlParameter("@Link", data.Link);
                card[6] = new SqlParameter("@StatusID", 1);
                card[7] = new SqlParameter("@CreatedDate", DateTime.Now);                              
                (new DBHelper().ExecuteNonQueryReturn)("sp_InsertCardPrint", card);

                try
                {
                    int OrderDetailID = 0;
                    foreach (var item in data.OrderDetail)
                    {
                        SqlParameter[] para = new SqlParameter[8];
                        para[0] = new SqlParameter("@OrderID", OrderID);//Hard Coded Value Pass
                        para[1] = new SqlParameter("@ItemID", item.ItemID);
                        para[2] = new SqlParameter("@GiftID", DBNull.Value);//Help required here 
                        para[3] = new SqlParameter("@Quantity", item.Qty);
                        para[4] = new SqlParameter("@Price", item.Price);
                        /*para[5] = new SqlParameter("@Cost", item.Cost); 
                        para[6] = new SqlParameter("@DiscountAmount", item.DiscountAmount);*/
                        para[5] = new SqlParameter("@LastUpdatedDate", item.LastUpdatedDate);
                        para[6] = new SqlParameter("@LastUpdatedBy", item.LastUpdatedBy);
                        para[7] = new SqlParameter("@DealID", item.DealID);
                        OrderDetailID = int.Parse(new DBHelper().GetTableFromSP("sp_OrderDetails", para).Rows[0]["ID"].ToString());

                        var giftList = data.OrderGifts.Where(x => x.ItemKey == item.Key).ToList();
                        if (giftList.Count > 0)
                        {
                            foreach (var i in giftList)
                            {
                                SqlParameter[] para1 = new SqlParameter[7];
                                para1[0] = new SqlParameter("@OrderDetailID", OrderDetailID);
                                para1[1] = new SqlParameter("@ItemID", i.ItemID);
                                para1[2] = new SqlParameter("@GiftID", i.GiftID);
                                para1[3] = new SqlParameter("@Quantity", i.Quantity);   
                                para1[4] = new SqlParameter("@DisplayPrice", i.DisplayPrice);
                                /* para[5] = new SqlParameter("@Cost", item.Cost);
                                para[6] = new SqlParameter("@DiscountAmount", item.DiscountAmount);*/
                                para1[5] = new SqlParameter("@LastUpdatedDate", i.LastUpdatedDate);
                                para1[6] = new SqlParameter("@LastUpdatedBy", i.LastUpdatedBy);
                                (new DBHelper().ExecuteNonQueryReturn)("sp_OrderGiftDetails", para1);

                            }
                        }
                    }
                  
                }
                catch (Exception ex)
                {
                }
                return rtn;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int OrderUpdate(int OrderID,int StatusID)
        {
            try
            {
                int rtn = 0;
                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@OrderID", OrderID);
                p[1] = new SqlParameter("@StatusID", StatusID);
                rtn = (new DBHelper().ExecuteNonQueryReturn)("sp_OrderReject", p);

                return rtn;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}