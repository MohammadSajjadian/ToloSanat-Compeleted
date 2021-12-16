using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ViewModels
{
    public class PostViewModel
    {
        public List<int> languageIds { get; set; }

        [Required(ErrorMessage ="عنوان مورد نظر را وارد کنید.")]
        public List<string> titles { get; set; }

        [Required(ErrorMessage ="توضیح کوتاه مورد نظر را وارد کنید.")]
        public List<string> shortDescriptions { get; set; }

        [Required(ErrorMessage ="توضیح کامل مورد نظر را وارد کنید.")]
        public List<string> longDescriptions { get; set; }

        public bool IsSms { get; set; }
        public bool IsEmail { get; set; }

        [Required(ErrorMessage ="عکس را وارد کنید.")]
        public IFormFile img { get; set; }
    }
}
