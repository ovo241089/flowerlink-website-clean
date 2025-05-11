
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
    public class subcategoryDB : baseDB
    {
        public static SubCategoryBLL repo;
        public static DataTable _dt;
        public static DataSet _ds;
        public subcategoryDB()
           : base()
        {
            repo = new SubCategoryBLL();
            _dt = new DataTable();
            _ds = new DataSet();
        }

        public List<SubCategoryBLL> GetAll()
        {
            try
            {
                var lst = new List<SubCategoryBLL>();

                _dt = (new DBHelper().GetTableFromSP)("sp_GetSubCategory");
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        lst = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<SubCategoryBLL>>();
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                return new List<SubCategoryBLL>();
            }
        }

        public SubCategoryBLL Get(int id)
        {
            try
            {
                var _obj = new SubCategoryBLL();
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@id", id);

                _dt = (new DBHelper().GetTableFromSP)("sp_GetSubCategorybyID_Admin", p);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        _obj = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<SubCategoryBLL>>().FirstOrDefault();
                    }
                }

                return _obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int Insert(SubCategoryBLL data)
        {
            try
            {
                int rtn = 0;
                SqlParameter[] p = new SqlParameter[6];

                p[0] = new SqlParameter("@Title", data.Title);
                p[1] = new SqlParameter("@ArabicTitle", data.ArabicTitle);
                p[2] = new SqlParameter("@LastUpdatedBy", data.LastUpdatedBy);
                p[3] = new SqlParameter("@LastUpdatedDate", data.LastUpdatedDate);
                p[4] = new SqlParameter("@StatusID", data.StatusID);
                p[5] = new SqlParameter("@SubCategoryID", data.SubCategoryID);
                rtn = (new DBHelper().ExecuteNonQueryReturn)("dbo.sp_insertSubCategory_Admin", p);

                if (data.Categories != null)
                {
                    SqlParameter[] p1 = new SqlParameter[2];
                    p1[0] = new SqlParameter("@CategoryIDs", data.Categories == "" ? null : data.Categories);
                    p1[1] = new SqlParameter("@SubCategoryID", rtn);
                    (new DBHelper().ExecuteNonQueryReturn)("sp_insertCatSubCatMapping_Admin", p1);
                }
                return rtn;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Update(SubCategoryBLL data)
        {
            try
            {
                int rtn = 0;
                SqlParameter[] p = new SqlParameter[6];
                p[0] = new SqlParameter("@Title", data.Title);
                p[1] = new SqlParameter("@ArabicTitle", data.ArabicTitle);
                p[2] = new SqlParameter("@LastUpdatedBy", data.LastUpdatedBy);
                p[3] = new SqlParameter("@LastUpdatedDate", data.LastUpdatedDate);
                p[4] = new SqlParameter("@StatusID", data.StatusID);
                p[5] = new SqlParameter("@SubCategoryID", data.SubCategoryID);
                rtn = (new DBHelper().ExecuteNonQueryReturn)("dbo.sp_updateSubCategory_Admin", p);

                if (data.Categories != null)
                {
                    SqlParameter[] p1 = new SqlParameter[2];

                    p1[0] = new SqlParameter("@CategoryIDs", data.Categories == "" ? null : data.Categories);
                    p1[1] = new SqlParameter("@SubCategoryID", data.SubCategoryID);
                    (new DBHelper().ExecuteNonQueryReturn)("sp_insertCatSubCatMapping_Admin", p1);
                }

                return rtn;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Delete(SubCategoryBLL data)
        {
            try
            {
                int _obj = 0;
                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@id", data.SubCategoryID);
                p[1] = new SqlParameter("@LastUpdatedDate", data.LastUpdatedDate);

                _obj = (new DBHelper().ExecuteNonQueryReturn)("sp_DeleteSubCategory", p);

                return _obj;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
