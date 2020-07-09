using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP2
{
    class ProgressReport
    {
        public ProgressReport(int currentImage, int totalImages)
        {
            CurrentImage = currentImage;
            TotalImages = totalImages;
        }

        public int CurrentImage { get; }

        public int TotalImages { get; }

        public long ToLong()
        {
            return ((long) CurrentImage << 32) | (long) TotalImages;
        }
    }
}
