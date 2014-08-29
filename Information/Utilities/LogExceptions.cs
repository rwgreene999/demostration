using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Diagnostics;
using System.Runtime.CompilerServices;    // for the [MethodImpl(MethodImplOptions.NoInlining)]

namespace Information.Utilities
{
    public class LogException
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public LogException(Exception ex, bool BreakOnException = false )
        {
            Debug.WriteLine("------------------------------------------ Start at " + DateTime.Now.ToString());
            while (ex != null)
            {
                
                var exceptionName = ex.GetType().Name;
                // Note: StackFrame idea from: http://stackoverflow.com/questions/1310145/get-calling-function-name-from-called-function
                var caller = new StackFrame(1, true).GetMethod().Name; 
                Debug.WriteLine("Exception at " + DateTime.Now + ", caller:" + caller + ", exception type:" + exceptionName + ", source:" + ex.Source); 
                Debug.WriteLine(ex.ToString());
                Debug.WriteLine("------------------------------------------ at " + DateTime.Now.ToString());
                ex = ex.InnerException;
            }
            if (BreakOnException)
            {
                Debugger.Break();
            }
        }
    }

}