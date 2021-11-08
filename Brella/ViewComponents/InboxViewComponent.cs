using Data.Entities;
using Data.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brella.ViewComponents
{
    public class InboxViewComponent : ViewComponent
    {
        private readonly IRepository<Inbox> inboxRepo;

        public InboxViewComponent(IRepository<Inbox> _inboxRepo)
        {
            inboxRepo = _inboxRepo;
        }

        public IViewComponentResult Invoke()
        {
            return View(inboxRepo.Get(x => x.IsConfirm == false, null, null).Count);
        }
    }
}
