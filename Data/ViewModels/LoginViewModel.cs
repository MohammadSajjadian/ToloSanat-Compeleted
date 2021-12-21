using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Required")]
        public string userName { get; set; }


        [Required(ErrorMessage = "Required")]
        public string password { get; set; }

        public bool rememberMe { get; set; }
    }
}
