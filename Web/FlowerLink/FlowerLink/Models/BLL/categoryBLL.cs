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
    public class categoryBLL
    {
        public int CategoryID { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public int CreationID { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public int UpdatedID { get; set; }
        public int IsActive { get; set; }
        public string ArabicTitle { get; set; }
        public string CategoryType { get; set; }
        public int Row_Counter { get; set; }

        public static DataTable _dt;
        public static DataSet _ds;
        public List<categoryBLL> GetAll()
        {
            try
            {
                var lst = new List<categoryBLL>();
                _dt = (new DBHelper().GetTableFromSP)("sp_GetCategoryList");
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        lst = _dt.DataTableToList<categoryBLL>();
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