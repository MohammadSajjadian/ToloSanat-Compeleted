using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class SlideBar : CommonAttributes
    {
        public byte[] img { get; set; } // 1920 * 720

        #region String Attributes

        public string buttonTitle { get; set; }
        public string buttonLink { get; set; }

        #endregion
    }
}
