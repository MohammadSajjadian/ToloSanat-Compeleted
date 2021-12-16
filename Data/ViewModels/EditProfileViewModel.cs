using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ViewModels
{
    public class EditProfileViewModel
    {
        #region Attributes

        [EmailAddress(ErrorMessage = "فرمت وارد شده نادرست است.")]

        #endregion
        public string userName { get; set; }

        #region Attributes

        [Phone(ErrorMessage = "فرمت وارد شده نادرست است.")]

        #endregion
        public string phoneNumber { get; set; }

        public string name { get; set; }

        public string family { get; set; }
    }
}
