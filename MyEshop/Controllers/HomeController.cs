using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyEshop.Data;
using MyEshop.Models;
using MyEshop.Models.ViewModels;
using ZarinpalSandbox;



namespace MyEshop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MyEshopContext _context;

        public HomeController(ILogger<HomeController> logger, MyEshopContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products
              .ToList();

            return View(products);
        }

        [Route(template: "ContactUs")]
        public IActionResult ContactUs()
        {
            return View();
        }
        [Route(template: "AboutUs")]
        public IActionResult AboutUs()
        {
            return View();
        }

        [Authorize] // با این صفت،اکشن زیر هروقت صدا زده شود، کاربر را ملزم به لاگین میکند
        public IActionResult AddToCart(int itemId)
        {
            var product = _context.Products.Include(navigationPropertyPath: p => p.Item)
                .SingleOrDefault(p => p.ItemId == itemId);
            if (product != null)
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
                var order = _context.Orders.FirstOrDefault(o => o.UserId == userId && !o.IsFinaly);
                if (order != null)
                {
                    //کاربر فاکتور  باز دارد
                    var orderDetail =
                        _context.OrderDetails.FirstOrDefault(d =>
                            d.OrderId == order.OrderId && d.ProductId == product.Id);
                    if (orderDetail != null)
                    {
                        // یک محصول درسبد خرید کاربر پیدا کردم، همین محصول را
                        orderDetail.Count += 1;
                    }

                    else
                    {
                        // محصول را در سبد خرید کاربر پیدا نکردم بنابرایت باید نیو شود
                        _context.OrderDetails.Add(new OrderDetail()
                        {
                            OrderId = order.OrderId,// ایدی را از اوردر نمونه سازی شده درخط 58 میگیرد
                            ProductId = product.Id,// آیدی محصول را از پروداکت خط 46 میگیرد
                            Price = product.Item.Price,
                            Count = 1
                        }
                   );
                    }

                }
                else
                {
                    // کاربر فاکتور بازی ندارد پس فاکتور جدیدی برایش می سازیم
                    order = new Order()
                    {
                        IsFinaly = false,
                        CreateDate = DateTime.Now,
                        UserId = userId // آیدی ماربر را از کلایمز در همین اکشن گرفته بودیم پس آن را به یوزر آیدی فاکتور جدید می دهیم
                    };
                    _context.Orders.Add(order); // فاکتور جدید را به حدول فاکتور بانک اضافه کن
                    _context.SaveChanges();
                    _context.OrderDetails.Add(new OrderDetail() // چون فاکتوری وجود نداشت پس ریز فاکتورهم ندارد بنابراین خودمان لذایش ریزفاکتور ایجاد میکنیم
                    {
                        OrderId = order.OrderId,// ایدی فاکتور را از فاکتور نمونه سازی شده جدید میگیرد
                        ProductId = product.Id,// آیدی محصول را از پروداکت خط 46 میگیرد
                        Price = product.Item.Price,
                        Count = 1
                    }
                    );
                }

                _context.SaveChanges();
            }
            return RedirectToAction("ShowCart");
        }
        [Authorize]
        public IActionResult ShowCart()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            // مدل صحفحه از نوع اوردر است اگر کاربر فاکتور بازی داشت برو اوردر دیتلز را هم بیار سپس محصول را هم بیاور
            var order = _context.Orders.Where(o => o.UserId == userId && !o.IsFinaly)
               .Include(o => o.OrderDetails)
                .ThenInclude(c => c.Product).FirstOrDefault();
            //Order order = _context.Orders.SingleOrDefault(o => o.UserId == userId && !o.IsFinaly);
            //List<OrderViewModels> _list = new List<OrderViewModels>();
            //if (order != null)
            //{
            //    var details = _context.OrderDetails.Where(d => d.OrderId == order.OrderId).ToList();
            //    foreach (var item in details)
            //    {
            //        var product = _context.Products.Find(item.ProductId);

            //        _list.Add(new OrderViewModels()
            //        {
            //            Count = item.Count,
            //            ImageName = item.ImageName,
            //            OrderDetailId = item.DetailId,
            //            Price = Convert.ToInt32(item.Price),
            //            Sum =Convert.ToInt32 (item.Count * item.Price),
            //            Title = product.Name
            //        });

            //    }
            //}

            return View(order);     // مدل را به صفحه ببر
        }

        [Authorize]
        public IActionResult RemoveCart(int detailId)
        {
            var orderDetail = _context.OrderDetails.Find(detailId);
            _context.Remove(orderDetail);
            _context.SaveChanges();
            return RedirectToAction("ShowCart");
        }
        [Authorize]
        public IActionResult Command(int id, string command)
        {

            bool orderDetail = _context.OrderDetails.Any(o=> o.DetailId == id);
            var orderD = _context.OrderDetails.Find(id);
            switch (command)
            {
                case "Up":
                    {
                        orderD.Count += 1;
                        _context.Update(orderD);
                        //orderDetail.Count += 1;
                        //_context.Update(orderDetail);
                        break;
                    }
                case "Down":
                    {
                        orderD.Count -= 1;
                        if (orderD.Count == 0)
                        {
                            _context.OrderDetails.Remove(orderD);
                        }
                        else
                        {
                            _context.Update(orderD);
                        }
                        //orderDetail.Count -= 1;
                        //if (orderDetail.Count == 0)
                        //{
                        //    _context.OrderDetails.Remove(orderDetail);

                        //}
                        //else
                        //{
                        //    _context.Update(orderDetail);
                        //}
                        break;
                    }
                 
            }
            _context.SaveChanges();
            return RedirectToAction("ShowCart");

        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Detail(int id)
        {
            var product = _context.Products
                  .Include(navigationPropertyPath: p => p.Item)
                .SingleOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var categories = _context.Products.Where(p => p.Id == id).SelectMany(c => c.CategoryToProducts).
                Select(ca => ca.Category).ToList();

            var vm = new DetailsViewModel()
            {
                Product = product,
                Categories = categories
            };
            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //[Authorize]
        //public IActionResult Payment()
        //{
        //    // آیدی کاربر رااز طریق کلایمز که در کنترلر اکانت بدست آوردیم را پس میگیریم
        //    int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        //    //فاکتوری که مقدار شماره کاربرش با یوزرآیدی کلایمز یکی بود را به همراه ریز فاکتور هایش برگردان
        //    var order = _context.Orders
        //        .Include(o => o.OrderDetails)
        //        .FirstOrDefault(o => o.UserId == userId && !o.IsFinaly);

        //    if (order == null)
        //    {
        //        return NotFound();
        //    }
        //    // ارسال مقدار پرداخت  یا همان جمع فاکتوربه زرین پال
        //    var payment = new Payment((int)order.OrderDetails.Sum(d => d.Price));
        //    // به زرین پال جواب میدیهیم که بعد از انجام پرداخت، کاربر را به آدرس زیر به همراه شماره فاکتور و ایمیل و شمارع تلفن کاربر را کید دهیم   
        //    var res = payment.PaymentRequest($"پرداخت فاکتور شماره {order.OrderId}",
        //                                     "http://localhost:1635/Home/OnlinePayment/" + order.OrderId,
        //                                     "razi@info.com", "09140275379");
        //    // اگر بانک جواب وضعیت را 100 داد
        //    if (res.Result.Status == 100)
        //    {
        //        // کاربر را با کد تراکنش که به آن اختصاص داده، میفرستد
        //        return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + res.Result.Authority);
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }


        //}

        [Authorize]
        public IActionResult Payment()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.UserId == userId && !o.IsFinaly);
            if (order == null)
                return NotFound();

            var payment = new Payment((int)order.OrderDetails.Sum(d => d.Price));
            var res = payment.PaymentRequest($"پرداخت فاکتور شماره {order.OrderId}",
                "http://localhost:13724/Home/OnlinePayment/" + order.OrderId, "Iman@Madaeny.com", "09197070750");
            if (res.Result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + res.Result.Authority);
            }
            else
            {
                return BadRequest();
            }

        }



        // شماره فاکتور را به عنوان ورودی دریافت می کند
        public IActionResult OnlinePayment(int id)
        {
            // باید حتما شماره استاتوس و آتوریتی در آدرس یو آر ال وجود داشته باشد
            if (HttpContext.Request.Query["Status"] != "" &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
                HttpContext.Request.Query["Authority"] != "")
            {
                // اردر را براساس آیدی که خودمان فرستادیم، بدست می آوریم
                string authority = HttpContext.Request.Query["Authority"].ToString();
                var order = _context.Orders.Include(o => o.OrderDetails)
                    .FirstOrDefault(o => o.OrderId == id);
                // پیمنت را می سازیم که به جمع فاکتور را می دهیم
                var payment = new Payment((int)order.OrderDetails.Sum(d => d.Price));
                //  چک کردن آتوریتی کد با فیمت داده شده به پیمنت توسط زرین پال
                var res = payment.Verification(authority).Result;
                if (res.Status == 100)
                {
                    order.IsFinaly = true;
                    _context.Orders.Update(order);
                    _context.SaveChanges();
                    ViewBag.code = res.RefId; // کد پیگیری کاربر
                    return View();
                }
            }

            return NotFound();
        }
    }
}
