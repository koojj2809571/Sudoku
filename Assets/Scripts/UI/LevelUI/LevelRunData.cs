using System.Collections.Generic;
using System.Linq;
using Util;

// ReSharper disable once CheckNamespace
namespace UI
{
    public class LevelRunData: BaseSingleton<LevelRunData>
    {
        private static List<List<int>> _levels;

        private Dictionary<int, string> _levelResult;

        public List<List<int>> Level => _levels;

        public int SelectedLevelIndex { get; set; } = -1;

        public List<int> CurLevel => SelectedLevelIndex == -1 ? new List<int>() : _levels[SelectedLevelIndex];

        
        
        public Dictionary<int,string> LevelResult
        {
            get
            {
                if (_levelResult != null) return _levelResult;
                _levelResult = new Dictionary<int,string>();
                var data = AssetsUtil.LevelResultData;
                if (data.Count <= 0) return _levelResult;
                foreach (var split in data.Select(e => e.Split("-")))
                {
                    _levelResult.Add(int.Parse(split[0]), split[1]);
                }
                return _levelResult;
            }
        }

        public void UpdateLevelResult(string finishTime)
        {
            var hasRecord = _levelResult.ContainsKey(SelectedLevelIndex);

            bool needUpdate;
            if (hasRecord)
            {
                needUpdate = NeedUpdate(_levelResult[SelectedLevelIndex], finishTime);
                if (needUpdate)
                {
                    _levelResult[SelectedLevelIndex] = finishTime;
                }
            }
            else
            {
                needUpdate = true;
                _levelResult.Add(SelectedLevelIndex, finishTime);
            }
            if(!needUpdate) return;
            var result = _levelResult.Select(kv => $"{kv.Key}-{kv.Value}").ToList();
            AssetsUtil.LevelResultData = result;
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            _levels = AssetsUtil.LevelData;
        }

        private bool NeedUpdate(string lastTime, string newTime)
        {
            var lastTimeArr = GetTimeArr(lastTime);
            var newTimeArr = GetTimeArr(newTime);
            var minReduce = newTimeArr[0] < lastTimeArr[0];
            var secReduce = newTimeArr[0] == lastTimeArr[0] && newTimeArr[1] < lastTimeArr[1];
            return minReduce || secReduce;
        }

        private int[] GetTimeArr(string time)
        {
            var lastTimeArr = time.Split(":");
            var min = int.Parse(lastTimeArr[0]);
            var sec = int.Parse(lastTimeArr[1]);
            return new[] { min, sec };
        }
    }
}