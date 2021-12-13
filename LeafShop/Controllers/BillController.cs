using LeafShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeafShop.Controllers
{
    public class BillController : Controller
    {
        // GET: Bill
        LeafShopDb db = new LeafShopDb();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListBills()
        {
            List<DatHang> list = new List<DatHang>();
            KhachHang kh = (KhachHang)Session[LeafShop.Session.ConstaintUser.USER_SESSION];
            list = db.DatHangs.Where(p => p.MaKhachHang == kh.MaKhachHang).OrderByDescending(x => x.NgayKhoiTao).ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            KhachHang kh = (KhachHang)Session[LeafShop.Session.ConstaintUser.USER_SESSION];
            if (kh == null)
            {
                return RedirectToAction("PageNotFound", "Error");
            }
            else
            {
                if (db.DatHangs.FirstOrDefault(x => x.MaKhachHang == kh.MaKhachHang) == null)
                {
                    return RedirectToAction("PageNotFound", "Error");
                }
            }
            DatHang dh = db.DatHangs.Include("TaiKhoanNguoiDung").Where(x => x.MaDatHang == id).FirstOrDefault();
            return View(dh);
        }

        [HttpGet]
        public ActionResult CreateBill(int tongtien, string ghiChu, string diaChi)
        {
            try
            {
                DatHang dh = new DatHang();
                KhachHang kh = (KhachHang)Session[LeafShop.Session.ConstaintUser.USER_SESSION];
                dh.MaKhachHang = kh.MaKhachHang;
                dh.NgayKhoiTao = DateTime.Now;
                dh.NgayGiaoHang = null;
                dh.DiaChi = diaChi;
                dh.TongTien = tongtien;
                dh.TrangThai = true;
                dh.MaNhanVien = null;
                dh.GhiChu = ghiChu;
                db.DatHangs.Add(dh);
                db.SaveChanges();
                List<ChiTietDatHang> list = (List<ChiTietDatHang>)Session[LeafShop.Session.ConstaintCart.CART];
                foreach (ChiTietDatHang item in list)
                {
                    item.SanPham = null;
                    item.MaDatHang = dh.MaDatHang;
                    db.ChiTietDatHangs.Add(item);
                    db.SaveChanges();
                }
                Session.Remove(LeafShop.Session.ConstaintCart.CART);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Có lỗi gì đó! Thử lại sau" + ex.Message });
            }

        }
    }
}