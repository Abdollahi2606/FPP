using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyEshop.Models
{
    public class AddEditProductViewModel
    {
        public int Id { get; set; }
        [Display(Name="نام کالا")]
        public string Name { get; set; }
        [Display(Name = "توضیحات")]
       public string Description { get; set; }
        [Display(Name = "قیمت کالا")]
        public decimal Price { get; set; }
        [Display(Name = "تعداد کالا")]
        public int QuantityInStock { get; set; }
        [Display(Name = "عکس کالا")]
        // این پراپرتی برای تفریف هرنوع فابلی اسن که میخواهیم آپلوئ کنیم
        public IFormFile Picture { get; set; }
        // این پراپرتی برای این اسن همه پروه ها را نمایش دهد و چون قابل نمایش هنگام صدا زدن 
        // متد ادد استیعنی قرار نیست چیزی را به آن بفرستیم در متد گت آن را باید ازبانک پر کنیم
        public List<Category> Categories { get; set; }
       
    }
}
