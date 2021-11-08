using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brella.Controllers
{
    public class CRUD : Controller
    {
        #region KeyWords

        private const string success = "success";
        private const string error = "error";
        private const string warning = "warning";
        private const string info = "info";
        private const string Index = "Index";
        private const string Home = "Home";
        private const string Account = "Account";

        #endregion


        #region Dependency Injections

        public CRUD()
        {

        }

        #endregion


        #region CV

        public IActionResult AddProject()
        {

        }

        #endregion
    }
}
