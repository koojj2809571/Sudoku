using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEditor;
using UnityEngine;

namespace Util
{
    public static class AssetsUtil
    {
        private static readonly string FileDir = $"{Application.dataPath}/Jsons";
        public static readonly string LevelDataFilePath = $"{FileDir}/RandomGame.json";
        public static readonly string LevelResultFilePath = $"{FileDir}/LevelResult.json";

        private static FileInfo JsonFile(string path) => new(path);

        public static List<List<int>> LevelData
        {
            get => ReadFile<List<int>>(LevelDataFilePath);
            set => WriteToFile(value, LevelDataFilePath);
        }

        public static List<string> LevelResultData
        {
            get => ReadFile<string>(LevelResultFilePath);
            set => WriteToFile(value, LevelResultFilePath);
        }

        public static List<T> ReadFile<T>(string path) where T : class
        {
            if (!JsonFile(path).Exists) return new List<T>();
            var sr = new StreamReader(path);
            var js = new JsonReader(sr);
            var nums = JsonMapper.ToObject<List<T>>(js);
            sr.Close();
            sr.Dispose();
            return nums;
        }
        
        public static void WriteToFile<T>(List<T> data, string path) where T: class
        {
            var sw = JsonFile(path).CreateText(); 
            var json = JsonMapper.ToJson(data); 
            sw.WriteLine(json); 
            sw.Close(); 
            sw.Dispose(); 
            AssetDatabase.Refresh();
        }

    }
}