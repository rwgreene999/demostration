using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Information.Utilities
{
    public class LogException
    {
        public LogException(Exception ex, bool BreakOnException = false )
        {
            System.Diagnostics.Debug.WriteLine("------------------------------------------ Start at " + DateTime.Now.ToString());
            while (ex != null)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                System.Diagnostics.Debug.WriteLine("------------------------------------------ at " + DateTime.Now.ToString());
                ex = ex.InnerException;
            }
            if (BreakOnException)
            {
                System.Diagnostics.Debugger.Break();
            }
        }
    }

}