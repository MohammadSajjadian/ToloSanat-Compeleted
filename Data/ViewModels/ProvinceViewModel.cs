using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ViewModels
{
    public class ProvinceViewModel
    {
        [Required(ErrorMessage = "نام شهر را وارد کنید.")]
        public string name { get; set; }

        [Required(ErrorMessage = "قیمت را وارد کنید.")]
        public int price { get; set; }
    }
}
