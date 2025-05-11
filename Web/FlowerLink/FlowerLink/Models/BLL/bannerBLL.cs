﻿using FlowerLink.Models;
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
    public class bannerBLL
    {
        public int BannerID { get; set; }
        public string Title { get; set; }
        public string MainHeading { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int StatusID { get; set; }
        public int Row_Counter { get; set; }
        public string ArabicTitle { get; set; }
        public string ArabicMainHeading { get; set; }
        public string ArabicDescription { get; set; }
        public string FormName { get; set; }

        public static DataTable _dt;
        public static DataSet _ds;
        public List<bannerBLL> GetBanner(string FormName)
        {
            try
            {
                var lst = new List<bannerBLL>();
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@FormName", FormName);
                _dt = (new DBHelper().GetTableFromSP)("sp_GetBanners",p);
                //_dt = (new DBHelper().GetTableFromSP)("sp_TestBanner",p);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        lst = _dt.DataTableToList<bannerBLL>();
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}