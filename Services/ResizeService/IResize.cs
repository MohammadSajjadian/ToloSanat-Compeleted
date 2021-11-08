using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ResizeService
{
    public interface IResize
    {
        byte[] Resizer(byte[] b, int width, int height, ImageFormat format);
    }
}
