

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

    public class reportDB : baseDB
    {
        public static DataTable _dt;
        public static DataSet _ds;
        public reportDB()
           : base()
        {
            _dt = new DataTable();
            _ds = new DataSet();
        }

        public List<salesSummarytBLL> GetSalesSummaryRpt( DateTime FromDate, DateTime ToDate)
        {
            try
            {
                var lst = new List<salesSummarytBLL>();

                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@fromdate", FromDate);
                p[1] = new SqlParameter("@todate", ToDate);

                _dt = (new DBHelper().GetTableFromSP)("sp_rptSalesSummaryAdmin", p);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        lst= JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<salesSummarytBLL>>();
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                return new List<salesSummarytBLL>();
            }
        }
        public List<SalesDetailBLL> GetSalesDetailRpt(  DateTime FromDate, DateTime ToDate)
        {
            try
            {
                var lst = new List<SalesDetailBLL>();

                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@fromdate", FromDate);
                p[1] = new SqlParameter("@todate", ToDate);

                _dt = (new DBHelper().GetTableFromSP)("sp_rptSalesDetailsReport", p);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        lst = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<SalesDetailBLL>>();
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                return new List<SalesDetailBLL>();
            }
        }
        public List<SalesItemwiseBLL> GetSalesItemwiseRpt(  DateTime FromDate, DateTime ToDate)
        {
            try
            {
                var lst = new List<SalesItemwiseBLL>();

                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@fromdate", FromDate);
                p[1] = new SqlParameter("@todate", ToDate);


                _dt = (new DBHelper().GetTableFromSP)("sp_rptSalesItemwiseAdmin", p);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        lst = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<SalesItemwiseBLL>>();

                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                return new List<SalesItemwiseBLL>();
            }
        }
        public List<SalesCategorywiseBLL> GetSalesCategorywiseRpt(  DateTime FromDate, DateTime ToDate)
        {
            try
            {
                var lst = new List<SalesCategorywiseBLL>();

                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@fromdate", FromDate);
                p[1] = new SqlParameter("@todate", ToDate);


                _dt = (new DBHelper().GetTableFromSP)("sp_rptSalesCategorywiseAdmin", p);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        lst = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<SalesCategorywiseBLL>>();
                        
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                return new List<SalesCategorywiseBLL>();
            }
        }
        public List<SalesUserwiseBLL> GetSalesUserwiseRpt(  DateTime FromDate, DateTime ToDate)
        {
            try
            {
                var lst = new List<SalesUserwiseBLL>();

                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@fromdate", FromDate);
                p[1] = new SqlParameter("@todate", ToDate);


                _dt = (new DBHelper().GetTableFromSP)("sp_rptSalesUserwiseAdmin", p);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        lst = _dt.DataTableToList<SalesUserwiseBLL>();
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                return new List<SalesUserwiseBLL>();
            }
        }
        public List<SalesCustomerwiseBLL> GetSalesCustomerwiseRpt(  int customerID, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                var lst = new List<SalesCustomerwiseBLL>();

                SqlParameter[] p = new SqlParameter[3];
                p[0] = new SqlParameter("@customerid", customerID);
                p[1] = new SqlParameter("@fromdate", FromDate);
                p[2] = new SqlParameter("@todate", ToDate);


                _dt = (new DBHelper().GetTableFromSP)("sp_rptSalesCustomerReport", p);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        lst = _dt.DataTableToList<SalesCustomerwiseBLL>();
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                return new List<SalesCustomerwiseBLL>();
            }
        }
    }
}
