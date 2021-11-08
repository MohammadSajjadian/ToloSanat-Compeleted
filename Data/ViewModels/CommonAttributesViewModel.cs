using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ViewModels
{
    public class CommonAttributesViewModel
    {
        public List<int> languageIds { get; set; }

        [Required(ErrorMessage ="عنوان مورد نظر را وارد کنید.")]
        public List<string> preTitles { get; set; }
        
        [Required(ErrorMessage ="عنوان مورد نظر را وارد کنید.")]
        public List<string> titles { get; set; }
        
        [Required(ErrorMessage ="توضیح مورد نظر را وارد کنید.")]
        public List<string> descriptions { get; set; }
        
        [Required(ErrorMessage ="متن دکمه مورد نظر را وارد کنید.")]
        public List<string> buttonTitles { get; set; }
        
        [Required(ErrorMessage ="لینک مورد نظر را وارد کنید.")]
        public string buttonLink { get; set; }
        
        [Required(ErrorMessage ="عکس مورد نظر را وارد کنید.")]
        public IFormFile img { get; set; }
    }
}
