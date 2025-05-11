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
    public class colorBLL
    {
        public int colorID { get; set; }
        public string Title { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public int CreationID { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public int UpdatedID { get; set; }

        public static DataTable _dt;
        public List<colorBLL> GetAll()
        {
            try
            {
                var lst = new List<colorBLL>();
                _dt = (new DBHelper().GetTableFromSP)("sp_GetColorList");
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        lst = _dt.DataTableToList<colorBLL>();
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