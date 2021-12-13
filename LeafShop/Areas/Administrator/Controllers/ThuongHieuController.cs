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
    public class ThuongHieuController : Controller
    {
        private LeafShopDb db = new LeafShopDb();

        // GET: Administrator/ThuongHieu
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
            IQueryable<ThuongHieu> thuonghieus = (from th in db.ThuongHieux
                                                  select th)
                    .OrderBy(x => x.MaThuongHieu);
            if (!String.IsNullOrEmpty(SearchString))
            {
                thuonghieus = thuonghieus.Where(p => p.TenThuongHieu.Contains(SearchString));
            }
            int pageSize = 5;

            int pageNumber = (page ?? 1);
            return View(thuonghieus.ToPagedList(pageNumber, pageSize));
        }

        // GET: Administrator/ThuongHieu/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThuongHieu thuongHieu = db.ThuongHieux.Find(id);
            if (thuongHieu == null)
            {
                return HttpNotFound();
            }
            return View(thuongHieu);
        }

        // GET: Administrator/ThuongHieu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administrator/ThuongHieu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ThuongHieu th, HttpPostedFileBase uploadhinh)
        {
            try
            {
                ThuongHieu existData = db.ThuongHieux.FirstOrDefault(x => x.MaThuongHieu == th.MaThuongHieu);
                if (existData != null)
                {
                    ViewBag.Error = "Đã tồn tại mã thương hiệu này";
                    return View(th);
                }
                else
                {
                    db.ThuongHieux.Add(th);
                    db.SaveChanges();
                    uploadhinh = Request.Files["ImageFile"];
                    if (uploadhinh != null && uploadhinh.ContentLength > 0)
                    {
                        int id = int.Parse(db.ThuongHieux.ToList().Last().MaThuongHieu.ToString());

                        string _FileName = "";
                        int index = uploadhinh.FileName.IndexOf('.');
                        _FileName = "thuonghieu" + id.ToString() + "." + uploadhinh.FileName.Substring(index + 1);
                        string _path = Path.Combine(Server.MapPath("~/Areas/UploadFile/ThuongHieu/"), _FileName);
                        uploadhinh.SaveAs(_path);

                        ThuongHieu item = db.ThuongHieux.FirstOrDefault(x => x.MaThuongHieu == id);
                        item.AnhThuongHieu = ("/Areas/UploadFile/ThuongHieu/" + _FileName);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu!" + ex.Message;
                return View(th);
            }
        }

        // GET: Administrator/ThuongHieu/Edit/5
        public ActionResult Edit(int id)
        {
            ThuongHieu thuongHieu = db.ThuongHieux.Find(id);
            if (thuongHieu == null)
            {
                return HttpNotFound();
            }
            return View(thuongHieu);
        }

        // POST: Administrator/ThuongHieu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ThuongHieu th, HttpPostedFileBase uploadhinh)
        {
            ThuongHieu ths = db.ThuongHieux.FirstOrDefault(x => x.MaThuongHieu == th.MaThuongHieu);
            ths.TenThuongHieu = th.TenThuongHieu;
            ths.DiaChiThuongHieu = th.DiaChiThuongHieu;
            ths.DienThoaiThuongHieu = th.DienThoaiThuongHieu;
            ths.MoTaThuongHieu = th.MoTaThuongHieu;

            uploadhinh = Request.Files["ImageFile"];
            if (uploadhinh != null && uploadhinh.ContentLength > 0)
            {
                int id = th.MaThuongHieu;
                string _FileName = "";
                int index = uploadhinh.FileName.IndexOf('.');
                _FileName = "thuonghieu" + id.ToString() + "." + uploadhinh.FileName.Substring(index + 1);
                string _path = Path.Combine(Server.MapPath("~/Areas/UploadFile/ThuongHieu"), _FileName);
                uploadhinh.SaveAs(_path);
                ths.AnhThuongHieu = ("/Areas/UploadFile/ThuongHieu/" + _FileName);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Administrator/ThuongHieu/Delete/5
        public ActionResult Delete(int id)
        {
            ThuongHieu thuongHieu = db.ThuongHieux.Find(id);
            if (thuongHieu == null)
            {
                return HttpNotFound();
            }
            return View(thuongHieu);
        }

        // POST: Administrator/ThuongHieu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ThuongHieu thuongHieu = db.ThuongHieux.Find(id);
            try
            {
                db.ThuongHieux.Remove(thuongHieu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }catch(Exception ex)
            {
                ViewBag.Error = "Không thể xóa bản ghi này " + ex.Message;
                return View("Delete", thuongHieu);
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
