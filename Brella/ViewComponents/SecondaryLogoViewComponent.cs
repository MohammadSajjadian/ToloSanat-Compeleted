using Data.Entities;
using Data.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brella.ViewComponents
{
    public class SecondaryLogoViewComponent : ViewComponent
    {
        private readonly IRepository<ElementProp> elementPropRepo;

        public SecondaryLogoViewComponent(IRepository<ElementProp> _elementPropRepo)
        {
            elementPropRepo = _elementPropRepo;
        }


        public IViewComponentResult Invoke()
        {
            ElementProp elementProp = elementPropRepo.Get(x => x.language.faTitle == "فارسی", null, null).FirstOrDefault();

            return View(elementProp);
        }
    }
}
