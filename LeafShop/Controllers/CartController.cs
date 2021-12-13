using LeafShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeafShop.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        LeafShopDb db = new LeafShopDb();
        int tongTien;
        public ActionResult Index()
        {
            return View();
        }

        // GET: Cart
        [HttpGet]
        public ActionResult Orders()
        {
            List<ChiTietDatHang> list = new List<ChiTietDatHang>();
 
            if (Session[LeafShop.Session.ConstaintCart.CART] != null)
            {
                list = (List<ChiTietDatHang>)Session[LeafShop.Session.ConstaintCart.CART];
                foreach (ChiTietDatHang item in list)
                {
                    item.SanPham = db.SanPhams.Where(x => x.MaSanPham == item.MaSanPham).FirstOrDefault();
                }
            }
            return View(list);
        }

        [HttpPost]
        public ActionResult Orders(List<ChiTietDatHang> list)
        {
            Session.Remove(LeafShop.Session.ConstaintCart.CART);
            foreach(ChiTietDatHang item in list)
            {
                item.DonGia = db.SanPhams.Where(s => s.MaSanPham == item.MaSanPham).FirstOrDefault().DonGia;
                SanPham sp = db.SanPhams.Where(x => x.MaSanPham == item.MaSanPham).FirstOrDefault();
                item.SanPham = sp;
            }
            Session[LeafShop.Session.ConstaintCart.CART] = list;
            return View(list);
        }


        [HttpPost]
        public JsonResult AddToCart(ChiTietDatHang chiTiet)
        {
            bool isExists = false;
            List<ChiTietDatHang> list = new List<ChiTietDatHang>();
            if (Session[LeafShop.Session.ConstaintCart.CART] != null)
            {
                list = (List<ChiTietDatHang>)Session[LeafShop.Session.ConstaintCart.CART];
                foreach (ChiTietDatHang item in list)
                {
                    if (item.MaSanPham == chiTiet.MaSanPham)
                    {
                        item.SoLuong += chiTiet.SoLuong;
                        isExists = true;
                    }
                }
                if (!isExists)
                {
                    list.Add(chiTiet);
                }
            }
            else
            {
                list = new List<ChiTietDatHang>();
                list.Add(chiTiet);
            }
            list.RemoveAll((x) => x.SoLuong <= 0);
            foreach (ChiTietDatHang item in list)
            {
                item.DonGia = db.SanPhams.Where(s => s.MaSanPham == item.MaSanPham).FirstOrDefault().DonGia;
                SanPham sp = db.SanPhams.Where(x => x.MaSanPham == item.MaSanPham).FirstOrDefault();
                item.SanPham = sp;
            }
            Session[LeafShop.Session.ConstaintCart.CART] = list;
            return Json(new { status = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getCart()
        {
            List<ChiTietDatHang> list = new List<ChiTietDatHang>();
            if (Session[LeafShop.Session.ConstaintCart.CART] != null)
            {
                list = (List<ChiTietDatHang>)Session[LeafShop.Session.ConstaintCart.CART];
            }
            foreach (ChiTietDatHang item in list)
            {
                SanPham sp = db.SanPhams.Where(x => x.MaSanPham == item.MaSanPham).FirstOrDefault();
                item.SanPham = sp;
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteFromCart(int id)
        {
            List<ChiTietDatHang> list = (List<ChiTietDatHang>)Session[LeafShop.Session.ConstaintCart.CART];
            list.RemoveAll((x) => x.MaSanPham == id);
            Session[LeafShop.Session.ConstaintCart.CART] = list;
            return Json(new {status = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CheckOut()
        {
            KhachHang kh = (KhachHang)Session[LeafShop.Session.ConstaintUser.USER_SESSION];
            if (kh == null)
            {
                return RedirectToAction("Login", "Home");
            }
            ViewBag.khachHang = (KhachHang)Session[LeafShop.Session.ConstaintUser.USER_SESSION];

            List<ChiTietDatHang> res = (List<ChiTietDatHang>)Session[LeafShop.Session.ConstaintCart.CART];
            foreach (ChiTietDatHang item in res)
            {
                if (item.DonGia != null)
                {
                    tongTien += (item.SoLuong.HasValue ? item.SoLuong.Value : 0) * (item.DonGia.HasValue ? item.DonGia.Value : 0);
                }
            }
            ViewBag.TaiKhoan = kh;
            ViewBag.TongTien = tongTien;

            return View(res);
        }

        [HttpPost]
        public ActionResult CheckOut([Bind(Include = "GhiChu,DiaChi")] DatHang dh)
        {
            string diaChi = dh.DiaChi;
            string ghiChu = dh.GhiChu;
            List<ChiTietDatHang> res = (List<ChiTietDatHang>)Session[LeafShop.Session.ConstaintCart.CART];
            foreach (ChiTietDatHang item in res)
            {
                if (item.DonGia != null)
                {
                    tongTien += (item.SoLuong.HasValue ? item.SoLuong.Value : 0) * (item.DonGia.HasValue ? item.DonGia.Value : 0);
                }
            }
            tongTien += 35000;
            return RedirectToAction("CreateBill", "Bill", new { tongTien = tongTien, ghiChu = ghiChu, diaChi = diaChi });
        }

        [HttpGet]
        public ActionResult DeleteFromCartByMaSP(int masp)
        {
            List<ChiTietDatHang> list = (List<ChiTietDatHang>)Session[LeafShop.Session.ConstaintCart.CART];
            list.RemoveAll((x) => x.MaSanPham == masp);
            Session[LeafShop.Session.ConstaintCart.CART] = list;
            return RedirectToAction("Orders");
        }
    }
}