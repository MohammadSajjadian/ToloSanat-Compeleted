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
        [Required(ErrorMessage = "ایمیل خود را وارد کنید.")]
        public string userName { get; set; }


        [Required(ErrorMessage = "رمزعبور خود را وارد کنید.")]
        public string password { get; set; }

        public bool rememberMe { get; set; }
    }
}
