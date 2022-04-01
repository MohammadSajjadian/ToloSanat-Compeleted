using Data.Entities;
using Data.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Brella.ViewComponents
{
    public class PhoneViewComponent : ViewComponent
    {
        private readonly IRepository<ElementProp> elementPropRepo;

        public PhoneViewComponent(IRepository<ElementProp> _elementPropRepo)
        {
            elementPropRepo = _elementPropRepo;
        }


        public IViewComponentResult Invoke()
        {
            string lang = CultureInfo.CurrentCulture.Name;

            ElementProp elementProp = elementPropRepo.Get(x => x.language.title == lang, null, null).FirstOrDefault();

            return View(elementProp);
        }
    }
}
