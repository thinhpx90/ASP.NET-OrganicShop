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
    public class DatHangController : Controller
    {
        private LeafShopDb db = new LeafShopDb();

        // GET: Administrator/DatHang
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
            IQueryable<DatHang> datHangs = (from hang in db.DatHangs
                                            select hang).Include("NhanVien").Include("KhachHang")
                    .OrderBy(student => student.MaDatHang);
         
            
            if (!String.IsNullOrEmpty(SearchString))
            {
                datHangs = datHangs.Where(p => p.NhanVien.TenNhanVien.Contains(SearchString) || p.KhachHang.TenKhachHang.Contains(SearchString) || p.MaDatHang.ToString().Contains(SearchString));
            }
            int pageSize = 5;

            int pageNumber = (page ?? 1);
            return View(datHangs.ToPagedList(pageNumber, pageSize));
        }
        // GET: Administrator/DatHang/Details/5
        public ActionResult Details(int id)
        {
            DatHang datHang = db.DatHangs.Include("NhanVien").Include("KhachHang").Where(s => s.MaDatHang == id).FirstOrDefault();
            ViewBag.listDH = db.ChiTietDatHangs.Include("SanPham").Where(s => s.MaDatHang == id).ToList();
            if (datHang == null)
            {
                return HttpNotFound();
            }
            return View(datHang);
        }

        // GET: Administrator/DatHang/Create
        public ActionResult Create()
        {
            ViewBag.MaKhachHang = new SelectList(db.KhachHangs, "MaKhachHang", "TenKhachHang");
            ViewBag.MaNhanVien = new SelectList(db.NhanViens, "MaNhanVien", "TenNhanVien");
            return View();
        }

        // POST: Administrator/DatHang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DatHang datHang)
        {
            try
            {
                
                ViewBag.MaKhachHang = new SelectList(db.KhachHangs, "MaKhachHang", "TenKhachHang", datHang.MaKhachHang);
                ViewBag.MaNhanVien = new SelectList(db.NhanViens, "MaNhanVien", "TenNhanVien", datHang.MaNhanVien);

                var existData = db.DatHangs.Where(x => x.MaDatHang == datHang.MaDatHang).FirstOrDefault();
                if(existData != null)
                {
                    ViewBag.Error = "Mã đặt hàng này đã tồn tại";
                }
                else if (existData == null)
                {
                    var data = new DatHang
                    {
                        MaKhachHang = datHang.MaKhachHang,
                        MaNhanVien = datHang.MaNhanVien,
                        DiaChi = datHang.DiaChi,
                        GhiChu = datHang.GhiChu,
                        NgayKhoiTao = DateTime.Now,
                        NgayGiaoHang = null,
                        TrangThai = datHang.TrangThai,
                        TongTien = datHang.TongTien
                    };
                    db.DatHangs.Add(data);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu!" + ex.Message;
                return View(datHang);
            }
        }

        // GET: Administrator/DatHang/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            DatHang datHang = db.DatHangs.Find(id);
            if (datHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKhachHang = new SelectList(db.KhachHangs, "MaKhachHang", "TenKhachHang", datHang.MaKhachHang);
            ViewBag.MaNhanVien = new SelectList(db.NhanViens, "MaNhanVien", "TenNhanVien", datHang.MaNhanVien);
            return View(datHang);
        }

        // POST: Administrator/DatHang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DatHang datHang)
        {
            DatHang dathangs = db.DatHangs.Where(s => s.MaDatHang == datHang.MaDatHang).FirstOrDefault();
            dathangs.MaKhachHang = datHang.MaKhachHang;
            dathangs.MaNhanVien = datHang.MaNhanVien;
            dathangs.NgayKhoiTao = datHang.NgayKhoiTao;
            dathangs.TongTien = datHang.TongTien;
            dathangs.DiaChi = datHang.DiaChi;
            dathangs.GhiChu = datHang.GhiChu;
            dathangs.TrangThai = datHang.TrangThai;
            if (dathangs.TrangThai == false)
            {
                dathangs.NgayGiaoHang = DateTime.Now;
             }
            else if(dathangs.TrangThai == true)
            {
                dathangs.NgayGiaoHang = null;
            } 

            db.SaveChanges();
            return RedirectToAction("Index");
        }

            // GET: Administrator/DatHang/Delete/5
            public ActionResult Delete(int id)
            {
            DatHang datHang = db.DatHangs.Include("NhanVien").Include("KhachHang").Where(s => s.MaDatHang == id).FirstOrDefault();
            ViewBag.listDH = db.ChiTietDatHangs.Include("SanPham").Where(s => s.MaDatHang == id).ToList();
            if (datHang == null)
            {
                return HttpNotFound();
            }
            return View(datHang);
            }

        // POST: Administrator/DatHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
              
            DatHang datHang = db.DatHangs.Find(id);
            try
            {
                List<ChiTietDatHang> dsDH = db.ChiTietDatHangs.Where(s => s.MaDatHang == id).ToList();
                for (var i = 0; i < dsDH.Count; i++)
                {
                    ChiTietDatHang x = db.ChiTietDatHangs.Where(s => s.MaDatHang == id).FirstOrDefault();
                    db.ChiTietDatHangs.Remove(x);
                    db.SaveChanges();
                }
                db.DatHangs.Remove(datHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.Error = "Không xoá được đơn đặt hàng này!";
                return View("Delete", datHang);
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
