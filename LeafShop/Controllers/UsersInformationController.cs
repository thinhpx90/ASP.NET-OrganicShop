using System;
using LeafShop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeafShop.Controllers
{
    public class UsersInformationController : Controller
    {
        LeafShopDb db = new LeafShopDb();
        // GET: UsersInformation
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UsersInf()
        {
            KhachHang session = (KhachHang)Session[LeafShop.Session.ConstaintUser.USER_SESSION];
            if (session == null)
            {
                return RedirectToAction("PageNotFound", "Error");
            }
            else
            {
                ViewBag.KhachHang = session;
                List<DatHang> listDh = db.DatHangs.Where(s => s.MaKhachHang == session.MaKhachHang).ToList();
                return View(listDh);
            }
        }
        [HttpGet]
        public ActionResult UsersUpdate(int id)
        {
            KhachHang session = (KhachHang)Session[LeafShop.Session.ConstaintUser.USER_SESSION];
            if (session == null)
            {
                return RedirectToAction("PageNotFound", "Error");
            }
            else
            {
                KhachHang tk = db.KhachHangs.Where(a => a.MaKhachHang.Equals(id)).FirstOrDefault();
                return View(tk);
            }
        }
        [HttpPost]
        public ActionResult UsersUpdate([Bind(Include = "MaKhachHang,TenKhachHang,Email,DiaChiKhachHang,DienThoaiKhachHang,GioiTinh,NgaySinh")] KhachHang kh)
        {
            var kha = kh.MaKhachHang;
            KhachHang res = db.KhachHangs.Where(a => a.MaKhachHang.Equals(kha)).FirstOrDefault();
            try
            {
                res.TenKhachHang = kh.TenKhachHang;
                res.DiaChiKhachHang = kh.DiaChiKhachHang;
                res.Email = kh.Email;
                res.DienThoaiKhachHang = kh.DienThoaiKhachHang;
                res.GioiTinh = kh.GioiTinh;
                res.NgaySinh = kh.NgaySinh;
                db.SaveChanges();
                Session[LeafShop.Session.ConstaintUser.USER_SESSION] = res;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ErrorUpdate", "Cập nhật thông tin không thành công ! Thử lại sau !" + ex.Message);
            }
            return RedirectToAction("UsersInf", "UsersInformation");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var chitiet = db.ChiTietDatHangs.Include("SanPham").Where(x => x.MaDatHang.Equals(id)).ToList();
            if (chitiet == null)
            {
                return HttpNotFound();
            }
            //int pageSize = 5;
            ViewBag.KhachHang = (KhachHang)Session[LeafShop.Session.ConstaintUser.USER_SESSION];
            //int pageNumber = (page ?? 1);
            return View(chitiet);
        }
    }
}