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
    public class BlogController : Controller
    {
        private LeafShopDb db = new LeafShopDb();

        // GET: Administrator/Blog
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
            //var blogs = db.Blogs.Select(d => d);
            IQueryable<Blog> blogs = (from bl in db.Blogs
                                            select bl).Include("DanhMucBlog").Include("NhanVien")
                    .OrderBy(x => x.MaBaiViet);
            if (!String.IsNullOrEmpty(SearchString))
            {
                blogs = blogs.Where(p => p.TieuDe.Contains(SearchString));
            }
            int pageSize = 5;

            int pageNumber = (page ?? 1);
            return View(blogs.ToPagedList(pageNumber, pageSize));
        }

        // GET: Administrator/Blog/Details/5
        public ActionResult Details(int id)
        {
            Blog blog = db.Blogs.Include("NhanVien").Include("DanhMucBlog").Where(x => x.MaBaiViet == id).FirstOrDefault();
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // GET: Administrator/Blog/Create
        public ActionResult Create()
        {
            ViewBag.MaNhanVien = new SelectList(db.NhanViens, "MaNhanVien", "TenNhanVien");
            ViewBag.MaDanhMucBlog = new SelectList(db.DanhMucBlogs, "MaDanhMucBlog", "TenDanhMucBlog");
            return View();
        }

        // POST: Administrator/Blog/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Blog blog, HttpPostedFileBase uploadhinh)
        {
            try
            {
                ViewBag.MaNhanVien = new SelectList(db.NhanViens, "MaNhanVien", "TenNhanVien", blog.MaNhanVien);
                ViewBag.MaDanhMucBlog = new SelectList(db.DanhMucBlogs, "MaDanhMucBlog", "TenDanhMucBlog");
                Blog existData = db.Blogs.Where(x => x.MaBaiViet == blog.MaBaiViet).FirstOrDefault();
                if (existData != null)
                {
                    ViewBag.Error = "Mã bài viết này đã tồn tại!";
                    return View(blog);
                }
                else if (existData == null)
                {
                    var data = new Blog
                    {
                        MaDanhMucBlog = blog.MaDanhMucBlog,
                        MaNhanVien = blog.MaNhanVien,
                        Noidung = blog.Noidung,
                        TieuDe = blog.TieuDe,
                        Tomtat = blog.Tomtat,
                        NgayKhoiTao = DateTime.Now,
                    };
                    db.Blogs.Add(data);
                    db.SaveChanges();
                    uploadhinh = Request.Files["ImageFile"];
                    if (uploadhinh != null && uploadhinh.ContentLength > 0)
                    {
                        int id = int.Parse(db.Blogs.ToList().Last().MaBaiViet.ToString());
                        string _FileName = "";
                        int index = uploadhinh.FileName.IndexOf('.');
                        _FileName = "blog" + id.ToString() + "." + uploadhinh.FileName.Substring(index + 1);
                        string _path = Path.Combine(Server.MapPath("~/Areas/UploadFile/Blog/"), _FileName);
                        uploadhinh.SaveAs(_path);

                        Blog blog1 = db.Blogs.FirstOrDefault(x => x.MaBaiViet == id);
                        blog1.Anh = ("/Areas/UploadFile/Blog/" + _FileName);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu!" + ex.Message;
                return View(blog);
            }
        }

        // GET: Administrator/Blog/Edit/5
        public ActionResult Edit(int id)
        {
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNhanVien = new SelectList(db.NhanViens, "MaNhanVien", "TenNhanVien", blog.MaNhanVien);
            ViewBag.MaDanhMucBlog = new SelectList(db.DanhMucBlogs, "MaDanhMucBlog", "TenDanhMucBlog");
            return View(blog);
        }

        // POST: Administrator/Blog/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Blog blog, HttpPostedFileBase uploadhinh)
        {
            Blog bls = db.Blogs.FirstOrDefault(x => x.MaBaiViet == blog.MaBaiViet);
            bls.Noidung = blog.Noidung;
            bls.TieuDe = blog.TieuDe;
            bls.Tomtat = blog.Tomtat;
            bls.MaDanhMucBlog = blog.MaDanhMucBlog;
            uploadhinh = Request.Files["ImageFile"];
            if (uploadhinh != null && uploadhinh.ContentLength > 0)
            {
                int id = blog.MaBaiViet;
                string _FileName = "";
                int index = uploadhinh.FileName.IndexOf('.');
                _FileName = "blog" + id.ToString() + "." + uploadhinh.FileName.Substring(index + 1);
                string _path = Path.Combine(Server.MapPath("~/Areas/UploadFile/Blog"), _FileName);
                uploadhinh.SaveAs(_path);
                bls.Anh = ("/Areas/UploadFile/Blog/" + _FileName);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Administrator/Blog/Delete/5
        public ActionResult Delete(int id)
        {
            Blog blog = db.Blogs.Include("NhanVien").Include("DanhMucBlog").Where(s => s.MaBaiViet == id).FirstOrDefault();
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: Administrator/Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Blog blog = db.Blogs.Find(id);
            try
            {
                db.Blogs.Remove(blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.Error = "Không xoá được bản ghi này: Bạn cần xoá sản phẩm danh mục blog trước khi xoá bài viết!";
                return View("Delete", blog);
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
