using System;
using System.Collections.Generic;
using System.Linq;
using Game.Item;
using Game.RunData;
using JetBrains.Annotations;
using UI;
using Util;
using Random = UnityEngine.Random;

namespace Game.Config
{
    [Serializable]
    public class NumberDataCtr
    {
        public string curKey = "";
        public int levelCount = 30;
        public int errorTimes;
        public bool gamePause;
        public bool isNote;
        public bool isGenerating;
        public List<string> errorKeyCache = new();
        public List<NumberItem> numberData = new();
        public readonly Dictionary<string, NumberItem> NumDict = new();
        public List<int> answer = new();
        

        private int _loopTimes;
        private DateTime _startGenerateTime;

        public UIInfoCtr infoCtr;

        public void SortData()
        {
            numberData.Sort((x,y) => x.itemIndex.CompareTo(y.itemIndex));
        }


        public void ClearData()
        {
            errorTimes = 0;
            _loopTimes = 0;
            infoCtr.RestartTimer();
            answer.Clear();
            numberData = numberData.Select(e =>
            {
                e.Value = 0;
                return e;
            }).ToList();
        }

        public void RandomNumber()
        {
            if(isGenerating) return;
            _startGenerateTime = DateTime.Now;
            isGenerating = true;
            var count = 0;
            while (true)
            {
                var areas = NumDataUtil.GetDataByRegion(Region.Area);

                for (var i = 1; i <= 9; i++)
                {
                    var usedRow = new List<int>();
                    var usedCol = new List<int>();
                    foreach (var area in areas)
                    {
                        var random = GetRandomItemIndex(area, usedRow, usedCol);
                        if (random == null) continue;
                        usedRow.Add(random.row);
                        usedCol.Add(random.column);
                        random.Value = i;
                        random.editAble = false;
                    }
                }

                if (numberData.Any(e => e.value == 0))
                {
                    ClearData();
                    count++;
                    continue;
                }

                _loopTimes = count;
                answer = numberData.Select(e => e.value).ToList();
                RandomEmpty();
                
                break;
            }
            LogUtil.Log($"循环{count}次, 耗时{(DateTime.Now - _startGenerateTime).TotalSeconds}秒");
            isGenerating = false;
            infoCtr.stopTimer = false;
        }

        [CanBeNull]
        private NumberItem GetRandomItemIndex(List<NumberItem> area, List<int> row, List<int> col)
        {
            var excludeHasValue = area.FindAll(e => e.value == 0);
            var excludeSameRow = excludeHasValue.FindAll(e => !row.Contains(e.row));
            var excludeSameCol = excludeSameRow.FindAll(e => !col.Contains(e.column));
            var random = Random.Range(0, excludeSameCol.Count);
            return excludeSameCol.Count == 0 ? null : excludeSameCol[random];
        }

        private void RandomEmpty()
        {
            var nextRandom = new System.Random(_loopTimes);
            var indexSet = new HashSet<int>();
            while (indexSet.Count < levelCount)
            {
                var index = nextRandom.Next(80);
                indexSet.Add(index);
                LogUtil.Log($"{index} 数量: {indexSet.Count}");
                numberData[index].Value = 0;
                numberData[index].editAble = true;
            }
        }
    }
}