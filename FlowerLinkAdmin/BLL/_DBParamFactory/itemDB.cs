

using FlowerLinkAdmin._Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WebAPICode.Helpers;

namespace BAL.Repositories
{

    public class itemDB : baseDB
    {
        public static ItemBLL repo;
        public static DataTable _dt;
        public static DataSet _ds;
        public itemDB()
           : base()
        {
            repo = new ItemBLL();
            _dt = new DataTable();
            _ds = new DataSet();
        }

        public List<ItemBLL> GetAll()
        {
            try
            {
                var lst = new List<ItemBLL>();
                _dt = (new DBHelper().GetTableFromSP)("sp_GetItems");
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        lst = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<ItemBLL>>();
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ItemBLL Get(int id)
        {

            try
            {
                var _obj = new ItemBLL();
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@id", id);

                _dt = (new DBHelper().GetTableFromSP)("sp_GetItembyID_Admin", p);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        _obj = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<ItemBLL>>().FirstOrDefault();
                    }
                }

                return _obj;
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
                var _obj = new ItemBLL();
                List<string> ImagesSource = new List<string>();
                _dt = new DataTable();
                SqlParameter[] p1 = new SqlParameter[1];
                p1[0] = new SqlParameter("@id", id);
                _dt = (new DBHelper().GetTableFromSP)("sp_GetItemImages_Admin", p1);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        _obj.ItemImages = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(_dt)).ToObject<List<ItemimagesBLL>>();

                        for (int i = 0; i < _obj.ItemImages.Count; i++)
                        {
                            ImagesSource.Add(_obj.ItemImages[i].Image);
                        }
                    }
                }

