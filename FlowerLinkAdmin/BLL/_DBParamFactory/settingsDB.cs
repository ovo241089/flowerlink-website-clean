

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

    public class settingsDB : baseDB
    {
        public static SettingsBLL repo;
        public static DataTable _dt;
        public static DataSet _ds;
        public settingsDB()
           : base()
        {
            repo = new SettingsBLL();
            _dt = new DataTable();
            _ds = new DataSet();
        }

        public SettingsBLL Get(int id)
        {
            try
            {
                var _obj = new SettingsBLL();

                _dt = (new DBHelper().GetTableFromSP)("sp_GetSettings_Admin");
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        _obj = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<SettingsBLL>>().FirstOrDefault();
                    }
                }
                return _obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public int Update(SettingsBLL data)
        {
            try
            {
                int rtn = 0;
                SqlParameter[] p = new SqlParameter[7];

                p[0] = new SqlParameter("@TaxPercentage", data.TaxPercentage);
                p[1] = new SqlParameter("@DeliveryCharges", data.DeliveryCharges);
                p[2] = new SqlParameter("@MinimumOrderValue", data.MinimumOrderValue);
                p[3] = new SqlParameter("@COD", data.COD);
                p[4] = new SqlParameter("@BenefitPay", data.BenefitPay);
                p[5] = new SqlParameter("@PayPal", data.PayPal);
                p[6] = new SqlParameter("@Credimax", data.Credimax);

                rtn = (new DBHelper().ExecuteNonQueryReturn)("dbo.sp_UpdateSettings01_Admin", p);
                return rtn;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
