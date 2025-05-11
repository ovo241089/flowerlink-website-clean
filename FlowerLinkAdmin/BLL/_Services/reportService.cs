using BAL.Repositories;
using FlowerLinkAdmin._Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FlowerLinkAdmin.BLL._Services
{
    public class reportService : baseService
    {
        reportDB _service;
        public reportService()
        {
            _service = new reportDB();
        }

        public List<salesSummarytBLL> GetSalesSummaryRpt(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                return _service.GetSalesSummaryRpt(FromDate, ToDate);
            }
            catch (Exception ex)
            {
                return new List<salesSummarytBLL>();
            }
        }
        public List<SalesDetailBLL> GetSalesDetailRpt(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                return _service.GetSalesDetailRpt(FromDate, ToDate);
            }
            catch (Exception ex)
            {
                return new List<SalesDetailBLL>();
            }
        }
        public List<SalesUserwiseBLL> GetSalesUserwiseRpt(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                return _service.GetSalesUserwiseRpt(FromDate, ToDate);
            }
            catch (Exception ex)
            {
                return new List<SalesUserwiseBLL>();
            }
        }
        public List<SalesItemwiseBLL> GetSalesItemwiseRpt(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                return _service.GetSalesItemwiseRpt(FromDate, ToDate);
            }
            catch (Exception ex)
            {
                return new List<SalesItemwiseBLL>();
            }
        }
        public List<SalesCustomerwiseBLL> GetSalesCustomerwiseRpt(int customerID, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                return _service.GetSalesCustomerwiseRpt(customerID, FromDate, ToDate);
            }
            catch (Exception ex)
            {
                return new List<SalesCustomerwiseBLL>();
            }
        }
        public List<SalesCategorywiseBLL> GetSalesCategorywiseRpt(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                return _service.GetSalesCategorywiseRpt(FromDate, ToDate);
            }
            catch (Exception ex)
            {
                return new List<SalesCategorywiseBLL>();
            }
        }
    }
}
