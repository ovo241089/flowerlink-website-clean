

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

    public class customerDB : baseDB
    {
        public static CustomerBLL repo;
        public static DataTable _dt;
        public static DataSet _ds;
        public customerDB()
           : base()
        {
            repo = new CustomerBLL();
            _dt = new DataTable();
            _ds = new DataSet();
        }

        public List<CustomerBLL> GetAll()
        {
            try
            {
                var lst = new List<CustomerBLL>();

                _dt = (new DBHelper().GetTableFromSP)("sp_GetCustomer");
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        lst = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<CustomerBLL>>();
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public CustomerBLL Get(int id)
        {
            try
            {
                var _obj = new CustomerBLL();
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@id", id);

                _dt = (new DBHelper().GetTableFromSP)("sp_GetCustomerbyID_Admin", p);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        _obj = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<CustomerBLL>>().FirstOrDefault();
                    }
                }
                return _obj;
            }
            catch (Exception ex)
            {
                return new CustomerBLL();
            }
        }

        public int Insert(CustomerBLL data)
        {
            try
            {
                int rtn = 0;
                SqlParameter[] p = new SqlParameter[9];

                p[0] = new SqlParameter("@FirstName", data.FirstName);
                p[1] = new SqlParameter("@LastName", data.LastName);
                p[2] = new SqlParameter("@Email", data.Email);
                p[3] = new SqlParameter("@Password", data.Password);
                p[4] = new SqlParameter("@IsVerified", data.IsVerified);
                p[5] = new SqlParameter("@StatusID", data.StatusID);
                p[6] = new SqlParameter("@LastUpdatedBy", data.LastUpdatedBy);
                p[7] = new SqlParameter("@LastUpdatedDate", data.LastUpdatedDate);
                p[8] = new SqlParameter("@CustomerID", data.CustomerID);

                rtn = (new DBHelper().ExecuteNonQueryReturn)("dbo.sp_insertCustomer_Admin", p);

                return rtn;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Update(CustomerBLL data)
        {
            try
            {
                int rtn = 0;
                SqlParameter[] p = new SqlParameter[9];

                p[0] = new SqlParameter("@FirstName", data.FirstName);
                p[1] = new SqlParameter("@LastName", data.LastName);
                p[2] = new SqlParameter("@Email", data.Email);
                p[3] = new SqlParameter("@Password", data.Password);
                p[4] = new SqlParameter("@IsVerified", data.IsVerified == null ? true : false);
                p[5] = new SqlParameter("@StatusID", data.StatusID);
                p[6] = new SqlParameter("@LastUpdatedBy", data.LastUpdatedBy);
                p[7] = new SqlParameter("@LastUpdatedDate", data.LastUpdatedDate);
                p[8] = new SqlParameter("@CustomerID", data.CustomerID);

                rtn = (new DBHelper().ExecuteNonQueryReturn)("dbo.sp_updateCustomer_Admin", p);
                return rtn;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Delete(CustomerBLL data)
        {
            try
            {
                int _obj = 0;
                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@id", data.CustomerID);
                p[1] = new SqlParameter("@LastUpdatedDate", data.LastUpdatedDate);

                _obj = (new DBHelper().ExecuteNonQueryReturn)("sp_DeleteCustomer", p);

                return _obj;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
