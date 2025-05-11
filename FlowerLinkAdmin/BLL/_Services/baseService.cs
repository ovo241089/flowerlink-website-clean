﻿using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FlowerLinkAdmin.BLL._Services
{
    public class baseService
    {


        public baseService()
        {
        }
        public DateTime _UTCDateTime_SA()
        {
            return DateTime.UtcNow.AddMinutes(180);
        }


        public string UploadImage(string image, string folderName, IWebHostEnvironment _env)
        {
            try
            {
                var chkImage = IsBase64Encoded(image
                    .Replace("data:image/png;base64,", "")
                    .Replace("data:image/jpg;base64,", "")
                    .Replace("data:image/jpeg;base64,", ""));

                if (chkImage)
                {
                    if (image != null && image != "")
                    {
                        image = uploadFiles(image, folderName, _env);
                    }
                }
            }
            catch { }

            return image;
        }

        public bool IsBase64Encoded(String str)
        {
            try
            {
                // If no exception is caught, then it is possibly a base64 encoded string
                byte[] data = Convert.FromBase64String(str);
                // The part that checks if the string was properly padded to the
                // correct length was borrowed from d@anish's solution
                return (str.Replace(" ", "").Length % 4 == 0);
            }
            catch
            {
                // If exception is caught, then it is not a base64 encoded string
                return false;
            }
        }
        public string uploadFiles(string _bytes, string foldername, IWebHostEnvironment _webHostEnvironment)
        {
            try
            {
                if (_bytes != null && _bytes.ToString() != "")
                {

                    byte[] bytes = Convert.FromBase64String(_bytes.Replace("data:image/png;base64,", "")
                        .Replace("data:image/jpg;base64,", "")
                        .Replace("data:image/jpeg;base64,", "")
                        .Replace("data:image/svg+xml;base64,", ""));

                    string webRootPath = _webHostEnvironment.WebRootPath;
                    string contentRootPath = _webHostEnvironment.ContentRootPath;

                    string path = "/ClientApp/dist/assets/Upload/" + foldername + "/" + Path.GetFileName(Guid.NewGuid() + ".jpg");
                    string filePath = contentRootPath + path;

                    System.IO.File.WriteAllBytes(filePath, bytes);

                    _bytes = path.Replace( "/ClientApp/dist/", "");

                }
                else
                {
                    _bytes = "";
                }
            }
            catch (Exception ex)
            {
                _bytes = "";
            }
            return _bytes;
        }
    }


}
