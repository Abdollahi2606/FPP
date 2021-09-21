using Microsoft.AspNetCore.Mvc;
using MyEshop.Data;
using MyEshop.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyEshop.Components
{
    public class OrderComponent : ViewComponent
    {
        private MyEshopContext _context;


        public OrderComponent(MyEshopContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<OrderViewModels> _list = new List<OrderViewModels>();

            if (User.Identity.IsAuthenticated)
            {
                string CurrentUserID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                int c1 = Convert.ToInt32(CurrentUserID);

                var order = _context.Orders.SingleOrDefault(o => o.UserId == c1 && !o.IsFinaly);
                if (order != null)
                {
                    var details = _context.OrderDetails.Where(d => d.OrderId == order.OrderId).ToList();
                    foreach (var item in details)
                    {
                        var product = _context.Products.Find(item.ProductId);
                        _list.Add(new OrderViewModels()
                        {
                            Count = item.Count,
                            Title = product.Name,
                            ImageName = product.Id
                        });

                    }
                }

            }

            return View("/Views/Components/OrderComponent.cshtml", _list);
        }
    }
}
