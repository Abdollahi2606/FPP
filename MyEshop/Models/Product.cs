using MyEshop.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyEshop.Models
{
    public class Product
    {
        //public Product()
        //{
        //    // برای استفاده از لیستی از کتگوری ها باید آن را در کانستراکتور نمونه سازی کنیم
        //    Categories = new List<Category>();
        //}
        // صفت های محصولات
        // کلید اصلی محصول
        public int Id { get; set; }
        // نام محصول
        public string Name { get; set; }
        // توضبح محصول
        public string Description { get; set; }
        // لیسنی از نوع گروه یعنی محصول می تواند در کتگوری های مختلفی باشد
        //public List<Category> Categories { get; set; }
        // کلید خارجی
        public int ItemId { get; set; }
        


        //Relation Or Navigation Property
        public ICollection<CategoryToProduct> CategoryToProducts { get; set; }
        public Item Item { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } // هر محصول می تواند در چند ریز فاکتور تکرار شود
    }
}
