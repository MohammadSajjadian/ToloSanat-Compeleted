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
        [EmailAddress(ErrorMessage = "EmailAddress")]
        public string userName { get; set; }

        #region Attributes

        [Required(ErrorMessage = "Required")]
        [Phone(ErrorMessage = "Phone")]

        #endregion
        public string phoneNumber { get; set; }

        [Required(ErrorMessage = "Required")]
        public string name { get; set; }

        [Required(ErrorMessage = "Required")]
        public string family { get; set; }
    }
}
