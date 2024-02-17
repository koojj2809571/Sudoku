using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.Networking;

namespace Util
{
    public static class AssetsUtil
    {
        private static readonly string LevelDataFilePath = $"{Application.streamingAssetsPath}/RandomGame.json";

        public static List<List<int>> LevelData => ReadFile<List<int>>(LevelDataFilePath);

        private static List<T> ReadFile<T>(string path) where T : class
        {
            var request = UnityWebRequest.Get(path);
            var operation = request.SendWebRequest();
            while (!operation.isDone)
            {
                DebugUtil.Log("Read Level ...");
            }

            var jsonStr = request.downloadHandler.text;
            var nums = JsonMapper.ToObject<List<T>>(jsonStr);
            return nums;
        }

    }
}