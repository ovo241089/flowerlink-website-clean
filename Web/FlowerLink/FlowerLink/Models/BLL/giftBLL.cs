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
    public class giftBLL
    {
        public int GiftID { get; set; }
        public string Title { get; set; }
        public string ArabicTitle { get; set; }
        public string Description { get; set; }
        public double ActualPrice { get; set; }
        public double DisplayPrice { get; set; }
        public double DiscountedPrice { get; set; }
        public bool InStock { get; set; }
        public string Image { get; set; }
        public int StatusID { get; set; }
        public int DisplayOrder { get; set; }
        public int StockQty { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public int Row_Counter { get; set; }

        public static DataTable _dt;
        public static DataSet _ds;

        public List<giftBLL> GetAll()
        {
            try
            {
                var lst = new List<giftBLL>();
                _dt = (new DBHelper().GetTableFromSP)("sp_GiftList");
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        lst = _dt.DataTableToList<giftBLL>();
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}