using System;
using System.Globalization;
using System.Threading;

namespace Cake.XdtTransform.Tests.Util {
    public static class CultureUtil {
        public static void UseGBCulture(Action test) {
            var currentThread = Thread.CurrentThread;
            var existingCulture = currentThread.CurrentCulture;
            var existingUiCulture = currentThread.CurrentUICulture;

            var GbCulture = CultureInfo.CreateSpecificCulture("en-GB");
            currentThread.CurrentCulture = GbCulture;
            currentThread.CurrentUICulture = GbCulture;

            try {
                test();
            }
            finally {
                currentThread.CurrentCulture = existingCulture;
                currentThread.CurrentUICulture = existingUiCulture;
            }
        }
    }
}
