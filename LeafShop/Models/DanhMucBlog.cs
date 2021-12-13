namespace LeafShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DanhMucBlog")]
    public partial class DanhMucBlog
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DanhMucBlog()
        {
            Blogs = new HashSet<Blog>();
        }

        [Key]
        public int MaDanhMucBlog { get; set; }

        [Required(ErrorMessage = "Tên danh mục blog không được để trống")]
        [StringLength(100)]
        public string TenDanhMucBlog { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Blog> Blogs { get; set; }
    }
}
