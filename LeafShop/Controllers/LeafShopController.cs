using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeafShop.Controllers
{
    public class LeafShopController : Controller
    {
        // GET: LeafShop
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChinhSach()
        {
            return View();
        }
        public ActionResult ThanhToan()
        {
            return View();
        }
        public ActionResult DieuKhoanSuDung()
        {
            return View();
        }
    }
}