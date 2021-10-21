using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ViewModels
{
    public class RegisterViewModel
    {
        #region Attributes

        [Required(ErrorMessage = "ایمیل خود را وارد کنید.")]
        [EmailAddress(ErrorMessage = "فرمت وارد شده نادرست است.")]
        [Remote("IsUserExist", "Account", ErrorMessage = "ایمیل در حال حاظر موجود میباشد.")]

        #endregion
        public string userName { get; set; }


        #region Attributes

        [Required(ErrorMessage = "رمز عبور را وارد کنید")]
        [MinLength(6, ErrorMessage = "رمز عبور باید حداقل 6 کاراکتر باشد")]
        [MaxLength(20, ErrorMessage = "رمز عبور باید حداکثر 20 کاراکتر باشد")]

        #endregion
        public string password { get; set; }


        #region Attributes

        [Compare(nameof(password))]

        #endregion
        public string rePassword { get; set; }

        public string name { get; set; }
        public string family { get; set; }
    }
}
