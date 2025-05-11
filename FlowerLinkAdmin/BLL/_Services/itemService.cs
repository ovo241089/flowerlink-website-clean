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
    public class itemService : baseService
    {
        itemDB _service;
        public itemService()
        {
            _service = new itemDB();
        }

        public List<ItemBLL> GetAll()
        {
            try
            {
                return _service.GetAll();
            }
            catch (Exception ex)
            {
                return new List<ItemBLL>();
            }
        }

        public ItemBLL Get(int id)
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

        public List<string> GetItemImages(int id)
        {
            try
            {
                return _service.GetItemImages(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int Insert(ItemBLL data, IWebHostEnvironment _env)
        {
            List<ItemimagesBLL> imBLL = new List<ItemimagesBLL>();
            try
            {
                data.LastUpdatedDate = _UTCDateTime_SA();
                for (int i = 0; i < data.ImagesSource.Count; i++)
                {
                    var img = data.ImagesSource[i].ToString();
                    if (i == 0)
                    {
                        data.Image = UploadImage(img, "Item", _env);
                    }
                    if (i == 1)
                    {
                        data.HoveredImage = UploadImage(img, "Item", _env);
                    }

                    imBLL.Add(new ItemimagesBLL
                    {
                        Image = UploadImage(img, "Item", _env),
                        UpdatedDate = data.LastUpdatedDate
                    });

                }
                data.ItemImages = imBLL;

                var result = _service.Insert(data);

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Update(ItemBLL data, IWebHostEnvironment _env)
        {
            List<ItemimagesBLL> imBLL = new List<ItemimagesBLL>();
            try
            {
                for (int i = 0; i < data.ImagesSource.Count; i++)
                {
                    var img = data.ImagesSource[i].ToString();
                    if (i == 0)
                    {
                        data.Image = UploadImage(img, "Item", _env);
                    }
                    if (i == 1)
                    {
                        data.HoveredImage = UploadImage(img, "Item", _env);
                    }

                    imBLL.Add(new ItemimagesBLL
                    {
                        Image = UploadImage(img, "Item", _env),
                        UpdatedDate = data.LastUpdatedDate
                    });

                }
                data.ItemImages = imBLL;

                data.LastUpdatedDate = _UTCDateTime_SA();
                var result = _service.Update(data);

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Delete(ItemBLL data)
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
