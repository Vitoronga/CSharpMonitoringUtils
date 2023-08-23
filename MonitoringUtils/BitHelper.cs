using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringUtils
{
    internal class BitHelper
    {
        public static bool IsBitOn(byte[] bytes, int position)
        {
            int byteIndex = position / 8;
            int subPos = position % 8;
            return (bytes[byteIndex] & (1 << subPos)) != 0;
        }
    }
}
