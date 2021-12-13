using LeafShop.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LeafShop.Controllers
{
    public class SanPhamController : Controller
    {
        LeafShopDb db = new LeafShopDb();
        // GET: SanPham
        public ActionResult Index(int? page,string searchString, string sortOrder, string currentFilter)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SapTheoTenAZ = "ten_asc";
            ViewBag.SapTheoTenZA = "ten_desc";
            ViewBag.GiaTangDan = "gia_asc";
            ViewBag.GiaGiamDan = "gia_desc";
            ViewBag.SapTheoNgaycCuNhat = "ngay_desc";
            ViewBag.SapTheoNgaycMoiNhat = "ngay_asc";
            ViewBag.SapTheoSoLuongBan = "slb_asc";
            ViewBag.SapTheoSoLuong = "sl_asc";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            var listall = db.SanPhams.ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                listall = listall.Where(p => p.TenSanPham.Contains(searchString)).ToList();
            }
            ViewBag.dmSP = db.DanhMucs.Include("DanhMuc1").Where(p => p.DanhMuc2 == null).Select(p => p).ToList();
            ViewBag.spNoiBat = db.SanPhams.OrderByDescending(s => s.SoLuongBan).Take(3).ToList();
            switch (sortOrder)
            {
                case "ten_desc":
                    listall = listall.OrderByDescending(s => s.TenSanPham).ToList();
                    break;
                case "gia_asc":
                    listall = listall.OrderBy(s => s.DonGia).ToList();
                    break;
                case "gia_desc":
                    listall = listall.OrderByDescending(s => s.DonGia).ToList();
                    break;
                case "ngay_desc":
                    listall = listall.OrderByDescending(s => s.NgayKhoiTao).ToList();
                    break;
                case "ngay_asc":
                    listall = listall.OrderBy(s => s.NgayKhoiTao).ToList();
                    break;
                case "slb_asc":
                    listall = listall.OrderBy(s => s.SoLuongBan).ToList();
                    break;
                case "sl_asc":
                    listall = listall.OrderBy(s => s.SoLuong).ToList();
                    break;
                default:
                    listall = listall.OrderBy(s => s.TenSanPham).ToList();
                    break;
            }
            ViewBag.totalSP = listall.Count;

            int pageSize = 12;
            int pageNumber = (page ?? 1);
            return View(listall.ToPagedList(pageNumber,pageSize));
        }
        public ActionResult Category(int? id,int? page, string sortOrder)
        {
            
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SapTheoTenAZ = "ten_asc";
            ViewBag.SapTheoTenZA = "ten_desc";
            ViewBag.GiaTangDan = "gia_asc";
            ViewBag.GiaGiamDan = "gia_desc";
            ViewBag.SapTheoNgaycCuNhat = "ngay_desc";
            ViewBag.SapTheoNgaycMoiNhat = "ngay_asc";
            ViewBag.SapTheoSoLuongBan = "slb_asc";
            ViewBag.SapTheoSoLuong = "sl_asc";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            DanhMuc getTen = db.DanhMucs.Where(x => x.MaDanhMuc == id).FirstOrDefault();
            ViewBag.nameCategory = getTen.TenDanhMuc;
            var product = db.SanPhams.Where(s => s.MaDanhMuc == id).ToList();

            var dsDMCon = db.DanhMucs.Where(x => x.ParentId == id).ToList();
            if(dsDMCon.Count > 0)
            {
                foreach (DanhMuc dmCon in dsDMCon)
                {
                    var sp1 = db.SanPhams.Where(s => s.MaDanhMuc == dmCon.MaDanhMuc).ToList();
                    product = product.Concat(sp1).ToList();
                }
                int idCha = dsDMCon.FirstOrDefault().MaDanhMuc;
                var dsDMChau = db.DanhMucs.Where(x => x.ParentId == idCha).ToList();
                if (dsDMChau.Count > 0)
                {
                    int idChau = dsDMChau.FirstOrDefault().MaDanhMuc;
                    var sp2 = db.SanPhams.Where(s => s.MaDanhMuc == idChau).ToList();
                    product = product.Concat(sp2).ToList();
                }    
            }
            ViewBag.dmSP =  db.DanhMucs.Include("DanhMuc1").Where(p => p.DanhMuc2 == null).Select(p => p).ToList();
            switch (sortOrder)
            {
                case "ten_desc":
                    product = product.OrderByDescending(s => s.TenSanPham).ToList();
                    break;
                case "gia_asc":
                    product = product.OrderBy(s => s.DonGia).ToList();
                    break;
                case "gia_desc":
                    product = product.OrderByDescending(s => s.DonGia).ToList();
                    break;
                case "ngay_desc":
                    product = product.OrderByDescending(s => s.NgayKhoiTao).ToList();
                    break;
                case "ngay_asc":
                    product = product.OrderBy(s => s.NgayKhoiTao).ToList();
                    break;
                case "slb_asc":
                    product = product.OrderBy(s => s.SoLuongBan).ToList();
                    break;
                case "sl_asc":
                    product = product.OrderBy(s => s.SoLuong).ToList();
                    break;
                default:
                    product = product.OrderBy(s => s.TenSanPham).ToList();
                    break;
            }
            ViewBag.spNoiBat = db.SanPhams.OrderByDescending(s => s.SoLuongBan).Take(3).ToList();
            ViewBag.totalSP = product.Count;    
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            return View(product.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ThuongHieu(int? id, int? page, string sortOrder)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.SapTheoTenAZ = "ten_asc";
            ViewBag.SapTheoTenZA = "ten_desc";
            ViewBag.GiaTangDan = "gia_asc";
            ViewBag.GiaGiamDan = "gia_desc";
            ViewBag.SapTheoNgaycCuNhat = "ngay_desc";
            ViewBag.SapTheoNgaycMoiNhat = "ngay_asc";
            ViewBag.SapTheoSoLuongBan = "slb_asc";
            ViewBag.SapTheoSoLuong = "sl_asc";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ThuongHieu getTen = db.ThuongHieux.Where(x => x.MaThuongHieu == id).FirstOrDefault();
            ViewBag.nameCategory = getTen.TenThuongHieu;
            var product = db.SanPhams.Where(s => s.MaThuongHieu == id).ToList();

            ViewBag.dmSP = db.DanhMucs.Include("DanhMuc1").Where(p => p.DanhMuc2 == null).Select(p => p).ToList();

            switch (sortOrder)
            {
                case "ten_desc":
                    product = product.OrderByDescending(s => s.TenSanPham).ToList();
                    break;
                case "gia_asc":
                    product = product.OrderBy(s => s.DonGia).ToList();
                    break;
                case "gia_desc":
                    product = product.OrderByDescending(s => s.DonGia).ToList();
                    break;
                case "ngay_desc":
                    product = product.OrderByDescending(s => s.NgayKhoiTao).ToList();
                    break;
                case "ngay_asc":
                    product = product.OrderBy(s => s.NgayKhoiTao).ToList();
                    break;
                case "slb_asc":
                    product = product.OrderBy(s => s.SoLuongBan).ToList();
                    break;
                case "sl_asc":
                    product = product.OrderBy(s => s.SoLuong).ToList();
                    break;
                default:
                    product = product.OrderBy(s => s.TenSanPham).ToList();
                    break;
            }
            ViewBag.spNoiBat = db.SanPhams.OrderByDescending(s => s.SoLuongBan).Take(3).ToList();
            ViewBag.totalSP = product.Count;
            ViewBag.thuongHieu = db.ThuongHieux.Where(s => s.MaThuongHieu == id).FirstOrDefault();
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            return View(product.ToPagedList(pageNumber, pageSize));
        }
        [ChildActionOnly]
        public ActionResult CategoryTreeProduct()
        {
            IEnumerable<DanhMuc> danhmucs = db.DanhMucs.Include("DanhMuc1").Where(p => p.DanhMuc2 == null).Select(p => p);

            return PartialView("CategoryTreeProduct", danhmucs);
        }
        public ActionResult Detail(int id)
        {
            SanPham sp = db.SanPhams.Include("ThuongHieu").Include("DanhMuc").Where(x => x.MaSanPham == id).FirstOrDefault();
            //List<SanPham> list = db.SanPhams.Where(s => s.MaSanPham.Equals(id)).ToList();
            //ViewBag.ChiTietSanPham = list;
            //ViewBag.Exitst = list[0];
            ViewBag.spNoiBat = db.SanPhams.OrderByDescending(s => s.SoLuongBan).Take(3).ToList();
            ViewBag.spLienQuan = db.SanPhams.Where(s => s.MaDanhMuc == sp.MaDanhMuc).Take(3).ToList();
            ViewBag.thuongHieu = db.ThuongHieux.Where(s => s.MaThuongHieu == sp.MaThuongHieu).FirstOrDefault();
            return View(sp);
        }

        [HttpPost]
        public JsonResult getProductDetail(int masp)
        {
            SanPham sp = db.SanPhams.Where(x => x.MaSanPham == masp).FirstOrDefault();
            return Json(sp, JsonRequestBehavior.AllowGet);
        }
    }
}