using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyEshop.Models
{
    public class OrderDetail
    {
        // جدول ریز فاکتور
        [Key]
        public int DetailId { get; set; }
        [Required]
        public int OrderId { get; set; } // ریز فاکتور هر محصول متعلق به کدام فاکتور است
        [Required]
        public int ProductId { get; set; } // چون کلید اصلی جدول پروداکت، ایدی است و همنام این صفت نیست باید آن را به صورت کلید خارجی تعریف کنیم
        [Required]
        public decimal Price { get; set; } // برای اینکه قیمت محصول مان که در مدل آیتم فرار دادیم عوض نشود
        [Required]
        public int Count { get; set; }
        


        // navigation 
        public Order Order { get; set; }//  ار تباط یک به چند فاکتور با ریز فاکتور
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
