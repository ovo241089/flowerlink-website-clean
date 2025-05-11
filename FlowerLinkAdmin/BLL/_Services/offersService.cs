using BAL.Repositories;
using FlowerLinkAdmin._Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FlowerLinkAdmin.BLL._Services
{
    public class offersService : baseService
    {
        offersDB _service;
        public offersService()
        {
            _service = new offersDB();
        }

        public List<OffersBLL> GetAll()
        {
            try
            {
                return _service.GetAll();
            }
            catch (Exception ex)
            {
                return new List<OffersBLL>();
            }
        }
        
        public OffersBLL Get(int id)
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
        public int Insert(OffersBLL data, IWebHostEnvironment _env)
        {
            try
            {
                data.DealImage = UploadImage(data.DealImage, "Offers", _env);
                data.LastUpdatedDate = _UTCDateTime_SA();
                var result = _service.Insert(data);

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Update(OffersBLL data, IWebHostEnvironment _env)
        {
            try
            {
                data.DealImage = UploadImage(data.DealImage, "Offers", _env);
                data.LastUpdatedDate = _UTCDateTime_SA();
                var result = _service.Update(data);

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Delete(OffersBLL data)
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
