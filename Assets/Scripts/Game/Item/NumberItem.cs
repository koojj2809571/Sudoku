using System;
using System.Collections.Generic;
using DG.Tweening;
using Game.RunData;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Item
{
    public enum ItemAniType
    {
        BgAni, TextAni
    }

    public class GradientParam
    {
        public ItemAniType Type;
        public Color Target;
        public List<string> ProcessAniNumbers;
        public float Duration = 0.3f;
        public bool IsPlayBack;

        public bool InValidParam(string key) => ProcessAniNumbers == null || !ProcessAniNumbers.Contains(key);
    }
    public delegate void NumberGradient(GradientParam param);
    public class NumberItem: MonoBehaviour
    {

        public NotePanelCtr notePanel; 
        public int value;
        public int row;
        public int column;
        public int area;
        public int itemIndex = -1;
        public bool editAble;
        public bool error;

        public Image bg;
        public Image aniMask;
        public Text num;

        public string ItemKey
        {
            get
            {
                if (area == 0 || row == 0 || column == 0 || itemIndex == -1) return "";
                return $"{area}-{row}-{column}-{itemIndex}";
            }
        }

        private string ItemErrorKey => $"{ItemKey}{value}";

        private NumberRunData Data => NumberRunData.Instance;

        public int Value
        {
            get => value;
            set
            {
                if(this.value == value) return;
                //赋值前移除错误记录缓存
                if (Data.dataCtr.errorKeyCache.Contains(ItemErrorKey))
                {
                    Data.dataCtr.errorKeyCache.Remove(ItemErrorKey);
                }

                //赋值
                SetValue(value);
                
                //检查数字是否错误
                if (!CheckError()) return;
                
                //记录错误次数并缓存错误item键
                if (!Data.dataCtr.errorKeyCache.Contains(ItemErrorKey) && error)
                {
                    Data.dataCtr.errorKeyCache.Add(ItemErrorKey);
                    Data.dataCtr.errorTimes += 1;
                    if (Data.dataCtr.errorTimes >= 5)
                    {
                        GameUIManager.Instance.uiDialogCtr.ShowDialog(DialogType.FinishWithFail);
                    }
                }

            }
        }

        public void SetValue(int v)
        {
            value = v;
            num.text = value != 0 ? value.ToString() : "";
        }
        
        private void Start()
        {
            Init();
            Data.NumberGradientDelegate += OnGradient;
        }

        private void OnGradient(GradientParam param)
        {
            if (param.InValidParam(ItemKey)) return;
            Tweener doColor = param.Type switch
            {
                ItemAniType.BgAni => aniMask.DOColor(param.Target, param.Duration),
                ItemAniType.TextAni => num.DOColor(param.Target, param.Duration),
                _ => throw new ArgumentOutOfRangeException(nameof(param.Type), param.Type, "渐变动画参数异常")
            };
            if(!param.IsPlayBack) return;
            doColor.SetAutoKill(false);
            doColor.OnComplete(() =>
            {
                aniMask.DOPlayBackwards();
            });
        }

        private void Update()
        {
            if (Data.dataCtr.startAniProcessing) return;
            UpdateErrorColor();
            UpdateItemColor();
        }

        private void Init()
        {
            aniMask.color = Data.colorConf.startBgColor;
            num.color = num.color.WithAlpha(0);
            
            var rootRowNumber = 0;
            var areNumber = 0;
            var subRowNumber = 0;
            var subItemNumber = 0;
            foreach (var trans in gameObject.GetComponentsInParent<Transform>())
            {
                switch (trans.tag)
                {
                    case "RootRow":
                        rootRowNumber = GetCountByName(trans.name);
                        break;
                    case "Area":
                        areNumber = GetCountByName(trans.name);
                        break;
                    case "SubRow":
                        subRowNumber = GetCountByName(trans.name);
                        break;
                    case "SubItem":
                        subItemNumber = GetCountByName(trans.name);
                        break;
                }
            }

            area = (rootRowNumber - 1) * 3 + areNumber;
            row = (rootRowNumber - 1) * 3 + subRowNumber;
            column = (areNumber - 1) * 3 + subItemNumber;

            itemIndex = (row - 1) * 9 + column - 1;
            Data.dataCtr.numberData.Add(this);
            Data.dataCtr.NumDict.Add(ItemKey, this);
            if (Data.dataCtr.numberData.Count < 81) return;
            Data.dataCtr.SortData();
            Data.dataCtr.GenerateGame();
        }

        private int GetCountByName(string goName)
        {
            return int.Parse(goName[^1..]);
        }

        private void UpdateItemColor()
        {
            if(Data.CurKey == "") return;
            var item = Data.CurItem;
            if (ItemKey == Data.CurKey)
            {
                bg.color = Data.colorConf.selectedColor;
                return;
            }

            if (value != 0 && item.value == value)
            {
                bg.color = Data.colorConf.sameColor;
                return;
            }
            if (area == Data.CurArea || row == Data.CurRow || column == Data.CurColumn)
            {
                bg.color = Data.colorConf.relationColor;
                return;
            }
            
            bg.color = Data.colorConf.originBgColor;
        }

        private bool CheckError()
        {
            if(Data.dataCtr.isGenerating || value == 0) return false;
            error = value != Data.dataCtr.answer[itemIndex];

            return error;
        }

        private void UpdateErrorColor()
        {
            CheckError();
            num.color = error ? Data.colorConf.errorColor : Data.OriginTextColor(editAble);
        }

        public void OnItemSelected()
        {
            if (!Data.GameReady) return;
            NumberRunData.Instance.CurKey = ItemKey;
        }

        public void ClearRelationSquareNote()
        {
            var sameRow = Data.dataCtr.RowData[row - 1];
            var sameCol = Data.dataCtr.ColData[column - 1];
            var sameArea = Data.dataCtr.AreaData[area - 1];
            for (var i = 0; i < sameRow.Count; i++)
            {
                var rowItem = sameRow[i];
                var colItem = sameCol[i];
                var areaItem = sameArea[i];
                if (rowItem.ItemKey != ItemKey && rowItem.notePanel.ShowNote)
                {
                    rowItem.notePanel.SetNoteVisible(value, false);
                }
                if (colItem.ItemKey != ItemKey && colItem.notePanel.ShowNote)
                {
                    colItem.notePanel.SetNoteVisible(value, false);
                }
                if (areaItem.ItemKey != ItemKey && areaItem.notePanel.ShowNote)
                {
                    areaItem.notePanel.SetNoteVisible(value, false);
                }
            }
        }
    }
}
