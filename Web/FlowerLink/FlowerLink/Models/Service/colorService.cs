﻿using FlowerLink.Models.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace FlowerLink.Models.Service
{
    public class colorService : baseService
    {
        colorBLL _service;
        public colorService()
        {
            _service = new colorBLL();
        }

        public List<colorBLL> GetAll()
        {
            try
            {
                return _service.GetAll();
            }
            catch (Exception ex)
            {
                return new List<colorBLL>();
            }
        }

    }
}