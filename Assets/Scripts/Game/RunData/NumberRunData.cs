using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Item;
using JetBrains.Annotations;
using UI;
using UnityEngine;
using Util;

namespace Game.RunData
{
    public class NumberRunData : BaseSingleton<NumberRunData>
    {

        public UIInfoCtr infoCtr;
        public UIDialogCtr dialogCtr;
        public Color originBgColor;
        public Color originTextColor;
        public Color selectedColor;
        public Color relationColor;
        public Color sameColor;
        public Color errorColor;
        public List<NumberItem> numberData = new();
        public List<string> errorKeyCache = new();
        public int errorTimes;
        public bool gamePause;
        public bool isNote;
        public bool isGenerating;
        public readonly Dictionary<string, NumberItem> NumDict = new();

        public string curKey = "";

        // public readonly NumberItem[,] RowArr = new NumberItem[9,9];
        // public readonly NumberItem[,] ColArr = new NumberItem[9,9];

        private int _loopTimes;
        public List<int> answer = new();

        private string[] CurKeyArr
        {
            get
            {
                return curKey == "" ? new string[] { } : curKey.Split('-');
            }
        }

        private bool CurKeyInvalid => curKey == "" || CurKeyArr.Length != 4;

        public int CurArea => CurKeyInvalid ? -1 : int.Parse(CurKeyArr[0]);
        public int CurRow => CurKeyInvalid ? -1 : int.Parse(CurKeyArr[1]);
        public int CurColumn => CurKeyInvalid ? -1 : int.Parse(CurKeyArr[2]);
        public int CurItemIndex => CurKeyInvalid ? -1 : int.Parse(CurKeyArr[3]);
        public NumberItem CurItem => NumDict[curKey];
        
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

        // public void RandomNumber()
        // {
        //     if(isGenerating) return;
        //     StartCoroutine(nameof(RandomNumberCoroutine));
        // }
        
        public void RandomNumber()
        {
            if(isGenerating) return;
            var begin = Time.time;
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
                    }
                }

                if (numberData.Any(e => e.value == 0))
                {
                    ClearData();
                    count++;
                    // yield return null;
                    continue;
                }

                _loopTimes = count;
                answer = numberData.Select(e => e.value).ToList();
                RandomEmpty(30);
                
                break;
            }
            // yield return count;
            LogUtil.Log($"循环{count}次, 耗时{Time.time - begin}秒");
            isGenerating = false;
            infoCtr.stopTimer = false;
        }

        [CanBeNull]
        private NumberItem GetRandomItemIndex(List<NumberItem> area, List<int> row, List<int> col)
        {
            var emptyItems = area.FindAll(e => e.value == 0);
            while (true)
            {
                if (emptyItems.Count == 0) return null;
                var random = Random.Range(0, emptyItems.Count);
                if (emptyItems[random].value == 0 && !row.Contains(emptyItems[random].row) && !col.Contains(emptyItems[random].column)) return emptyItems[random];
                emptyItems.RemoveAt(random);
            }
        }

        private void RandomEmpty(int levelCount)
        {
            var nextRandom = new System.Random(_loopTimes);
            for (var i = 0; i < levelCount; i++)
            {
                var index = nextRandom.Next(80);
                numberData[index].Value = 0;
                numberData[index].editAble = true;
            }
        }

        public void CheckSuccess()
        {
            foreach (var item in numberData)
            {
                if (item.error) return;
                if (item.value == 0) return;
            }

            infoCtr.stopTimer = true;
            dialogCtr.ShowDialog(DialogType.Finish);
        }
    }
}
