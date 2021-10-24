using Data.Entities;
using Data.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brella.ViewComponents
{
    public class LanguageViewComponent : ViewComponent
    {
        private IRepository<Language> languageRepo;
        public LanguageViewComponent(IRepository<Language> _languageRepo)
        {
            languageRepo = _languageRepo;
        }

        public IViewComponentResult Invoke()
        {
            return View(languageRepo.Get(null, null, null));
        }
    }
}
