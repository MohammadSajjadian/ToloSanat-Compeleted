using cloudscribe.Pagination.Models;
using Data.Entities;
using Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brella.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    public class AdminController : Controller
    {
        #region Depency Injections

        private readonly IRepository<Project> projectRepo;
        private readonly IRepository<Post> postRepo;
        private readonly IRepository<Language> languageRepo;
        private readonly IRepository<Inbox> inboxRepo;
        private readonly IRepository<ElementProp> elementPropRepo;
        private readonly IRepository<SlideBar> slideBarRepo;
        private readonly IRepository<Element1> element1Repo;
        private readonly IRepository<Element2> element2Repo;
        private readonly IRepository<Element3> element3Repo;
        private readonly IRepository<Element4> element4Repo;

        public AdminController(IRepository<Project> _projectRepo, IRepository<Post> _postRepo,
            IRepository<Language> _languageRepo, IRepository<Inbox> _inboxRepo, IRepository<ElementProp> _elementPropRepo,
            IRepository<SlideBar> _slideBarRepo, IRepository<Element1> _element1Repo, IRepository<Element2> _element2Repo,
            IRepository<Element3> _element3Repo, IRepository<Element4> _element4Repo)
        {
            projectRepo = _projectRepo;
            postRepo = _postRepo;
            languageRepo = _languageRepo;
            inboxRepo = _inboxRepo;
            elementPropRepo = _elementPropRepo;
            slideBarRepo = _slideBarRepo;
            element1Repo = _element1Repo;
            element2Repo = _element2Repo;
            element3Repo = _element3Repo;
            element4Repo = _element4Repo;
        }

        #endregion


        #region Project

        public IActionResult InsertProject()
        {
            ViewData["Languages"] = languageRepo.Get(null, null, null);

            return View();
        }


        [Route("/admin/projectoptions/{pagenumber}")]
        public IActionResult ProjectOptions(int pagenumber)
        {
            #region Pagination

            var projects = projectRepo.Get(null, x => x.OrderByDescending(x => x.id), "Language").AsEnumerable();

            var pagination = PagingList.Create(projects, 20, pagenumber);

            var result = new PagedResult<Project>
            {
                Data = pagination.ToList(),
                PageNumber = pagenumber,
                PageSize = 20,
                TotalItems = projects.Count()
            };

            #endregion

            return View(result);
        }


        public IActionResult UpdateProject(int id)
        {
            return View(projectRepo.Find(id));
        }

        #endregion


        #region Post

        public IActionResult InsertPost()
        {
            ViewData["Languages"] = languageRepo.Get(null, null, null);
            return View();
        }


        [Route("/admin/postoptions/{pagenumber}")]
        public IActionResult PostOptions(int pagenumber)
        {
            #region Pagination

            var posts = postRepo.Get(null, x => x.OrderByDescending(x => x.id), "language").AsEnumerable();

            var pagination = PagingList.Create(posts, 20, pagenumber);

            var result = new PagedResult<Post>
            {
                Data = pagination.ToList(),
                PageNumber = pagenumber,
                PageSize = 20,
                TotalItems = posts.Count()
            };

            #endregion

            return View(result);
        }


        public IActionResult UpdatePost(int id)
        {
            return View(postRepo.Find(id));
        }

        #endregion


        #region Inbox

        [Route("/admin/inboxoptions/{pagenumber}")]
        public IActionResult InboxOptions(int pagenumber)
        {
            #region pagination

            List<Inbox> inboxes = inboxRepo.Get(null, x => x.OrderByDescending(x => x.id), null);

            IEnumerable<Inbox> pagination = PagingList.Create(inboxes, 10, pagenumber).AsEnumerable();

            PagedResult<Inbox> result = new()
            {
                Data = pagination.ToList(),
                PageSize = 10,
                PageNumber = pagenumber,
                TotalItems = inboxes.Count
            };

            #endregion

            return View(result);
        }

        #endregion


        #region Header

        public IActionResult UpdateHeaderProp()
        {
            return View(elementPropRepo.Get(null, null, "language"));
        }

        #endregion


        #region Slide Bar

        public IActionResult SlideBarOptions()
        {
            ViewData["Languages"] = languageRepo.Get(null, null, null);
            ViewData["SlideBar"] = slideBarRepo.Get(null, x => x.OrderByDescending(x => x.id), "language");
            ViewBag.SlideBarCount = slideBarRepo.Get(null, null, null).Count;

            return View();
        }


        public IActionResult UpdateSlideBar(int id)
        {
            return View(slideBarRepo.Find(id));
        }

        #endregion


        #region About Us

        public IActionResult AboutUsProp()
        {
            return View(elementPropRepo.Get(null, null, "language"));
        }

        #endregion


        #region Contact Us

        public IActionResult ContactUsProp()
        {
            return View(elementPropRepo.Get(null, null, "language"));
        }

        #endregion


        #region Element1

        public IActionResult UpdateElement1Prop()
        {
            return View(elementPropRepo.Get(null, null, "language"));
        }


        public IActionResult Element1Options()
        {
            ViewData["Languages"] = languageRepo.Get(null, null, null);
            ViewData["Element1"] = element1Repo.Get(null, x => x.OrderByDescending(x => x.id), "language");
            ViewBag.Element1Count = element1Repo.Get(null, null, null).Count;

            return View();
        }


        public IActionResult UpdateElement1(int id)
        {
            return View(element1Repo.Find(id));
        }

        #endregion


        #region Element2

        public IActionResult UpdateElement2Prop()
        {
            return View(elementPropRepo.Get(null, null, "language"));
        }


        public IActionResult Element2Options()
        {
            ViewData["Languages"] = languageRepo.Get(null, null, null);
            ViewData["Element2"] = element2Repo.Get(null, x => x.OrderByDescending(x => x.id), "language");
            ViewBag.Element2Count = element2Repo.Get(null, null, null).Count;

            return View();
        }


        public IActionResult UpdateElement2(int id)
        {
            return View(element2Repo.Find(id));
        }

        #endregion


        #region Element3

        public IActionResult UpdateElement3Prop()
        {
            return View(elementPropRepo.Get(null, null, "language"));
        }


        public IActionResult Element3Options()
        {
            ViewData["Languages"] = languageRepo.Get(null, null, null);
            ViewData["Element3"] = element3Repo.Get(null, x => x.OrderByDescending(x => x.id), "language");
            ViewBag.Element3Count = element3Repo.Get(null, null, null).Count;

            return View();
        }


        public IActionResult UpdateElement3(int id)
        {
            return View(element3Repo.Find(id));
        }

        #endregion


        #region Element4

        public IActionResult UpdateElement4Prop()
        {
            return View(elementPropRepo.Get(null, null, "language"));
        }


        public IActionResult Element4Options()
        {
            ViewData["Languages"] = languageRepo.Get(null, null, null);
            ViewData["Element4"] = element4Repo.Get(null, x => x.OrderByDescending(x => x.id), "language");
            ViewBag.Element4Count = element4Repo.Get(null, null, null).Count;

            return View();
        }


        public IActionResult UpdateElement4(int id)
        {
            return View(element4Repo.Find(id));
        }

        #endregion


        #region Element5

        public IActionResult UpdateElement5Prop()
        {
            return View(elementPropRepo.Get(null, null, "language"));
        }

        #endregion
    }
}
