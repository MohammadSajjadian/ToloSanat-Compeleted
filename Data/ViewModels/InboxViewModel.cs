using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ViewModels
{
    public class InboxViewModel
    {
        #region String Attributes

        [Required(ErrorMessage ="نام و نام خانوادگی را وارد کنید")]
        public string nameFamily { get; set; }
        [Required(ErrorMessage ="شماره تلفن را وارد کنید")]
        public string phonenumber { get; set; }
        [Required(ErrorMessage ="پیغام خود را وارد کنید")]
        public string message { get; set; }

        #endregion
    }
}
