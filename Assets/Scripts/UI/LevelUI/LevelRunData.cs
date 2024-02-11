using System;
using System.Collections.Generic;
using System.Linq;
using Util;

// ReSharper disable once CheckNamespace
namespace UI
{
    public class LevelRunData: BaseSingleton<LevelRunData>
    {
        private static List<List<int>> _levels;

        public List<List<int>> Level => _levels;

        public int SelectedLevelIndex { get; set; } = -1;

        public List<int> CurLevel => SelectedLevelIndex == -1 ? new List<int>() : _levels[SelectedLevelIndex];

        public Dictionary<int,string> LevelResult
        {
            get
            {
                var result = new Dictionary<int,string>();
                var data = AssetsUtil.LevelResultData;
                if (data.Count <= 0) return result;
                foreach (var split in data.Select(e => e.Split("-")))
                {
                    result.Add(int.Parse(split[0]), split[1]);
                }
                return result;
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            _levels = AssetsUtil.LevelData;
        }
    }
}