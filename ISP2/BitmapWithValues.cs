using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP2
{
    internal class BitmapWithValues
    {
        public Bitmap Bitmap { get; }
        public int Expected { get; }
        public float ZeroPercentage { get; }
        public float OnePercentage { get; }
        public float TwoPercentage { get; }
        public float ThreePercentage { get; }
        public float FourPercentage { get; }
        public float FivePercentage { get; }
        public float SixPercentage { get; }
        public float SevenPercentage { get; }
        public float EightPercentage { get; }
        public float NinePercentage { get; }

        public BitmapWithValues(Bitmap bitmap, int expected, float zeroPercentage, float onePercentage, float twoPercentage, float threePercentage, float fourPercentage, float fivePercentage, float sixPercentage, float sevenPercentage, float eightPercentage, float ninePercentage)
        {
            Bitmap = bitmap;
            Expected = expected;
            ZeroPercentage = zeroPercentage;
            OnePercentage = onePercentage;
            TwoPercentage = twoPercentage;
            ThreePercentage = threePercentage;
            FourPercentage = fourPercentage;
            FivePercentage = fivePercentage;
            SixPercentage = sixPercentage;
            SevenPercentage = sevenPercentage;
            EightPercentage = eightPercentage;
            NinePercentage = ninePercentage;
        }
    }
}
