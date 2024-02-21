using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Item;
using Game.RunData;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using Util;

namespace Game.Config
{
    [Serializable]
    public class NumberDataCtr: MonoBehaviour
    {
        public string curKey = "";
        public int errorTimes;
        public bool gamePause;
        public bool isNote;
        public bool isGenerating;
        public bool startAniProcessing;
        public List<string> errorKeyCache = new();
        public List<NumberItem> numberData = new();
        public readonly Dictionary<string, NumberItem> NumDict = new();
        public List<int> answer = new();
        public List<List<NumberItem>> RowData = new();
        public List<List<NumberItem>> ColData = new();
        public List<List<NumberItem>> AreaData = new();
        

        private int _loopTimes;
        private DateTime _startGenerateTime;

        private RandomUtil _ranUtil;
        public RandomUtil RanUtil => _ranUtil ??= new RandomUtil();
        
        public int LevelCount
        {
            get
            {
                var curLevel = LevelRunData.Instance.SelectedGameIndex;
                if (curLevel != -1)
                {
                    return curLevel switch
                    {
                        <= 25 => 28,
                        <= 50 => 36,
                        <= 75 => 45,
                        _ => 54
                    };
                }

                return LevelRunData.Instance.diffLevel != -1 ? LevelRunData.Instance.diffLevel : 30;
            }
        }

        public void SortData()
        {
            numberData.Sort((x,y) => x.itemIndex.CompareTo(y.itemIndex));
        }


        public void ClearData()
        {
            errorTimes = 0;
            _loopTimes = 0;
            GameUIManager.Instance.uiInfoCtr.RestartTimer();
            answer.Clear();
            numberData = numberData.Select(e =>
            {
                e.Value = 0;
                return e;
            }).ToList();
        }
        
        public void GenerateGame()
        {
            if(isGenerating) return;
            isGenerating = true;
            startAniProcessing = true;
            var curGame = LevelRunData.Instance.CurGame;
            if (curGame.Count == 0)
            {
                RandomNumber();
            }
            else
            {
                for (var i = 0; i < numberData.Count; i++)
                {
                    numberData[i].Value = curGame[i];
                    numberData[i].editAble = false;
                }
                answer = curGame.ToList();
                RowData = NumDataUtil.GetDataByRegion(Region.Row);
                ColData = NumDataUtil.GetDataByRegion(Region.Column);
                AreaData = NumDataUtil.GetDataByRegion(Region.Area);
                RanUtil.RandomEmpty(LevelRunData.Instance.RanSeed, LevelCount);
            }
            StartCoroutine(nameof(PlayStartAni));
            isGenerating = false;
        }

        public void RandomNumber()
        {
            DebugUtil.Log("随机模式");
            _startGenerateTime = DateTime.Now;
            var count = 0;
            
            RanUtil.RandomOriginNumbers(ref count, ClearData, c =>
            {
                _loopTimes = c;
                answer = numberData.Select(e => e.value).ToList();
                RowData = NumDataUtil.GetDataByRegion(Region.Row);
                ColData = NumDataUtil.GetDataByRegion(Region.Column);
                AreaData = NumDataUtil.GetDataByRegion(Region.Area);
                RanUtil.RandomEmpty(_loopTimes, LevelCount);
            });
            
            DebugUtil.Log($"循环{count}次, 耗时{(DateTime.Now - _startGenerateTime).TotalSeconds}秒");
        }

        private IEnumerator PlayStartAni()
        {
            for (var i = 0; i < RowData.Count; i++)
            {
                if (i == 5)
                {
                    StartCoroutine(nameof(StartShowNumber));
                }
                NumberRunData.Instance.NumberGradientDelegate(new GradientParam
                {
                    ProcessAniNumbers = RowData[i].Select(e => e.ItemKey).ToList(),
                    Target = Color.white.WithAlpha(0),
                    Type = ItemAniType.BgAni,
                    Duration = 0.1f
                });
                yield return new WaitForSeconds(0.1f);
            }

            yield return null;
        }

        private IEnumerator StartShowNumber()
        {
            foreach (var t in RowData)
            {
                NumberRunData.Instance.NumberGradientDelegate(new GradientParam
                {
                    ProcessAniNumbers = t.Select(e => e.ItemKey).ToList(),
                    Target = NumberRunData.Instance.colorConf.fixTextColor,
                    Type = ItemAniType.TextAni,
                    Duration = 0.1f
                });
                yield return new WaitForSeconds(0.1f);
            }
            startAniProcessing = false;
            GameUIManager.Instance.uiInfoCtr.stopTimer = false;
            yield return null;
        }
    }
}