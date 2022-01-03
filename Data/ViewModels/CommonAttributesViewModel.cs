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

        public List<string> preTitles { get; set; }
        
        public List<string> titles { get; set; }
        
        public List<string> descriptions { get; set; }
        
        public List<string> buttonTitles { get; set; }
        
        public string buttonLink { get; set; }
        
        [Required(ErrorMessage ="عکس مورد نظر را وارد کنید.")]
        public IFormFile img { get; set; }
    }
}
