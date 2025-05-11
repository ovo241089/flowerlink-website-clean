

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

    public class couponDB : baseDB
    {
        public static CouponBLL repo;
        public static DataTable _dt;
        public static DataSet _ds;
        public couponDB()
           : base()
        {
            repo = new CouponBLL();
            _dt = new DataTable();
            _ds = new DataSet();
        }

        public List<CouponBLL> GetAll()
        {
            try
            {
                var lst = new List<CouponBLL>();

                _dt = (new DBHelper().GetTableFromSP)("sp_getCoupons");
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        lst = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<CouponBLL>>();
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public CouponBLL Get(int id)
        {
            try
            {
                var _obj = new CouponBLL();
                SqlParameter[] p = new SqlParameter[1];

                p[0] = new SqlParameter("@id", id);
                _dt = (new DBHelper().GetTableFromSP)("sp_GetCouponsbyID_Admin",p);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        _obj = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<CouponBLL>>().FirstOrDefault();
                    }
                }
                return _obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int Insert(CouponBLL data)
        {
            try
            {
                int rtn = 0;
                SqlParameter[] p = new SqlParameter[11];

                p[0] = new SqlParameter("@Title", data.Title);
                p[1] = new SqlParameter("@Description", data.Description);
                p[2] = new SqlParameter("@FromDate", Convert.ToDateTime(data.FromDate));
                p[3] = new SqlParameter("@ToDate", Convert.ToDateTime(data.ToDate));
                p[4] = new SqlParameter("@Type", data.Type);
                p[5] = new SqlParameter("@Amount", data.Amount);
                p[6] = new SqlParameter("@CouponCode", data.CouponCode);
                p[7] = new SqlParameter("@StatusID", data.StatusID);
                p[8] = new SqlParameter("@LastUpdatedBy", data.LastUpdatedBy);
                p[9] = new SqlParameter("@LastUpdatedDate", data.LastUpdatedDate);
                p[10] = new SqlParameter("@CouponID", data.CouponID);

                rtn = (new DBHelper().ExecuteNonQueryReturn)("dbo.sp_insertCoupons_Admin", p);

                return rtn;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Update(CouponBLL data)
        {
            try
            {
                int rtn = 0;
                SqlParameter[] p = new SqlParameter[11];

                p[0] = new SqlParameter("@Title", data.Title);
                p[1] = new SqlParameter("@Description", data.Description);
                p[2] = new SqlParameter("@FromDate", Convert.ToDateTime(data.FromDate));
                p[3] = new SqlParameter("@ToDate", Convert.ToDateTime(data.ToDate));
                p[4] = new SqlParameter("@Type", data.Type);
                p[5] = new SqlParameter("@Amount", data.Amount);
                p[6] = new SqlParameter("@CouponCode", data.CouponCode);
                p[7] = new SqlParameter("@StatusID", data.StatusID);
                p[8] = new SqlParameter("@LastUpdatedBy", data.LastUpdatedBy);
                p[9] = new SqlParameter("@LastUpdatedDate", data.LastUpdatedDate);
                p[10] = new SqlParameter("@CouponID", data.CouponID);

                rtn = (new DBHelper().ExecuteNonQueryReturn)("dbo.sp_updateCoupons_Admin", p);
                return rtn;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Delete(CouponBLL data)
        {
            try
            {
                int _obj = 0;
                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@id", data.CouponID);
                   p[1] = new SqlParameter("@LastUpdatedDate", data.LastUpdatedDate);
                _obj = (new DBHelper().ExecuteNonQueryReturn)("sp_DeleteCoupons", p);

                return _obj;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
