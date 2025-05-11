

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

    public class bannerDB : baseDB
    {
        public static BannerBLL repo;
        public static DataTable _dt;
        public static DataSet _ds;
        public bannerDB()
           : base()
        {
            repo = new BannerBLL();
            _dt = new DataTable();
            _ds = new DataSet();
        }

        public List<BannerBLL> GetAll()
        {
            try
            {
                var lst = new List<BannerBLL>();

                _dt = (new DBHelper().GetTableFromSP)("sp_GetBanner");
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        lst = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<BannerBLL>>();
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public BannerBLL Get(int id)
        {
            try
            {
                var _obj = new BannerBLL();
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@id", id);

                _dt = (new DBHelper().GetTableFromSP)("sp_GetBannerbyID_Admin", p);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        _obj = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<BannerBLL>>().FirstOrDefault();
                    }
                }
                return _obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
       
        public int Insert(BannerBLL data)
        {
            try
            {
                int rtn = 0;
                SqlParameter[] p = new SqlParameter[12];

                p[0] = new SqlParameter("@Title", data.Title);
                p[1] = new SqlParameter("@Description", data.Description);
                p[2] = new SqlParameter("@MainHeading", data.MainHeading);
                p[3] = new SqlParameter("@Image", data.Image);
                p[4] = new SqlParameter("@StatusID", data.StatusID);
                p[5] = new SqlParameter("@LastUpdatedBy", data.LastUpdatedBy);
                p[6] = new SqlParameter("@LastUpdatedDate", data.LastUpdatedDate);
                p[7] = new SqlParameter("@BannerID", data.BannerID);
                p[8] = new SqlParameter("@ArabicTitle", data.ArabicTitle);
                p[9] = new SqlParameter("@ArabicDescription", data.ArabicDescription);
                p[10] = new SqlParameter("@ArabicMainHeading", data.ArabicMainHeading);
                p[11] = new SqlParameter("@FormName", data.FormName);

                rtn = (new DBHelper().ExecuteNonQueryReturn)("dbo.sp_insertBanner_Admin", p);
              
                return rtn;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Update(BannerBLL data)
        {
            try
            {
                int rtn = 0;
                SqlParameter[] p = new SqlParameter[12];

                p[0] = new SqlParameter("@Title", data.Title);
                p[1] = new SqlParameter("@Description", data.Description);
                p[2] = new SqlParameter("@MainHeading", data.MainHeading);
                p[3] = new SqlParameter("@Image", data.Image);
                p[4] = new SqlParameter("@StatusID", data.StatusID);
                p[5] = new SqlParameter("@LastUpdatedBy", data.LastUpdatedBy);
                p[6] = new SqlParameter("@LastUpdatedDate", data.LastUpdatedDate);
                p[7] = new SqlParameter("@BannerID", data.BannerID);
                p[8] = new SqlParameter("@ArabicTitle", data.ArabicTitle);
                p[9] = new SqlParameter("@ArabicDescription", data.ArabicDescription);
                p[10] = new SqlParameter("@ArabicMainHeading", data.ArabicMainHeading);
                p[11] = new SqlParameter("@FormName", data.FormName);

                rtn = (new DBHelper().ExecuteNonQueryReturn)("dbo.sp_updateBanner_Admin", p);
                return rtn;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Delete(BannerBLL data)
        {
            try
            {
                int _obj = 0;
                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@id", data.BannerID);
                p[1] = new SqlParameter("@LastUpdatedDate", data.LastUpdatedDate);

                _obj = (new DBHelper().ExecuteNonQueryReturn)("sp_DeleteBanner", p);

                return _obj;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
