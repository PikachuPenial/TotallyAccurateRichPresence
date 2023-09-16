using System;
using System.Collections.Generic;
using System.Text;

namespace TARP.Utilities
{
    internal class TARPDebug
    {
        public static void Log(object message)
        {
            if (TARP.allowLogging)
            {
                UnityEngine.Debug.Log("TARP: " + message);
            }

        }
    }
}
