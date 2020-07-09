using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP2
{
    class ToInt
    {
        public static int ToInteger(string str, int defaultValue)
        {
            if (!int.TryParse(str, out var val))
            {
                val = defaultValue;
            }

            return val;
        }
    }
}
