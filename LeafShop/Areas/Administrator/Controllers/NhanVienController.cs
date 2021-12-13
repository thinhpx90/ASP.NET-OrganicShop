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
    public class NhanVienController : Controller
    {
        private LeafShopDb db = new LeafShopDb();

        // GET: Administrator/NhanVien
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
            IQueryable<NhanVien> nhanviens = (from nv in db.NhanViens
                                                  select nv)
                    .OrderBy(x => x.MaNhanVien);
            if (!String.IsNullOrEmpty(SearchString))
            {
                nhanviens = nhanviens.Where(p => p.TenNhanVien.Contains(SearchString));
            }
            int pageSize = 5;

            int pageNumber = (page ?? 1);
            return View(nhanviens.ToPagedList(pageNumber, pageSize));
        }

        // GET: Administrator/NhanVien/Details/5
        public ActionResult Details(int id)
        {
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // GET: Administrator/NhanVien/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administrator/NhanVien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NhanVien nv, HttpPostedFileBase uploadhinh)
        {
            try
            {
                NhanVien existData = db.NhanViens.FirstOrDefault(x => x.MaNhanVien == nv.MaNhanVien);
                if (existData != null)
                {
                    ViewBag.Error = "Đã tồn tại mã nhân viên này";
                    return View(nv);
                }
                else
                {
                    db.NhanViens.Add(nv);
                    db.SaveChanges();
                    uploadhinh = Request.Files["ImagesFile"];
                    if (uploadhinh != null && uploadhinh.ContentLength > 0)
                    {
                        int id = int.Parse(db.NhanViens.ToList().Last().MaNhanVien.ToString());

                        string _FileName = "";
                        int index = uploadhinh.FileName.IndexOf('.');
                        _FileName = "nhanvien" + id.ToString() + "." + uploadhinh.FileName.Substring(index + 1);
                        string _path = Path.Combine(Server.MapPath("~/Areas/UploadFile/NhanVien/"), _FileName);
                        uploadhinh.SaveAs(_path);

                        NhanVien nguoi = db.NhanViens.FirstOrDefault(x => x.MaNhanVien == id);
                        nguoi.Avatar = ("/Areas/UploadFile/NhanVien/" + _FileName);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu!" + ex.Message;
                return View(nv);
            }
        }

        // GET: Administrator/NhanVien/Edit/5
        public ActionResult Edit(int id)
        {
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // POST: Administrator/NhanVien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(NhanVien nv, HttpPostedFileBase uploadhinh)
        {
            NhanVien nvs = db.NhanViens.FirstOrDefault(x => x.MaNhanVien == nv.MaNhanVien);
            nvs.TenNhanVien = nv.TenNhanVien;
            nvs.DienThoai = nv.DienThoai;
            nvs.Email = nv.Email;
            nvs.DiaChi = nv.DiaChi;
            nvs.NgaySinh = nv.NgaySinh;
            nvs.GioiTinh = nv.GioiTinh;
            uploadhinh = Request.Files["ImagesFile"];
            if (uploadhinh != null && uploadhinh.ContentLength > 0)
            {
                int id = nv.MaNhanVien;
                string _FileName = "";
                int index = uploadhinh.FileName.IndexOf('.');
                _FileName = "nhanvien" + id.ToString() + "." + uploadhinh.FileName.Substring(index + 1);
                string _path = Path.Combine(Server.MapPath("~/Areas/UploadFile/NhanVien"), _FileName);
                uploadhinh.SaveAs(_path);
                nvs.Avatar = ("/Areas/UploadFile/NhanVien/" + _FileName);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Administrator/NhanVien/Delete/5
        public ActionResult Delete(int id)
        {
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // POST: Administrator/NhanVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NhanVien nhanVien = db.NhanViens.Find(id);
            try
            {
                db.NhanViens.Remove(nhanVien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Không xoá được bản ghi này!" + " " + ex.Message;
                return View(nhanVien);
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
