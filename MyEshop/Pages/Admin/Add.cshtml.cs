using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyEshop.Controllers;
using MyEshop.Data;
using MyEshop.Models;

namespace MyEshop.Pages.Admin
{
    public class AddModel : PageModel
    {
        private MyEshopContext _context;
        public AddModel(MyEshopContext context)
        {
            _context = context;
        }
        [BindProperty] // این صفت باعث می شود تمامی پراپرتی های شی زیر به اینپوت های داخل صخحه بچسبد
        public AddEditProductViewModel Product { get; set; }

        // ایم پراپرتی مربوط به چک باکس های روی فرم است یعنی با انتخاب هر چک باکسی مشخص
        // کند که محصول امتخاب شده میتواند در این گروه قرار بگیرد
        // و مقدار برگشتی آن مقدار آی دی آن گروه باشد
        // درحقیقت می خواهیم مخصولی که انتخاب میکنیم را به همراه گروههایی که برایش در نظر میگیریم
        // در بانک ذخیره کنیم
        [BindProperty]
        public List<int> selectedGroups { get; set; }
        public void OnGet()
        {
            Product = new AddEditProductViewModel()
            {
                Categories = _context.Categories.ToList()
            };
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var item = new Item()
            {
                Price = Product.Price,
                QuantityInStock = Product.QuantityInStock
            };
            _context.Add(item);
            _context.SaveChanges();

            var pro = new Product()
            {
                Name = Product.Name,
                Item = item,
                Description = Product.Description
            };
            _context.Add(pro);
            _context.SaveChanges();

            //pro.ItemId = pro.Id;
            //_context.SaveChanges();
            // اگر فایلی در ویومدل پروداکت انتخاب شده بود و حجم یا طول فایل بیشتر از صفر بود
            if (Product.Picture?.Length > 0)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    pro.Id + Path.GetExtension(Product.Picture.FileName)
                    );
                // برای ذخیره کردن فایل ها یوزینگ میکنیم و محل ذخیره فایل و بعد مود ذخیره فایل را تعیین میکنیم
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    
                    Product.Picture.CopyTo(stream);
                }
            }

            // باید چک باکس ها را در تیبل بچینیم بااین شرط آیا پیزی انتخاب شده یا چچیزی در آن گروه هست
            if (selectedGroups.Any() && selectedGroups.Count > 0)
            {
                // اگر انتخاب شد به تعداد انتخاب شده ها در مدل گروهی از محصولات ،آیدی 
                //  گروه انتخاب شده و آیدی محصول راکه بعد از سیو چنج خط 62 داریم یه
                // جدول گروهی از محصولات در بانک اضافه کن
                foreach (var gr in selectedGroups)
                {
                    _context.CategoryToProducts.Add(new CategoryToProduct()
                    {
                        CategoryId =gr,
                        ProductId=pro.Id
                    });
                }
                _context.SaveChanges();
            }

            return RedirectToPage("Index");
        }
    }
}