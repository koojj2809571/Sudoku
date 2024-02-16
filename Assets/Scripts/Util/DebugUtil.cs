using System;
using UnityEngine;

namespace Util
{
    public static class DebugUtil
    {
        public static void Log(string content)
        {
            RunOnDebug(() =>
            {
                Debug.Log(content);
            });
        }

        public static void RunOnDebug(Action action)
        {
            if (Debug.isDebugBuild)
            {
                action();
            }
        }
    }
}
