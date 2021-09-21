using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyEshop.Data;
using MyEshop.Models;

namespace MyEshop.Controllers
{
    public class ProductController : Controller
    {
        private MyEshopContext _context;
        public ProductController(MyEshopContext context)
        {
            _context = context;
        }

        //[Authorize] // با این صفت،اکشن زیر هروقت صدا زده شود، کاربر را ملزم به لاگین میکند
        //public IActionResult AddToCart(int itemId)
        //{
        //    var product = _context.Products.Include(navigationPropertyPath: p => p.Item)
        //        .SingleOrDefault(p => p.ItemId == itemId);
        //    if (product != null)
        //    {
        //        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
        //        var order = _context.Orders.FirstOrDefault(o => o.UserId == userId && !o.IsFinaly);
        //        if (order != null)
        //        {
        //            //کاربر فاکتور  باز دارد
        //            var orderDetail =
        //                _context.OrderDetails.FirstOrDefault(d =>
        //                    d.OrderId == order.OrderId && d.ProductId == product.Id);
        //            if (orderDetail != null)
        //            {
        //                // یک محصول درسبد خرید کاربر پیدا کردم، همین محصول را
        //                orderDetail.Count += 1;
        //            }

        //            else
        //            {
        //                // محصول را در سبد خرید کاربر پیدا نکردم بنابرایت باید نیو شود
        //                _context.OrderDetails.Add(new OrderDetail()
        //                {
        //                    OrderId = order.OrderId,// ایدی را از اوردر نمونه سازی شده درخط 58 میگیرد
        //                    ProductId = product.Id,// آیدی محصول را از پروداکت خط 46 میگیرد
        //                    Price = product.Item.Price,
        //                    Count = 1
        //                }
        //           );
        //            }

        //        }
        //        else
        //        {
        //            // کاربر فاکتور بازی ندارد
        //            order = new Order()
        //            {
        //                IsFinaly = false,
        //                CreateDate = DateTime.Now,
        //                UserId = userId
        //            };
        //            _context.Orders.Add(order);
        //            _context.SaveChanges();
        //            _context.OrderDetails.Add(new OrderDetail()
        //            {
        //                OrderId = order.OrderId,// ایدی را از اوردر نمونه سازی شده درخط 58 میگیرد
        //                ProductId = product.Id,// آیدی محصول را از پروداکت خط 46 میگیرد
        //                Price = product.Item.Price,
        //                Count = 1
        //            }
        //            );
        //        }

        //        _context.SaveChanges();
        //    }
        //    return RedirectToAction("ShowCart");
        //}
        [Route("Group/{id}/{name}")]
        public IActionResult ShowProductByGroupId(int id, string name)
        {
            ViewData["GroupName"] = name;
            var products = _context.CategoryToProducts
                .Where(c => c.CategoryId == id)
                .Include(c => c.Product)
                .Select(c => c.Product)
                .ToList();
            return View(products);
        }
    }
}