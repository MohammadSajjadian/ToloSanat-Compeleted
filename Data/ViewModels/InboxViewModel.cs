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

        [Required(ErrorMessage = "Required")]
        public string nameFamily { get; set; }

        [Required(ErrorMessage = "Required")]
        public string phonenumber { get; set; }

        [Required(ErrorMessage = "Required")]
        public string message { get; set; }

        #endregion
    }
}
