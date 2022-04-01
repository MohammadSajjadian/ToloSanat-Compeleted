using cloudscribe.Pagination.Models;
using Data.Entities;
using Data.Repository;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Brella.Controllers
{
    public class ProjectController : Controller
    {
        #region Dependency Injection

        private readonly IRepository<Project> projectRepo;
        private readonly string lang = CultureInfo.CurrentCulture.Name;

        public ProjectController(IRepository<Project> _projectRepo)
        {
            projectRepo = _projectRepo;
        }

        #endregion

        public IActionResult Index()
        {
            #region Pagination

            List<Project> projects = projectRepo.Get(x => x.language.title == lang, x => x.OrderByDescending(x => x.id), "ApplicationUser");

            var paging = PagingList.Create(projects, 9, 1);

            var result = new PagedResult<Project>
            {
                Data = paging,
                PageSize = 9,
                PageNumber = 1,
                TotalItems = projects.Count
            };

            #endregion

            return View(result);
        }


        [Route("project/{pagenumber}")]
        public IActionResult Project(int pagenumber)
        {
            #region Pagination

            List<Project> projects = projectRepo.Get(x => x.language.title == lang, x => x.OrderByDescending(x => x.id), "ApplicationUser");

            var paging = PagingList.Create(projects, 9, pagenumber);

            var result = new PagedResult<Project>
            {
                Data = paging,
                PageSize = 9,
                PageNumber = pagenumber,
                TotalItems = projects.Count
            };

            #endregion

            return View(result);
        }


        [HttpGet("projectdetail/{title}/{id}")]
        public IActionResult ProjectDetail(string title, int id)
        {
            Project project = projectRepo.Find(id);

            var projects = projectRepo.Get(x => x.language.title == lang, x => x.OrderByDescending(x => x.id), "ApplicationUser");
            projects.Remove(project);
            ViewBag.ProjectsCount = projects.Count;

            ViewData["Projects"] = projects;

            return View(project);
        }
    }
}
