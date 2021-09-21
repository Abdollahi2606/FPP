using MyEshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyEshop.Controllers
{
    public class Item
    {
        // آیتم هایی که قرار است در سبد خرید قرار بگیرند به عبارتی سفارشات ما هستند
        public int Id { get; set; }
        // یک پروداکت دارد یعنی کدام محصول است   
        //  قیمت  
        public  decimal Price { get; set; }
        // تعداد
        public int QuantityInStock { get; set; }
        //Navigation Property
        public Product Product { get; set; }
    }
}
