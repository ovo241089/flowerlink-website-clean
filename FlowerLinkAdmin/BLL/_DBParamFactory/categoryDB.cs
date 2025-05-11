

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

    public class categoryDB : baseDB
    {
        public static CategoryBLL repo;
        public static DataTable _dt;
        public static DataSet _ds;
        public categoryDB()
           : base()
        {
            repo = new CategoryBLL();
            _dt = new DataTable();
            _ds = new DataSet();
        }

        public List<CategoryBLL> GetAll()
        {
            try
            {
                var lst = new List<CategoryBLL>();

                _dt = (new DBHelper().GetTableFromSP)("sp_GetCategory");
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        //lst = _dt.DataTableToList<CategoryBLL>();
                        lst = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<CategoryBLL>>(); 
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                return new List<CategoryBLL>();
            }
        }

        public CategoryBLL Get(int id)
        {
            try
            {
                var _obj = new CategoryBLL();
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@id", id);

                _dt = (new DBHelper().GetTableFromSP)("sp_GetCategorybyID_Admin", p);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        //_obj = _dt.DataTableToList<CategoryBLL>().FirstOrDefault();
                        _obj = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<CategoryBLL>>().FirstOrDefault();
                    }
                }

                return _obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int Insert(CategoryBLL data)
        {
            try
            {
                int rtn = 0;
                SqlParameter[] p = new SqlParameter[9];

                p[0] = new SqlParameter("@Title", data.Title);
                p[1] = new SqlParameter("@ArabicTitle", data.ArabicTitle);
                p[2] = new SqlParameter("@Description", data.Description);
                p[3] = new SqlParameter("@Image", data.Image);
                p[4] = new SqlParameter("@UpdatedID", data.UpdatedID);
                p[5] = new SqlParameter("@UpdatedDate", data.UpdatedDate);
                p[6] = new SqlParameter("@IsActive", data.IsActive);
                p[7] = new SqlParameter("@CategoryID", data.CategoryID);
                p[8] = new SqlParameter("@DisplayOrder", data.DisplayOrder);
                rtn = (new DBHelper().ExecuteNonQueryReturn)("dbo.sp_insertCategory_Admin", p);

                return rtn;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Update(CategoryBLL data)
        {
            try
            {
                int rtn = 0;
                SqlParameter[] p = new SqlParameter[9];

                p[0] = new SqlParameter("@Title", data.Title);
                p[1] = new SqlParameter("@ArabicTitle", data.ArabicTitle);
                p[2] = new SqlParameter("@Description", data.Description);
                p[3] = new SqlParameter("@Image", data.Image);
                p[4] = new SqlParameter("@UpdatedID", data.UpdatedID);
                p[5] = new SqlParameter("@UpdatedDate", data.UpdatedDate);
                p[6] = new SqlParameter("@IsActive", data.IsActive);
                p[7] = new SqlParameter("@CategoryID", data.CategoryID);
                p[8] = new SqlParameter("@DisplayOrder", data.DisplayOrder);
                rtn = (new DBHelper().ExecuteNonQueryReturn)("dbo.sp_updateCategory_Admin", p);

                return rtn;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Delete(CategoryBLL data)
        {
            try
            {
                int _obj = 0;
                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@id", data.CategoryID);
                p[1] = new SqlParameter("@UpdatedDate", data.UpdatedDate);

                _obj = (new DBHelper().ExecuteNonQueryReturn)("sp_DeleteCategory", p);

                return _obj;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
