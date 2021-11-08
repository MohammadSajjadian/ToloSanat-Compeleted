using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ResizeService
{
    public class ResizerImage : IResize
    {
        public byte[] Resizer(byte[] b, int width, int height, ImageFormat format)
        {
            var memoryStream = new MemoryStream(b);

            var image = Image.FromStream(memoryStream);

            var bitmap = new Bitmap(image, width, height);

            var secondMemoryStream = new MemoryStream();
            bitmap.Save(secondMemoryStream, format);

            return secondMemoryStream.ToArray();
        }
    }
}
