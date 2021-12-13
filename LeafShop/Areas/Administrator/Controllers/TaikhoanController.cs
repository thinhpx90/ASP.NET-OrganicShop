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
    public class TaikhoanController : Controller
    {
        private LeafShopDb db = new LeafShopDb();

        // GET: Administrator/Taikhoan

        //public ActionResult Index()
        //{
        //    var taikhoans = db.Taikhoans.Include(t => t.NhanVien);
        //    return View(taikhoans.ToList());
        //}

        public ActionResult Index(string SearchString, string currentFilter, int? page)
        {
            //var taikhoans = db.Taikhoans.Include(t => t.NhanVien);
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
            IQueryable<Taikhoan> taikhoans = (from tk in db.Taikhoans select tk).Include(t => t.NhanVien).OrderBy(x => x.USERNAME);

            if (!String.IsNullOrEmpty(SearchString))
            {
                taikhoans = taikhoans.Where(p => p.USERNAME.Contains(SearchString));
            }
            int pageSize = 5;

            int pageNumber = (page ?? 1);
            return View(taikhoans.ToPagedList(pageNumber, pageSize));
        }

        // GET: Administrator/Taikhoan/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Taikhoan taikhoan = db.Taikhoans.Find(id);
            if (taikhoan == null)
            {
                return HttpNotFound();
            }
            return View(taikhoan);
        }

        // GET: Administrator/Taikhoan/Create
        public ActionResult Create()
        {
            ViewBag.MaNhanVien = new SelectList(db.NhanViens, "MaNhanVien", "TenNhanVien");
            return View();
        }

        // POST: Administrator/Taikhoan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "USERNAME,PASSWORD,Quantri,MaNhanVien")] Taikhoan taikhoan)
        {
            try
            {
                ViewBag.MaNhanVien = new SelectList(db.NhanViens, "MaNhanVien", "TenNhanVien", taikhoan.MaNhanVien);
                Taikhoan existData = db.Taikhoans.FirstOrDefault(x => x.MaNhanVien == taikhoan.MaNhanVien);

                if (existData != null)
                {
                    ViewBag.Error = "Nhân viên này đã có tài khoản";
                    return View(taikhoan);
                }
                else if (existData == null)
                {
                    if (ModelState.IsValid)
                    {
                        db.Taikhoans.Add(taikhoan);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu!" + ex.Message;
                return View(taikhoan);
            }
        }

        // GET: Administrator/Taikhoan/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Taikhoan taikhoan = db.Taikhoans.Find(id);
            if (taikhoan == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNhanVien = new SelectList(db.NhanViens, "MaNhanVien", "TenNhanVien", taikhoan.MaNhanVien);
            return View(taikhoan);
        }

        // POST: Administrator/Taikhoan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "USERNAME,PASSWORD,Quantri,MaNhanVien")] Taikhoan taikhoan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taikhoan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaNhanVien = new SelectList(db.NhanViens, "MaNhanVien", "TenNhanVien", taikhoan.MaNhanVien);
            return View(taikhoan);
        }

        // GET: Administrator/Taikhoan/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Taikhoan taikhoan = db.Taikhoans.Include("NhanVien").Where(s => s.USERNAME == id).FirstOrDefault();
            if (taikhoan == null)
            {
                return HttpNotFound();
            }
            return View(taikhoan);
        }

        // POST: Administrator/Taikhoan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Taikhoan taikhoan = db.Taikhoans.Find(id);
            db.Taikhoans.Remove(taikhoan);
            db.SaveChanges();
            return RedirectToAction("Index");
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
