using FlowerLink.Models.BLL;
using FlowerLink.Models.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FlowerLink.Controllers
{
    public class SharedController : Controller
    {
        // GET: Shared
        public ActionResult Header()
        {
            return PartialView("Header", new navigationBLL().GetSubCategory());
        }
        public ActionResult MobileNavigation()
        {
            return PartialView("MobileNavigation", new navigationBLL().GetSubCategory());
        }
    }
}