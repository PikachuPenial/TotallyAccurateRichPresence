using System;
using System.Collections.Generic;
using System.Text;

namespace TARP.Utilities
{
    internal class TARPDebug
    {
        public static void Log(object message)
        {
            if (TARP.allowDebug)
            {
                UnityEngine.Debug.Log("TARP: " + message);
            }

        }
    }
}
