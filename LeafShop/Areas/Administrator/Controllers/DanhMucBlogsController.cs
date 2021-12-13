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
    public class DanhMucBlogsController : Controller
    {
        private LeafShopDb db = new LeafShopDb();

        // GET: Administrator/DanhMucBlogs
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
            IQueryable<DanhMucBlog> taikhoans = (from dm in db.DanhMucBlogs
                                              select dm)
                    .OrderBy(x => x.MaDanhMucBlog);
            if (!String.IsNullOrEmpty(SearchString))
            {
                taikhoans = taikhoans.Where(p => p.TenDanhMucBlog.Contains(SearchString));
            }
            int pageSize = 5;

            int pageNumber = (page ?? 1);
            return View(taikhoans.ToPagedList(pageNumber, pageSize));
        }

        // GET: Administrator/DanhMucBlogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMucBlog danhMucBlog = db.DanhMucBlogs.Find(id);
            if (danhMucBlog == null)
            {
                return HttpNotFound();
            }
            return View(danhMucBlog);
        }

        // GET: Administrator/DanhMucBlogs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administrator/DanhMucBlogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDanhMucBlog,TenDanhMucBlog")] DanhMucBlog danhMucBlog)
        {
            if (ModelState.IsValid)
            {
                db.DanhMucBlogs.Add(danhMucBlog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(danhMucBlog);
        }

        // GET: Administrator/DanhMucBlogs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMucBlog danhMucBlog = db.DanhMucBlogs.Find(id);
            if (danhMucBlog == null)
            {
                return HttpNotFound();
            }
            return View(danhMucBlog);
        }

        // POST: Administrator/DanhMucBlogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDanhMucBlog,TenDanhMucBlog")] DanhMucBlog danhMucBlog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(danhMucBlog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(danhMucBlog);
        }

        // GET: Administrator/DanhMucBlogs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMucBlog danhMucBlog = db.DanhMucBlogs.Find(id);
            if (danhMucBlog == null)
            {
                return HttpNotFound();
            }
            return View(danhMucBlog);
        }

        // POST: Administrator/DanhMucBlogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DanhMucBlog danhMucBlog = db.DanhMucBlogs.Find(id);
            try
            {
                db.DanhMucBlogs.Remove(danhMucBlog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }catch(Exception ex)
            {
                ViewBag.Error = "Không thể xóa bản ghi này " + ex.Message;
                return View("Delete", danhMucBlog);
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
