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
    public class flashSaleJuncBLL
    {
        public int FlashSaleItemID { get; set; }
        public string Title { get; set; }
        public int? FlashSaleID { get; set; }
        public int? ItemID { get; set; }
        public double? Price { get; set; }
        public double? DiscountPercent { get; set; }
        public double? DiscountedPrice { get; set; }
        public double? DiscountAmount { get; set; }
        public string Image { get; set; }
        public string HoveredImage { get; set; }

    }
    public class flashSaleBLL
        {
            public int FlashSaleID { get; set; }
            public string Title { get; set; }          
            public int StatusID { get; set; }
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }

            public DateTime? CreatedDate { get; set; }

            public DateTime? UpdatedDate { get; set; }

            public List<flashSaleJuncBLL> flashSaleJuncBLL { get; set; }

                public static DataTable _dt;
                public static DataSet _ds;

        public flashSaleBLL GetAllFlashSale()
        {
            try
            {
                var _obj = new flashSaleBLL();
               
                SqlParameter[] p = new SqlParameter[0];
                _ds = (new DBHelper().GetDatasetFromSP)("sp_GetFlashSale_Web", p);

                if (_ds != null)
                {
                    if (_ds.Tables.Count > 0)
                    {
                        _obj = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_ds.Tables[0])).ToObject<List<flashSaleBLL>>().SingleOrDefault();
                        if (_obj != null)
                        {
                            _obj.flashSaleJuncBLL =  JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_ds.Tables[1])).ToObject<List<flashSaleJuncBLL>>().ToList();

                        }
                    }
                }
                return _obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }

 
       
  

}