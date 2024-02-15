using System.Collections.Generic;
using System.Linq;
using Util;

// ReSharper disable once CheckNamespace
namespace UI
{
    public class LevelRunData: BaseSingleton<LevelRunData>
    {
        private static List<List<int>> _gameSeeds = new();

        private Dictionary<int, string> _gameResult;

        public List<List<int>> GameSeed => _gameSeeds;

        public int SelectedGameIndex { get; set; } = -1;
        
        public int diffLevel = -1;

        private int _ranSeed = -1;
        public int RanSeed
        {
            get => SelectedGameIndex != -1 ? SelectedGameIndex : _ranSeed;
            set => _ranSeed = value;
        }

        public List<int> RandomGame { get; set; }

        public List<int> CurGame
        {
            get
            {
                if (SelectedGameIndex != -1)
                {
                    return _gameSeeds[SelectedGameIndex];
                }

                return RandomGame ?? new List<int>();
            }
        }

        public Dictionary<int,string> GameResult
        {
            get
            {
                if (_gameResult != null) return _gameResult;
                _gameResult = new Dictionary<int,string>();
                var data = AssetsUtil.LevelResultData;
                if (data.Count <= 0) return _gameResult;
                foreach (var split in data.Select(e => e.Split("-")))
                {
                    _gameResult.Add(int.Parse(split[0]), split[1]);
                }
                return _gameResult;
            }
        }

        public void UpdateLevelResult(string finishTime)
        {
            var hasRecord = _gameResult.ContainsKey(SelectedGameIndex);

            bool needUpdate;
            if (hasRecord)
            {
                needUpdate = NeedUpdate(_gameResult[SelectedGameIndex], finishTime);
                if (needUpdate)
                {
                    _gameResult[SelectedGameIndex] = finishTime;
                }
            }
            else
            {
                needUpdate = true;
                _gameResult.Add(SelectedGameIndex, finishTime);
            }
            if(!needUpdate) return;
            var result = _gameResult.Select(kv => $"{kv.Key}-{kv.Value}").ToList();
            AssetsUtil.LevelResultData = result;
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            _gameSeeds = AssetsUtil.LevelData;
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