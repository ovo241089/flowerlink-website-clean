﻿using FlowerLink.Models.BLL;
using FlowerLink.Models.Service;
using org.apache.tomcat.jni;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace FlowerLink.Controllers
{
    public class AccountController : Controller
    {
        
        // GET: Account
        [HttpGet]
        public ActionResult Login_Register(int id = 0)
        {
            ViewBag.Banner = new bannerBLL().GetBanner("Shop");
            Session["LoginRoute"] = id;
            Session["CustomerID"] = 0;
            return View();
        }
        [HttpPost]
        public ActionResult Login_Register(loginBLL service)
        {
            if (service.ContactNo != null)
            {
                service.Register();
                Session["LoginNote"] = "Login Now";
                return RedirectToAction("Login_Register", "Account");
            }
            else
            {
                service = service.login();
                Session["LoginNote"] = null;
                Session["CustomerID"] = service.CustomerID;
                Session["CustomerEmail"] = service.Email;
                Session["CustomerContactNo"] = service.ContactNo;
                Session["CustomerName"] = string.Concat(service.FirstName," ", service.LastName);
                Session["IsVerified"] = service.IsVerified;
                if (Convert.ToInt32(Session["IsVerified"]) != 0)
                {
                    if (Session["CustomerEmail"].ToString() != null)
                    {
                        Session["LoginNote"] = "Successfully Login";
                        if (Convert.ToInt32(Session["LoginRoute"]) == 1)
                        {
                            return RedirectToAction("Checkout", "Order");
                            
                        }
                        else
                        {
                            return RedirectToAction("Checkout", "Order");
                            
                        } 
                    }
                    Session["LoginNote"] = "User is not verified";
                    return RedirectToAction("Login_Register", "Account");
                }
                else
                {
                    Session["CustomerName"] = null;
                    Session["LoginNote"] = "Invalid Email or Password";
                    return RedirectToAction("Login_Register", "Account");
                }
            }
            
        }
       
        public ActionResult Logout()
        {
            Session["LoginNote"] = null;
            Session["CustomerID"] = null;
            Session["CustomerEmail"] = null;
            Session["CustomerContactNo"] = null;
            Session["CustomerName"] = null;
            Session["IsVerified"] = null;
            Session["LoginRoute"] = null;
            return RedirectToAction("Index", "Home");
        }

      

    }
}