using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LeafShop.Models;
using PagedList;

namespace LeafShop.Areas.Administrator.Controllers
{
    public class SanPhamController : Controller
    {
        private LeafShopDb db = new LeafShopDb();

        // GET: Administrator/SanPham
        //public ActionResult Index()
        //{
        //    var sanPhams = db.SanPhams.Include(s => s.DanhMuc).Include(s => s.KhuVuc).Include(s => s.ThuongHieu);
        //    return View(sanPhams.ToList());
        //}m d

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
            //var thuonghieus = db.ThuongHieux.Select(d => d);
            IQueryable<SanPham> sanphams = (from sp in db.SanPhams
                                            select sp).Include(s => s.DanhMuc).Include(s => s.ThuongHieu).OrderBy(x => x.MaSanPham);

            if (!String.IsNullOrEmpty(SearchString))
            {
                sanphams = sanphams.Where(p => p.TenSanPham.Contains(SearchString));
            }
            int pageSize = 5;

            int pageNumber = (page ?? 1);
            return View(sanphams.ToPagedList(pageNumber, pageSize));
        }

        // GET: Administrator/SanPham/Details/5
        public ActionResult Details(int id)
        {
            SanPham sanPham = db.SanPhams.Include("DanhMuc").Include("ThuongHieu").Where(s => s.MaSanPham == id).FirstOrDefault();
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // GET: Administrator/SanPham/Create
        public ActionResult Create()
        {
            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc");
            ViewBag.MaThuongHieu = new SelectList(db.ThuongHieux, "MaThuongHieu", "TenThuongHieu");
            return View();
        }

        // POST: Administrator/SanPham/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "MaSanPham,TenSanPham,MaDanhMuc,MaThuongHieu,MaKhuVuc,DonViTinh,SoLuong,SoLuongBan,DonGia,MoTa,NgayCapNhat,HinhMinhHoa,BinhLuan")] SanPham sanPham)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.SanPhams.Add(sanPham);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
        //    ViewBag.MaKhuVuc = new SelectList(db.KhuVucs, "MaKhuVuc", "TenKhuVuc", sanPham.MaKhuVuc);
        //    ViewBag.MaThuongHieu = new SelectList(db.ThuongHieux, "MaThuongHieu", "TenThuongHieu", sanPham.MaThuongHieu);
        //    return View(sanPham);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SanPham sp, HttpPostedFileBase uploadhinh)
        {
            try
            {
                ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc");
                ViewBag.MaThuongHieu = new SelectList(db.ThuongHieux, "MaThuongHieu", "TenThuongHieu");
                var existData = db.SanPhams.Where(x => x.MaSanPham == sp.MaSanPham).FirstOrDefault();

                if (sp.SoLuong < 0)
                {
                    ViewBag.Error = "Số lượng không được phép nhỏ hơn 0";
                    return View(sp);
                }
                if (existData != null)
                {
                    ViewBag.Error = "Mã sản phẩm này đã tồn tại";
                    return View(sp);
                }
                else if (existData == null && sp.SoLuong >= 0)
                {
                    var data = new SanPham
                    {
                        MaThuongHieu = sp.MaThuongHieu,
                        MaDanhMuc = sp.MaDanhMuc,
                        TenSanPham = sp.TenSanPham,
                        MoTa = sp.MoTa,
                        DonGia = sp.DonGia,
                        DonViTinh = sp.DonViTinh,
                        SoLuong = sp.SoLuong,
                        SoLuongBan = sp.SoLuongBan,
                        NgayKhoiTao = DateTime.Now,
                        NgayCapNhat = null
                    };
                    db.SanPhams.Add(data);
                    db.SaveChanges();
                    uploadhinh = Request.Files["ImageFile"];
                    if (uploadhinh != null && uploadhinh.ContentLength > 0)
                    {
                        int id = int.Parse(db.SanPhams.ToList().Last().MaSanPham.ToString());

                        string _FileName = "";
                        int index = uploadhinh.FileName.IndexOf('.');
                        _FileName = "sanpham" + id.ToString() + "." + uploadhinh.FileName.Substring(index + 1);
                        string _path = Path.Combine(Server.MapPath("~/Areas/UploadFile/SanPham/"), _FileName);
                        uploadhinh.SaveAs(_path);

                        SanPham item = db.SanPhams.FirstOrDefault(x => x.MaSanPham == id);
                        item.HinhMinhHoa = ("/Areas/UploadFile/SanPham/" + _FileName);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu!" + ex.Message;
                return View(sp);
            }

        }

        // GET: Administrator/SanPham/Edit/5
        public ActionResult Edit(int id)
        {
            SanPham sanPham = db.SanPhams.Include("DanhMuc").Include("ThuongHieu").Where(s => s.MaSanPham == id).FirstOrDefault();
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
            ViewBag.MaThuongHieu = new SelectList(db.ThuongHieux, "MaThuongHieu", "TenThuongHieu", sanPham.MaThuongHieu);
            return View(sanPham);
        }

        // POST: Administrator/SanPham/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "MaSanPham,TenSanPham,MaDanhMuc,MaThuongHieu,MaKhuVuc,DonViTinh,SoLuong,SoLuongBan,DonGia,MoTa,NgayCapNhat,HinhMinhHoa,BinhLuan")] SanPham sanPham)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(sanPham).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
        //    ViewBag.MaKhuVuc = new SelectList(db.KhuVucs, "MaKhuVuc", "TenKhuVuc", sanPham.MaKhuVuc);
        //    ViewBag.MaThuongHieu = new SelectList(db.ThuongHieux, "MaThuongHieu", "TenThuongHieu", sanPham.MaThuongHieu);
        //    return View(sanPham);
        //}

        [HttpPost]
        public ActionResult Edit(SanPham sp, HttpPostedFileBase uploadhinh)
        {
            SanPham sps = db.SanPhams.Where(x => x.MaSanPham == sp.MaSanPham).Include("ThuongHieu").Include("DanhMuc").FirstOrDefault();
            sps.NgayCapNhat = DateTime.Now;
            sps.SoLuong = sp.SoLuong;
            sps.SoLuongBan = sp.SoLuongBan;
            sps.MaDanhMuc = sp.MaDanhMuc;
            sps.MaThuongHieu = sp.MaThuongHieu;
            sps.DonViTinh = sp.DonViTinh;
            sps.DonGia = sp.DonGia;
            sps.TenSanPham = sp.TenSanPham;
            sps.MoTa = sp.MoTa;
            uploadhinh = Request.Files["ImageFile"];
            if (uploadhinh != null && uploadhinh.ContentLength > 0)
            {
                int id = sp.MaSanPham;
                string _FileName = "";
                int index = uploadhinh.FileName.IndexOf('.');
                _FileName = "sanpham" + id.ToString() + "." + uploadhinh.FileName.Substring(index + 1);
                string _path = Path.Combine(Server.MapPath("~/Areas/UploadFile/SanPham"), _FileName);
                uploadhinh.SaveAs(_path);
                sps.HinhMinhHoa = ("/Areas/UploadFile/SanPham/" + _FileName);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Administrator/SanPham/Delete/5
        public ActionResult Delete(int id)
        {
            SanPham sanPham = db.SanPhams.Include("DanhMuc").Include("ThuongHieu").Where(s => s.MaSanPham == id).FirstOrDefault();
            DanhMuc dmSP = db.DanhMucs.Where(s => s.MaDanhMuc == sanPham.MaDanhMuc).FirstOrDefault();
            ThuongHieu thSP = db.ThuongHieux.Where(s => s.MaThuongHieu == sanPham.MaThuongHieu).FirstOrDefault();
            ViewBag.TenDM = dmSP.TenDanhMuc;
            ViewBag.TenTH = thSP.TenThuongHieu;
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: Administrator/SanPham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPhams.Include("DanhMuc").Include("ThuongHieu").Where(s => s.MaSanPham == id).FirstOrDefault();
            DanhMuc dmSP = db.DanhMucs.Where(s => s.MaDanhMuc == sanPham.MaDanhMuc).FirstOrDefault();
            ThuongHieu thSP = db.ThuongHieux.Where(s => s.MaThuongHieu == sanPham.MaThuongHieu).FirstOrDefault();
            ViewBag.TenDM = dmSP.TenDanhMuc;
            ViewBag.TenTH = thSP.TenThuongHieu;
            try
            {
                db.SanPhams.Remove(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Không xoá được bản ghi này" + ex.Message;

                return View("Delete", sanPham);
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
