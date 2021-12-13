namespace LeafShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    [Table("Blog")]
    public partial class Blog
    {
        [Key]
        public int MaBaiViet { get; set; }

        public int? MaNhanVien { get; set; }

        public int? MaDanhMucBlog { get; set; }

        [StringLength(500)]
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string TieuDe { get; set; }

        [StringLength(1000)]
        public string Anh { get; set; }

        [StringLength(500)]
        [AllowHtml]
        [Required(ErrorMessage = "Tóm tắt không được trống")]
        public string Tomtat { get; set; }

        [StringLength(2000)]
        [AllowHtml]
        [Required(ErrorMessage = "Nội dung không được để trống")]
        public string Noidung { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString =
        "{0:yyyy-MM-dd}",
        ApplyFormatInEditMode = true)]
        public DateTime? NgayKhoiTao { get; set; }
        [ScriptIgnore]
        public virtual NhanVien NhanVien { get; set; }

        public virtual DanhMucBlog DanhMucBlog { get; set; }
    }
}
