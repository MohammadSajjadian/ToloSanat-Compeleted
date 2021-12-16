using Data.Entities;
using Data.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brella.ViewComponents
{
    public class OrderViewComponent : ViewComponent
    {
        private readonly IRepository<Order> order;

        public OrderViewComponent(IRepository<Order> _order)
        {
            order = _order;
        }

        public IViewComponentResult Invoke()
        {
            return View(order.Get(x => x.IsAdminConfirmed == false, null, null).Count);
        }
    }
}
