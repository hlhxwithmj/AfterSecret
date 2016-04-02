using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AfterSecret.Lib
{
    public class Common
    {
        public static int ConvertToTimestamp(DateTime value)
        {
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            return (int)span.TotalSeconds;
        }
    }
}