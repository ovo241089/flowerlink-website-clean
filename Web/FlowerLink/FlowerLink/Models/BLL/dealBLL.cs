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
    public class dealBLL
    {
        public int DealID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DealImage { get; set; }
        public double DiscountedPrice { get; set; }
        public DateTime StartDate { get; set; }
        public string EndDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public int NumberOfDays { get; set; }
        public int ItemID { get; set; }
        public int DisplayOrder { get; set; }
        public int LastUpdatedBy { get; set; }
        public int Row_Counter { get; set; }

        public static DataTable _dt;
        public static DataSet _ds;

        public List<dealBLL> GetAll()
        {
            try
            {
                var lst = new List<dealBLL>();
                _dt = (new DBHelper().GetTableFromSP)("sp_DealList");
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        lst = _dt.DataTableToList<dealBLL>();
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