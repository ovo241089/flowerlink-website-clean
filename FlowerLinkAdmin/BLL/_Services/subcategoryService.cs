using BAL.Repositories;
using FlowerLinkAdmin._Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlowerLinkAdmin.BLL._Services
{
    public class subcategoryService : baseService
    {
        subcategoryDB _service;
        public subcategoryService()
        {
            _service = new subcategoryDB();
        }

        public List<SubCategoryBLL> GetAll()
        {
            try
            {
                return _service.GetAll();
            }
            catch (Exception ex)
            {
                return new List<SubCategoryBLL>();
            }
        }

        public SubCategoryBLL Get(int id)
        {
            try
            {
                return _service.Get(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int Insert(SubCategoryBLL data, IWebHostEnvironment _env)
        {
            try
            {
                data.LastUpdatedDate = _UTCDateTime_SA();
                var result = _service.Insert(data);

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Update(SubCategoryBLL data, IWebHostEnvironment _env)
        {
            try
            {
                
                data.LastUpdatedDate = _UTCDateTime_SA();
                var result = _service.Update(data);

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Delete(SubCategoryBLL data)
        {
            try
            {
                data.LastUpdatedDate = _UTCDateTime_SA();
                var result = _service.Delete(data);

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
