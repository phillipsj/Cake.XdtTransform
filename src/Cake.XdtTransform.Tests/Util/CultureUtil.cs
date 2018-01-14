using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cake.XdtTransform.Tests.Util
{
    public static class CultureUtil
    {
        public static void UseGBCulture(Action test)
        {
            var currentThread = Thread.CurrentThread;
            var existingCulture = currentThread.CurrentCulture;
            var existingUiCulture = currentThread.CurrentUICulture;

            var GbCulture = CultureInfo.CreateSpecificCulture("en-GB");
            currentThread.CurrentCulture = GbCulture;
            currentThread.CurrentUICulture = GbCulture;

            try
            {
                test();
            }
            finally
            {
                currentThread.CurrentCulture = existingCulture;
                currentThread.CurrentUICulture = existingUiCulture;
            }
        }
    }
}
