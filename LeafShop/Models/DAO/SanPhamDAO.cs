using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeafShop.Models.DAO
{
    public class SanPhamDAO
    {
        public LeafShopDb dbContext = null;
        public SanPhamDAO()
        {
            dbContext = new LeafShopDb();
        }

        //public List<SanPham> ListAll(string id)
    }
}