

using FlowerLinkAdmin._Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WebAPICode.Helpers;

namespace BAL.Repositories
{

    public class giftDB : baseDB
    {
        public static GiftBLL repo;
        public static DataTable _dt;
        public static DataSet _ds;
        public giftDB()
           : base()
        {
            repo = new GiftBLL();
            _dt = new DataTable();
            _ds = new DataSet();
        }

        public List<GiftBLL> GetAll()
        {
            try
            {
                var lst = new List<GiftBLL>();

                _dt = (new DBHelper().GetTableFromSP)("sp_GetGift");
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        //lst = _dt.DataTableToList<GiftBLL>();
                        lst = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<GiftBLL>>(); 
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                return new List<GiftBLL>();
            }
        }

        public GiftBLL Get(int id)
        {
            try
            {
                var _obj = new GiftBLL();
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@id", id);

                _dt = (new DBHelper().GetTableFromSP)("sp_GetGiftbyID_Admin", p);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        _obj = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<GiftBLL>>().FirstOrDefault();
                    }
                }

                return _obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int Insert(GiftBLL data)
        {
            try
            {
                int rtn = 0;
                SqlParameter[] p = new SqlParameter[14];

                p[0] = new SqlParameter("@Title", data.Title);
                p[1] = new SqlParameter("@ArabicTitle", data.ArabicTitle);
                p[2] = new SqlParameter("@Description", data.Description);
                p[3] = new SqlParameter("@Image", data.Image);
                p[4] = new SqlParameter("@ActualPrice", data.ActualPrice);
                p[5] = new SqlParameter("@DisplayPrice", data.DisplayPrice);
                p[6] = new SqlParameter("@DiscountedPrice", data.DiscountedPrice);
                p[7] = new SqlParameter("@InStock", data.InStock);
                p[8] = new SqlParameter("@DisplayOrder", data.DisplayOrder);
                p[9] = new SqlParameter("@StockQty", data.StockQty);
                p[10] = new SqlParameter("@StatusID", data.StatusID);
                p[11] = new SqlParameter("@LastUpdatedBy", data.LastUpdatedBy);
                p[12] = new SqlParameter("@LastUpdatedDate", data.LastUpdatedDate);
                p[13] = new SqlParameter("@GiftID", data.GiftID);
                rtn = (new DBHelper().ExecuteNonQueryReturn)("dbo.sp_insertGift_Admin", p);

                return rtn;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Update(GiftBLL data)
        {
            try
            {
                int rtn = 0;
                SqlParameter[] p = new SqlParameter[14];

                p[0] = new SqlParameter("@Title", data.Title);
                p[1] = new SqlParameter("@ArabicTitle", data.ArabicTitle);
                p[2] = new SqlParameter("@Description", data.Description);
                p[3] = new SqlParameter("@Image", data.Image);
                p[4] = new SqlParameter("@ActualPrice", data.ActualPrice);
                p[5] = new SqlParameter("@DisplayPrice", data.DisplayPrice);
                p[6] = new SqlParameter("@DiscountedPrice", data.DiscountedPrice);
                p[7] = new SqlParameter("@InStock", data.InStock);
                p[8] = new SqlParameter("@DisplayOrder", data.DisplayOrder);
                p[9] = new SqlParameter("@StockQty", data.StockQty);
                p[10] = new SqlParameter("@StatusID", data.StatusID);
                p[11] = new SqlParameter("@LastUpdatedBy", data.LastUpdatedBy);
                p[12] = new SqlParameter("@LastUpdatedDate", data.LastUpdatedDate);
                p[13] = new SqlParameter("@GiftID", data.GiftID);
                rtn = (new DBHelper().ExecuteNonQueryReturn)("dbo.sp_updateGift_Admin", p);

                return rtn;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Delete(GiftBLL data)
        {
            try
            {
                int _obj = 0;
                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@id", data.GiftID);
                p[1] = new SqlParameter("@LastUpdatedDate", data.LastUpdatedDate);

                _obj = (new DBHelper().ExecuteNonQueryReturn)("sp_DeleteGift", p);

                return _obj;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
