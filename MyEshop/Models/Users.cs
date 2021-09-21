using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyEshop.Models
{
    public class Users
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        [MaxLength(300)]
        [Display(Name ="ایمیل")]
        public string Email { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "رمزعبور")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "تاریخ ثبت نام")]
        public DateTime RegisterDate { get; set; }
        [Display(Name = "ادمین است؟")]
        public bool  IsAdmin { get; set; }

        public List<Order> Orders{ get; set; } // ارتباط چند به یک فاکتور با جدول یوزر یعنی هر یوزر می تواند چند فاکتور داشته باشد
    }
}
