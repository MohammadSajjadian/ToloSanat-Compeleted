using cloudscribe.Pagination.Models;
using Data.Entities;
using Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRepository<Project> projectRepo;
        private readonly IRepository<Post> postRepo;
        private readonly IRepository<Group> groupRepo;
        private readonly IRepository<Message> messageRepo;
        private readonly IRepository<Province> provinceRepo;
        private readonly IRepository<Language> languageRepo;
        private readonly IRepository<Inbox> inboxRepo;
        private readonly IRepository<ElementProp> elementPropRepo;
        private readonly IRepository<SlideBar> slideBarRepo;
        private readonly IRepository<Element1> element1Repo;
        private readonly IRepository<Element2> element2Repo;
        private readonly IRepository<Element3> element3Repo;
        private readonly IRepository<Element4> element4Repo;

        public AdminController(UserManager<ApplicationUser> _userManager, IRepository<Project> _projectRepo, IRepository<Post> _postRepo, IRepository<Group> _groupRepo,
            IRepository<Message> _messageRepo, IRepository<Province> _provinceRepo, IRepository<Language> _languageRepo,
            IRepository<Inbox> _inboxRepo, IRepository<ElementProp> _elementPropRepo, IRepository<SlideBar> _slideBarRepo,
            IRepository<Element1> _element1Repo, IRepository<Element2> _element2Repo, IRepository<Element3> _element3Repo, IRepository<Element4> _element4Repo)
        {
            userManager = _userManager;
            projectRepo = _projectRepo;
            postRepo = _postRepo;
            groupRepo = _groupRepo;
            messageRepo = _messageRepo;
            provinceRepo = _provinceRepo;
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


        #region Chat

        [Route("/admin/ChatManagement/{pagenumber}")]
        public IActionResult ChatManagement(int pagenumber)
        {
            #region Pagination

            IEnumerable<Group> groups = groupRepo.Get(x => x.IsDeleteForAdmin == false,
                x => x.OrderBy(x => x.lastMessageTime), "messages").AsEnumerable();

            PagingList<Group> paging = PagingList.Create(groups, 2, pagenumber);

            PagedResult<Group> result = new()
            {
                Data = paging.ToList(),
                PageSize = 2,
                PageNumber = pagenumber,
                TotalItems = groups.Count()
            };

            #endregion

            return View(result);
        }


        public IActionResult AdminSideChatsBridge(string clientId)
        {
            HttpContext.Session.SetString("clientId", clientId);

            return RedirectToAction(nameof(AdminSideChats));
        }


        public async Task<IActionResult> AdminSideChats()
        {
            string clientId = HttpContext.Session.GetString("clientId");

            Group group = groupRepo.Get(x => x.userId == clientId, null, null).FirstOrDefault();

            List<Message> messages = messageRepo.Get(x => x.groupId == group.id, null, "applicationUser");

            ApplicationUser client = await userManager.FindByIdAsync(clientId);

            ViewBag.clientId = client.Id;
            ViewBag.clientNameFamily = $"{client.name} {client.family}";
            ViewBag.clientId = client.Id;
            ViewBag.groupId = group.id;

            return View(messages);
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

            var projects = projectRepo.Get(null, x => x.OrderByDescending(x => x.id), "language").AsEnumerable();

            var pagination = PagingList.Create(projects, 4, pagenumber);

            var result = new PagedResult<Project>
            {
                Data = pagination.ToList(),
                PageNumber = pagenumber,
                PageSize = 4,
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


        #region Contract

        public IActionResult ContractFileProp()
        {
            return View(elementPropRepo.Get(x => x.language.faTitle == "فارسی", null, "language").FirstOrDefault());
        }


        public IActionResult ContractProp()
        {
            return View(elementPropRepo.Get(null, null, "language"));
        }

        #endregion


        #region Province

        public IActionResult ProvinceOptions()
        {
            List<Province> provinces = provinceRepo.Get(null, x => x.OrderByDescending(x => x.id), null);
            ViewData["Provinces"] = provinces;
            ViewBag.ProvincesCount = provinces.Count;

            return View();
        }


        public IActionResult UpdateProvince(int id)
        {
            return View(provinceRepo.Find(id));
        }

        #endregion


        #region Transportation

        public IActionResult TransportationProp()
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
