using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LeafShop.Models;
using PagedList;

namespace LeafShop.Areas.Administrator.Controllers
{
    public class KhachHangController : Controller
    {
        private LeafShopDb db = new LeafShopDb();

        // GET: Administrator/KhachHang
        public ActionResult Index(string SearchString, string currentFilter, int? page)
        {
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            ViewBag.CurrentFilter = SearchString;
            //var khachhangs = db.KhachHangs.Select(d => d);
            IQueryable<KhachHang> khachhangs = (from kh in db.KhachHangs
                                                select kh)
                    .OrderBy(student => student.MaKhachHang);
            if (!String.IsNullOrEmpty(SearchString))
            {
                khachhangs = khachhangs.Where(p => p.TenKhachHang.Contains(SearchString));
            }
            int pageSize = 5;

            int pageNumber = (page ?? 1);
            return View(khachhangs.ToPagedList(pageNumber, pageSize));
        }

        // GET: Administrator/KhachHang/Details/5
        public ActionResult Details(int id)
        {
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // GET: Administrator/KhachHang/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administrator/KhachHang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKhachHang,TenKhachHang,DiaChiKhachHang,DienThoaiKhachHang,TenDangNhap,MatKhau,NgaySinh,GioiTinh,Email,TrangThai")] KhachHang khachHang)
        {
            try
            {
                var existData = db.KhachHangs.Where(x => x.TenDangNhap == khachHang.TenDangNhap).FirstOrDefault();
                if (existData != null)
                {
                    ViewBag.Error = "Tên đăng nhập này đã tồn tại!";
                    return View(khachHang);
                }
                else
                {
                    db.KhachHangs.Add(khachHang);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu!" + ex.Message;
                return View(khachHang);
            }

        }

        // GET: Administrator/KhachHang/Edit/5
        public ActionResult Edit(int id)
        {
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // POST: Administrator/KhachHang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKhachHang,TenKhachHang,DiaChiKhachHang,DienThoaiKhachHang,TenDangNhap,MatKhau,NgaySinh,GioiTinh,Email,TrangThai")] KhachHang khachHang)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(khachHang).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu!" + ex.Message;
                return View(khachHang);
            }

        }

        // GET: Administrator/KhachHang/Delete/5
        public ActionResult Delete(int id)
        {
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // POST: Administrator/KhachHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KhachHang khachHang = db.KhachHangs.Find(id);
            try
            {
                db.KhachHangs.Remove(khachHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Không được xoá bản ghi này!" + ex.Message;
                return View(khachHang);
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
