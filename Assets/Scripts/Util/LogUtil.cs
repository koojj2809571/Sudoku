using UnityEngine;

namespace Util
{
    public static class LogUtil
    {
        public static void Log(string content)
        {
            if (Debug.isDebugBuild)
            {
                Debug.Log(content);
            }
        }
    }
}
