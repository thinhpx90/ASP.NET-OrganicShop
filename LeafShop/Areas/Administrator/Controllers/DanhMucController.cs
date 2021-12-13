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
    public class DanhMucController : Controller
    {
        private LeafShopDb db = new LeafShopDb();

        // GET: Administrator/DanhMuc
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
            //var danhmucs = db.DanhMucs.Select(d => d);
            IQueryable<DanhMuc> danhmucs = db.DanhMucs.Include("DanhMuc2").OrderBy(x => x.MaDanhMuc);
            if (!String.IsNullOrEmpty(SearchString))
            {
                danhmucs = danhmucs.Where(p => p.TenDanhMuc.Contains(SearchString));
            }
            int pageSize = 10;

            int pageNumber = (page ?? 1);
            return View(danhmucs.ToPagedList(pageNumber, pageSize));
        }

        // GET: Administrator/DanhMuc/Details/5
        public ActionResult Details(int id)
        {
            DanhMuc danhMuc = db.DanhMucs.Find(id);
            if (danhMuc == null)
            {
                return HttpNotFound();
            }
            return View(danhMuc);
        }

        // GET: Administrator/DanhMuc/Create
        public ActionResult Create()
        {
            ViewBag.ParentId = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc");
            return View();
        }

        // POST: Administrator/DanhMuc/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDanhMuc,TenDanhMuc,ParentId")] DanhMuc danhMuc)
        {
            try
            {
                ViewBag.ParentId = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", danhMuc.ParentId);

                DanhMuc existData = db.DanhMucs.FirstOrDefault(x => x.MaDanhMuc == danhMuc.MaDanhMuc);
                if (existData != null)
                {
                    ViewBag.Error = "Mã danh mục đã tồn tại";
                    return View(danhMuc);
                }
                else if (existData == null)
                {

                        db.DanhMucs.Add(danhMuc);
                        db.SaveChanges();

                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu!" + ex.Message;
                return View(danhMuc);
            }
            
        }

        // GET: Administrator/DanhMuc/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMuc danhMuc = db.DanhMucs.Find(id);
            if (danhMuc == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentId = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", danhMuc.ParentId);

            return View(danhMuc);
        }

        // POST: Administrator/DanhMuc/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDanhMuc,TenDanhMuc,ParentId")] DanhMuc danhMuc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(danhMuc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(danhMuc);
        }

        // GET: Administrator/DanhMuc/Delete/5
        public ActionResult Delete(int id)
        {
            DanhMuc danhMuc = db.DanhMucs.Find(id);
            if(danhMuc.ParentId != null)
            {
                DanhMuc dmCha = db.DanhMucs.Where(s => s.MaDanhMuc == danhMuc.ParentId).FirstOrDefault();
                ViewBag.tenDMCha = dmCha.TenDanhMuc;
            }    
            if (danhMuc == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentId = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", danhMuc.ParentId);
            return View(danhMuc);
        }

        // POST: Administrator/DanhMuc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DanhMuc danhMuc = db.DanhMucs.Find(id);
            try
            {
                db.DanhMucs.Remove(danhMuc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.Error = "Không xoá được bản ghi này: Bạn cần xoá sản phẩm trong danh mục trước khi xoá danh mục";
                return View("Delete", danhMuc);
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
