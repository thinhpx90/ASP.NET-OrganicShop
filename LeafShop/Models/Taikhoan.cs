namespace LeafShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Taikhoan")]
    public partial class Taikhoan
    {
        [Key]
        [StringLength(50)]
        [Required(ErrorMessage = "Username không được để trống")]
        public string USERNAME { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(50)]
        public string PASSWORD { get; set; }

        public bool Quantri { get; set; }

        public int? MaNhanVien { get; set; }

        public virtual NhanVien NhanVien { get; set; }
    }
}
