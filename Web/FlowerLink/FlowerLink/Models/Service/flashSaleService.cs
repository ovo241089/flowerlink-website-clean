using FlowerLink.Models.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlowerLink.Models.Service
{
    public class flashSaleService : baseService
    {
        flashSaleBLL _service;
        
        public flashSaleService()
        {
            _service = new flashSaleBLL();
            
        }

       
        public flashSaleBLL GetFlashSale()
        {
            try
            {
                return _service.GetAllFlashSale();
            }
            catch (Exception ex)
            {
                return new flashSaleBLL();
            }
        }
        
        

    }
}