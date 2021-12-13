using LeafShop.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeafShop.Controllers
{
    public class BlogController : Controller
    {
        LeafShopDb db = new LeafShopDb();
        // GET: Blog
        public ActionResult Index(int? page,string searchString)
        {
            if (searchString != null)
            {
                page = 1;
            }
            ViewBag.CurrentFilter = searchString;
         
            var blogs = db.Blogs.Include("NhanVien").ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                blogs = blogs.Where(p => p.TieuDe.Contains(searchString)).ToList();
            }
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            ViewBag.listDM = db.DanhMucBlogs.ToList();
            ViewBag.blogMoiNhat = db.Blogs.ToList().Take(3);
            return View(blogs.ToPagedList(pageNumber,pageSize));
        }
        public ActionResult Detail(int id)
        {
            Blog bl = db.Blogs.Include("NhanVien").Include("DanhMucBlog").Where(s => s.MaBaiViet == id).FirstOrDefault();
            //List<SanPham> list = db.SanPhams.Where(s => s.MaSanPham.Equals(id)).ToList();
            //ViewBag.ChiTietSanPham = list;
            //ViewBag.Exitst = list[0];
            ViewBag.listDM = db.DanhMucBlogs.ToList();
            ViewBag.blogMoiNhat = db.Blogs.Take(3).ToList();
            ViewBag.blogLienQuan = db.Blogs.Where(s => s.MaDanhMucBlog == bl.MaDanhMucBlog && s.MaBaiViet != id).Take(3).ToList();
            return View(bl);
        }
        public ActionResult Category(int id, int? page)
        {
            var blogs = db.Blogs.Include("NhanVien").Where(s => s.MaDanhMucBlog == id).ToList();
            var blogFirst = db.DanhMucBlogs.Where(s => s.MaDanhMucBlog == id).FirstOrDefault();
            ViewBag.TenDanhMuc = blogFirst.TenDanhMucBlog;
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            ViewBag.listDM = db.DanhMucBlogs.ToList();
            ViewBag.blogMoiNhat = db.Blogs.ToList().Take(3);
            return View(blogs.ToPagedList(pageNumber, pageSize));
        }
    }
}