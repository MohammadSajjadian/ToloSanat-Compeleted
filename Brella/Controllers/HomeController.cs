using Data.Entities;
using Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Brella.Controllers
{
    public class HomeController : Controller
    {
        #region Dependency Injections

        private readonly IRepository<Project> projectRepo;
        private readonly IRepository<Post> postRepo;
        private readonly IRepository<ElementProp> elementpropRepo;
        private readonly IRepository<SlideBar> slideBarRepo;
        private readonly IRepository<Element1> element1Repo;
        private readonly IRepository<Element2> element2Repo;
        private readonly IRepository<Element3> element3Repo;
        private readonly IRepository<Element4> element4Repo;
        private readonly string lang = CultureInfo.CurrentCulture.Name;

        public HomeController(IRepository<Project> _projectRepo,
            IRepository<Post> _postRepo, IRepository<Inbox> _inboxRepo, IRepository<SlideBar> _slideBarRepo,
            IRepository<Element1> _element1Repo, IRepository<Element2> _element2Repo, IRepository<Element3> _element3Repo,
            IRepository<Element4> _element4Repo, IRepository<ElementProp> _elementpropRepo)
        {
            projectRepo = _projectRepo;
            postRepo = _postRepo;
            elementpropRepo = _elementpropRepo;
            slideBarRepo = _slideBarRepo;
            element1Repo = _element1Repo;
            element2Repo = _element2Repo;
            element3Repo = _element3Repo;
            element4Repo = _element4Repo;
        }

        #endregion


        public IActionResult Index()
        {
            ElementProp elementProp = elementpropRepo.Get(x => x.language.title == lang, null, null).FirstOrDefault();

            #region Slide Bar

            ViewData["SlideBars"] = slideBarRepo.Get(x => x.language.title == lang, x => x.OrderByDescending(x => x.id), null);
            ViewBag.SlideBarsCount= slideBarRepo.Get(x => x.language.title == lang, null, null).Count;

            #endregion


            #region Project

            ViewData["Projects"] = projectRepo.Get(x => x.language.title == lang, x => x.OrderByDescending(x => x.id), null);
            ViewBag.ProjectsCount = projectRepo.Get(x => x.language.title == lang, null, null).Count;

            #endregion


            #region Post

            ViewData["Posts"] = postRepo.Get(x => x.language.title == lang, x => x.OrderByDescending(x => x.id), "ApplicationUser");
            ViewBag.PostsCount = postRepo.Get(x => x.language.title == lang, null, null).Count;

            #endregion


            #region Element1

            ViewData["Element1"] = element1Repo.Get(x => x.language.title == lang, x => x.OrderByDescending(x => x.id), null);
            ViewBag.Element1Count = element1Repo.Get(x => x.language.title == lang, null, null).Count;

            #endregion


            #region Element2

            ViewData["Element2"] = element2Repo.Get(x => x.language.title == lang, x => x.OrderByDescending(x => x.id), null);
            ViewBag.Element2Count = element2Repo.Get(x => x.language.title == lang, null, null).Count;

            #endregion


            #region Element3

            ViewData["Element3"] = element3Repo.Get(x => x.language.title == lang, x => x.OrderByDescending(x => x.id), null);
            ViewBag.Element3Count = element3Repo.Get(x => x.language.title == lang, null, null).Count;

            #endregion


            #region Element4

            ViewData["Element4"] = element4Repo.Get(x => x.language.title == lang, x => x.OrderByDescending(x => x.id), null);
            ViewBag.Element4Count = element4Repo.Get(x => x.language.title == lang, null, null).Count;

            #endregion

            return View(elementProp);
        }


        public IActionResult Error()
        {
            return View();
        }


        public IActionResult ContactUs()
        {
            ElementProp elementProp = elementpropRepo.Get(x => x.language.title == lang, null, null).FirstOrDefault();

            #region ViewBags

            ViewBag.PhoneNumber = elementProp.phoneNumber;
            ViewBag.Email = elementProp.email;
            ViewBag.Address = elementProp.address;
            ViewBag.contactUsTitle = elementProp.contactUsTitle;
            ViewBag.contactUsDescription = elementProp.contactUsDescription;
            
            #endregion

            return View();
        }


        public IActionResult AboutUs()
        {
            ElementProp elementProp = elementpropRepo.Get(x => x.language.title == lang, null, null).FirstOrDefault();

            return View(elementProp);
        }


        #region ChangeLanguage

        public IActionResult ChangeLanguage(string lang)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(lang)),
                new CookieOptions() { Expires = DateTime.Now.AddYears(1) });

            return Redirect(Request.Headers["Referer"].ToString());
        }

        #endregion
    }
}
