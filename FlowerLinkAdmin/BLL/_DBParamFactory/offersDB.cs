

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

    public class offersDB : baseDB
    {
        public static OffersBLL repo;
        public static DataTable _dt;
        public static DataSet _ds;
        public offersDB()
           : base()
        {
            repo = new OffersBLL();
            _dt = new DataTable();
            _ds = new DataSet();
        }

        public List<OffersBLL> GetAll()
        {
            try
            {
                var lst = new List<OffersBLL>();
                _dt = (new DBHelper().GetTableFromSP)("sp_getDeal");
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        lst = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<OffersBLL>>();
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public OffersBLL Get(int id)
        {
            try
            {
                var _obj = new OffersBLL();
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@id", id);

                _dt = (new DBHelper().GetTableFromSP)("sp_GetDealbyID_Admin", p);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        _obj = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<OffersBLL>>().FirstOrDefault();
                    }
                }
                return _obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int Insert(OffersBLL data)
        {
            try
            {
                int rtn = 0;
                SqlParameter[] p = new SqlParameter[12];

                p[0] = new SqlParameter("@Title", data.Title);
                p[1] = new SqlParameter("@Description", data.Description);
                p[2] = new SqlParameter("@StartDate", Convert.ToDateTime(data.StartDate));
                p[3] = new SqlParameter("@EndDate", Convert.ToDateTime(data.EndDate));
                p[4] = new SqlParameter("@DealImage", data.DealImage);
                p[5] = new SqlParameter("@ItemID", data.ItemID);
                p[6] = new SqlParameter("@DisplayOrder", data.DisplayOrder);
                p[7] = new SqlParameter("@NumberOfDays", data.NumberOfDays);
                p[8] = new SqlParameter("@StatusID", data.StatusID);
                p[9] = new SqlParameter("@LastUpdatedBy", data.LastUpdatedBy);
                p[10] = new SqlParameter("@LastUpdatedDate", data.LastUpdatedDate);
                p[11] = new SqlParameter("@DealID", data.DealID);

                rtn = (new DBHelper().ExecuteNonQueryReturn)("dbo.sp_insertDeal_Admin", p);

                return rtn;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Update(OffersBLL data)
        {
            try
            {
                int rtn = 0;
                SqlParameter[] p = new SqlParameter[12];

                p[0] = new SqlParameter("@Title", data.Title);
                p[1] = new SqlParameter("@Description", data.Description);
                p[2] = new SqlParameter("@StartDate", Convert.ToDateTime(data.StartDate));
                p[3] = new SqlParameter("@EndDate", Convert.ToDateTime(data.EndDate));
                p[4] = new SqlParameter("@DealImage", data.DealImage);
                p[5] = new SqlParameter("@ItemID", data.ItemID);
                p[6] = new SqlParameter("@DisplayOrder", data.DisplayOrder);
                p[7] = new SqlParameter("@NumberOfDays", data.NumberOfDays);
                p[8] = new SqlParameter("@StatusID", data.StatusID);
                p[9] = new SqlParameter("@LastUpdatedBy", data.LastUpdatedBy);
                p[10] = new SqlParameter("@LastUpdatedDate", data.LastUpdatedDate);
                p[11] = new SqlParameter("@DealID", data.DealID);

                rtn = (new DBHelper().ExecuteNonQueryReturn)("dbo.sp_updateDeal_Admin", p);
                return rtn;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Delete(OffersBLL data)
        {
            try
            {
                int _obj = 0;
                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@id", data.DealID);
                p[1] = new SqlParameter("@LastUpdatedDate", data.LastUpdatedDate);
                _obj = (new DBHelper().ExecuteNonQueryReturn)("sp_DeleteDeal", p);

                return _obj;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
