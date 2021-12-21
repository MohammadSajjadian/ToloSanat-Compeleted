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

        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "EmailAddress")]
        [Remote("IsUserExist", "Account", ErrorMessage = "Remote")]

        #endregion
        public string userName { get; set; }

        #region Attributes
        
        [Required(ErrorMessage = "Required")]
        [Phone(ErrorMessage = "Phone")]

        #endregion
        public string phoneNumber { get; set; }


        #region Attributes

        [Required(ErrorMessage = "Required")]
        [MinLength(6, ErrorMessage = "MinLength")]
        [MaxLength(20, ErrorMessage = "MaxLength")]

        #endregion
        public string password { get; set; }


        #region Attributes

        [Compare(nameof(password), ErrorMessage = "Compare")]

        #endregion
        public string rePassword { get; set; }

        #region Attributes

        [Required(ErrorMessage = "Required")]

        #endregion
        public string name { get; set; }

        #region Attributes

        [Required(ErrorMessage = "Required")]

        #endregion
        public string family { get; set; }
    }
}
