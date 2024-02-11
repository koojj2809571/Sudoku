using System;
using System.Collections.Generic;
using System.Linq;
using Game.Item;
using Game.RunData;
using UI;
using Util;

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
        public List<List<NumberItem>> rowData = new();
        public List<List<NumberItem>> colData = new();
        public List<List<NumberItem>> areaData = new();
        

        private int _loopTimes;
        private DateTime _startGenerateTime;

        public UIInfoCtr infoCtr;

        private RandomUtil _ranUtil;
        public RandomUtil RanUtil => _ranUtil ??= new RandomUtil();

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
            LogUtil.Log("随机模式");
            if(isGenerating) return;
            _startGenerateTime = DateTime.Now;
            isGenerating = true;
            var count = 0;
            
            RanUtil.RandomOriginNumbers(ref count, ClearData, c =>
            {
                _loopTimes = c;
                answer = numberData.Select(e => e.value).ToList();
                rowData = NumDataUtil.GetDataByRegion(Region.Row);
                colData = NumDataUtil.GetDataByRegion(Region.Column);
                areaData = NumDataUtil.GetDataByRegion(Region.Area);
                RanUtil.RandomEmpty(_loopTimes, levelCount);
            });
            
            LogUtil.Log($"循环{count}次, 耗时{(DateTime.Now - _startGenerateTime).TotalSeconds}秒");
            isGenerating = false;
            infoCtr.stopTimer = false;
            
        }

        public void GenerateByLevel()
        {
            LogUtil.Log("关卡模式");
            if(isGenerating) return;
            isGenerating = true;

            for (var i = 0; i < numberData.Count; i++)
            {
                numberData[i].Value = LevelRunData.Instance.CurLevel[i];
                numberData[i].editAble = false;
            }
            answer = numberData.Select(e => e.value).ToList();
            rowData = NumDataUtil.GetDataByRegion(Region.Row);
            colData = NumDataUtil.GetDataByRegion(Region.Column);
            areaData = NumDataUtil.GetDataByRegion(Region.Area);
            RanUtil.RandomEmpty(LevelRunData.Instance.SelectedLevelIndex, levelCount);
            
            infoCtr.stopTimer = false;
            isGenerating = false;
        }
    }
}