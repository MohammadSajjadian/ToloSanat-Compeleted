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
    public class BlogController : Controller
    {
        #region Dependency Injection

        private readonly IRepository<Post> postRepo;
        private readonly string lang = CultureInfo.CurrentCulture.Name;

        public BlogController(IRepository<Post> _postRepo)
        {
            postRepo = _postRepo;
        }

        #endregion

        public IActionResult Index()
        {
            #region Pagination

            List<Post> posts = postRepo.Get(x => x.language.title == lang, x => x.OrderByDescending(x => x.createTime), "ApplicationUser");

            var paging = PagingList.Create(posts, 9, 1);

            var result = new PagedResult<Post>
            {
                Data = paging,
                PageSize = 9,
                PageNumber = 1,
                TotalItems = posts.Count
            };

            #endregion

            return View(result);
        }


        [Route("blog/{pagenumber}")]
        public IActionResult Blog(int pagenumber)
        {
            #region Pagination

            List<Post> posts = postRepo.Get(x => x.language.title == lang, x => x.OrderByDescending(x => x.createTime), "ApplicationUser");

            var paging = PagingList.Create(posts, 9, pagenumber);

            var result = new PagedResult<Post>
            {
                Data = paging,
                PageSize = 9,
                PageNumber = pagenumber,
                TotalItems = posts.Count
            };

            #endregion

            return View(result);
        }


        [HttpGet("blogdetail/{title}/{id}")]
        public IActionResult BlogDetail(string title, int id)
        {
            Post post = postRepo.Find(id);
            
            var posts = postRepo.Get(x => x.language.title == lang, x => x.OrderByDescending(x => x.createTime), "ApplicationUser");
            posts.Remove(post);
            ViewBag.PostsCount = posts.Count;

            ViewData["Posts"] = posts;
            
            return View(post);
        }
    }
}