                return ImagesSource;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int Insert(ItemBLL data)
        {
            try
            {
                int rtn = 0;
                SqlParameter[] p = new SqlParameter[21];

                p[0] = new SqlParameter("@Title", data.Title);
                p[1] = new SqlParameter("@ArabicTitle", data.ArabicTitle);
                p[2] = new SqlParameter("@Description", data.Description);
                p[3] = new SqlParameter("@Image", data.Image);
                p[4] = new SqlParameter("@HoveredImage", data.HoveredImage);
                p[5] = new SqlParameter("@Barcode", data.Barcode);
                p[6] = new SqlParameter("@SKU", data.SKU);
                p[7] = new SqlParameter("@DisplayOrder", data.DisplayOrder);
                p[8] = new SqlParameter("@Price", data.Price);
                p[9] = new SqlParameter("@DiscountedPrice", data.DiscountedPrice);
                p[10] = new SqlParameter("@Cost", data.Cost);
                p[11] = new SqlParameter("@LastUpdatedBy", data.LastUpdatedBy);
                p[12] = new SqlParameter("@LastUpdatedDate", data.LastUpdatedDate);
                p[13] = new SqlParameter("@StatusID", data.StatusID);
                p[14] = new SqlParameter("@IsFeatured", data.IsFeatured);
                p[15] = new SqlParameter("@InStock", data.InStock);
                p[16] = new SqlParameter("@StockQty", data.StockQty);
                p[17] = new SqlParameter("@ItemID", data.ItemID);
                p[18] = new SqlParameter("@Color", data.Color);
                p[19] = new SqlParameter("@DoublePrice", data.DoublePrice);
                p[20] = new SqlParameter("@IsDoubleQty", data.IsDoubleQty);

                rtn = int.Parse((new DBHelper().GetTableRow)("dbo.sp_insertItem_Admin", p).Rows[0]["ID"].ToString());



                if ( data.Categories != null)
                {
                    SqlParameter[] p1 = new SqlParameter[2];
                    p1[0] = new SqlParameter("@CategoryIDs", data.Categories == "" ? null : data.Categories);
                    p1[1] = new SqlParameter("@ItemID", rtn);
                    (new DBHelper().ExecuteNonQueryReturn)("sp_insertItemCatMapping_Admin", p1);
                }

                if ( data.Subcategories != null)
                {
                    SqlParameter[] p2 = new SqlParameter[2];
                    p2[0] = new SqlParameter("@SubCategoryIDs", data.Subcategories == "" ? null : data.Subcategories);
                    p2[1] = new SqlParameter("@ItemID", rtn);
                    (new DBHelper().ExecuteNonQueryReturn)("sp_insertItemSubcatMapping_Admin", p2);
                }

                try
                {
                    var imgStr = String.Join(",", data.ItemImages.Select(p => p.Image));
                    SqlParameter[] p3 = new SqlParameter[3];
                    p3[0] = new SqlParameter("@Images", imgStr);
                    p3[1] = new SqlParameter("@ItemID", rtn);
                    p3[2] = new SqlParameter("@LastUpdatedDate", data.LastUpdatedDate);
                    (new DBHelper().ExecuteNonQueryReturn)("sp_insertItemImages_Admin", p3);
                }
                catch { }

                return rtn;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Update(ItemBLL data)
        {
            try
            {
                int rtn = 0;
                SqlParameter[] p = new SqlParameter[21];

                p[0] = new SqlParameter("@Title", data.Title);
                p[1] = new SqlParameter("@ArabicTitle", data.ArabicTitle);
                p[2] = new SqlParameter("@Description", data.Description);
                p[3] = new SqlParameter("@Image", data.Image);
                p[4] = new SqlParameter("@HoveredImage", data.HoveredImage);
                p[5] = new SqlParameter("@Barcode", data.Barcode);
                p[6] = new SqlParameter("@SKU", data.SKU);
                p[7] = new SqlParameter("@DisplayOrder", data.DisplayOrder);
                p[8] = new SqlParameter("@Price", data.Price);
                p[9] = new SqlParameter("@DiscountedPrice", data.DiscountedPrice);
                p[10] = new SqlParameter("@Cost", data.Cost);
                p[11] = new SqlParameter("@LastUpdatedBy", data.LastUpdatedBy);
                p[12] = new SqlParameter("@LastUpdatedDate", data.LastUpdatedDate);
                p[13] = new SqlParameter("@StatusID", data.StatusID);
                p[14] = new SqlParameter("@IsFeatured", data.IsFeatured);
                p[15] = new SqlParameter("@InStock", data.InStock);
                p[16] = new SqlParameter("@StockQty", data.StockQty);
                p[17] = new SqlParameter("@ItemID", data.ItemID);
                p[18] = new SqlParameter("@Color", data.Color);
                p[19] = new SqlParameter("@DoublePrice", data.DoublePrice);
                p[20] = new SqlParameter("@IsDoubleQty", data.IsDoubleQty);
                rtn = (new DBHelper().ExecuteNonQueryReturn)("dbo.sp_updateItem_Admin", p);

                if (data.Categories != null)
                {
                    SqlParameter[] p1 = new SqlParameter[2];
          
                    p1[0] = new SqlParameter("@CategoryIDs", data.Categories == "" ? null : data.Categories);
                    p1[1] = new SqlParameter("@ItemID", data.ItemID);
                    (new DBHelper().ExecuteNonQueryReturn)("sp_insertItemCatMapping_Admin", p1);
                }

                if (data.Subcategories != null)
                {
                    SqlParameter[] p2 = new SqlParameter[2];
                    
                    p2[0] = new SqlParameter("@SubCategoryIDs", data.Subcategories == "" ? null : data.Subcategories);
                    p2[1] = new SqlParameter("@ItemID", data.ItemID);
                    (new DBHelper().ExecuteNonQueryReturn)("sp_insertItemSubcatMapping_Admin", p2);
                }

                try
                {
                   
                    SqlParameter[] p3 = new SqlParameter[3];
                    var imgStr = String.Join(",", data.ItemImages.Select(p => p.Image));
                    p3[0] = new SqlParameter("@Images", imgStr);
                    p3[1] = new SqlParameter("@ItemID", data.ItemID);
                    p3[2] = new SqlParameter("@LastUpdatedDate", data.LastUpdatedDate);
                    (new DBHelper().ExecuteNonQueryReturn)("sp_insertItemImages_Admin", p3);
                }
                catch { }

                return rtn;
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
                int _obj = 0;
                SqlParameter[] p = new SqlParameter[2];
                p[0] = new SqlParameter("@id", data.ItemID);
                p[1] = new SqlParameter("@LastUpdatedDate", data.LastUpdatedDate);

                _obj = (new DBHelper().ExecuteNonQueryReturn)("sp_DeleteItem", p);

                return _obj;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
