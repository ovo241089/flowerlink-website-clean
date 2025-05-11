using BAL.Repositories;
using FlowerLinkAdmin._Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FlowerLinkAdmin.BLL._Services
{
    public class dashboardService : baseService
    {
        dashboardDB _service;
        public dashboardService()
        {
            _service = new dashboardDB();
        }

        public RspDashboard GetDashboard(string FDate, string TDate)
        {
            var rsp = new RspDashboard();
            try
            {
                rsp.summarysales = _service.GetDashboardSummary(DateTime.Parse(FDate), DateTime.Parse(TDate));
            }
            catch (Exception)
            {
                rsp.summarysales = new DashboardSummary();
            }


            try
            {
                rsp.maensales = _service.GetMAENSummary( DateTime.Parse(FDate));
            }
            catch (Exception)
            {
                rsp.maensales = new DashboardMAEN();
            }


            try
            {
                rsp.todaysales = _service.GetTodaySales( DateTime.Parse(FDate));
            }
            catch (Exception)
            {
                rsp.todaysales = new DashboardToday();
            }


            return rsp;

        }

    }
}
