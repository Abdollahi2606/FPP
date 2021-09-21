using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyEshop.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public int UserId { get; set; } // کدام کاربر این فاکتور را خریده
        [Required]
        public DateTime CreateDate { get; set; }
        
        public bool  IsFinaly { get; set; } // برای اینمه اگر فاکتور به درگاه پرداخت رفت و پرداخت شد،دیگر فاکنور قابل تغییر نباشد

        [ForeignKey("UserId")]
        public Users Users { get; set; } // ارتباط فاکتور با جدول یوزر یعنی هر فاکتور فقط مزبوط به یک کاربراست

        public  List<OrderDetail> OrderDetails { get; set; } // هر فاکتور می تواند چند ریز فاکتور داشته باشد
    }
}
