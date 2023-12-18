using System;
using System.Collections.Generic;

namespace Util
{
    public static class UI
    {
        public static string SeparatorConvert(long in_number)
        {
            return string.Format("{0:#,###}", in_number);
        }
    }
}
