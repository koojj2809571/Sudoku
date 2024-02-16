using System.Collections.Generic;
using DG.Tweening;
using Game.RunData;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Item
{
    public delegate void NumberBgGradient(List<string> finishNumbers);
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
            set
            {
                //赋值前移除错误记录缓存
                if (Data.dataCtr.errorKeyCache.Contains(ItemErrorKey))
                {
                    Data.dataCtr.errorKeyCache.Remove(ItemErrorKey);
                }

                //记录上一个值
                var lastValue = this.value;
                
                //赋值
                this.value = value;
                num.text = value != 0 ? this.value.ToString() : "";
                
                //记录数字使用次数
                if (lastValue == 0 && value != 0)
                {
                    Data.InputNumberDelegate(value, 1);
                }

                //清空时扣减上一个数字使用次数
                if (lastValue != 0)
                {
                    Data.InputNumberDelegate(lastValue, -1);
                }
                
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
        
        private void Start()
        {
            Init();
            Data.NumberGradientDelegate += OnBgGradient;
        }

        private void OnBgGradient(List<string> finishNumbers)
        {
            if (!finishNumbers.Contains(ItemKey)) return;
            Tweener doColor = aniMask.DOColor(Data.colorConf.finishItemGradientColor, 0.3f);
            doColor.SetAutoKill(false);
            doColor.OnComplete(() =>
            {
                doColor.PlayBackwards();
            });
        }

        private void Update()
        {
            UpdateErrorColor();
            UpdateItemColor();
        }

        private void Init()
        {
            var rootRowNumber = 0;
            var areNumber = 0;
            var subRowNumber = 0;
            var subItemNumber = 0;
            foreach (var trans in gameObject.GetComponentsInParent<Transform>())
            {
                // LogUtil.Log($"{trans.tag} -- {GetCountByName(trans.name)}");
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
            // Data.RowArr[row - 1, column - 1] = this;
            // Data.ColArr[column - 1, row - 1] = this;
            if (Data.dataCtr.numberData.Count < 81) return;
            Data.dataCtr.SortData();
            var levelRunData = LevelRunData.Instance;
            if (levelRunData == null)
            {
                Data.dataCtr.RandomNumber();
            }
            else
            {
                Data.dataCtr.GenerateBySeed();
            }
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
            if(value == 0) return false;
            var usedInRow = NumDataUtil.UsedInRegion(Region.Row,row, value);
            var usedInCol = NumDataUtil.UsedInRegion(Region.Column,column, value);
            var usedInArea = NumDataUtil.UsedInRegion(Region.Area,area, value);
            
            error = usedInRow || usedInCol || usedInArea;

            return error;
        }

        private void UpdateErrorColor()
        {
            CheckError();
            num.color = error ? Data.colorConf.errorColor : Data.OriginTextColor(editAble);
        }

        public void OnItemSelected()
        {
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
                    rowItem.notePanel.SetNoteVisible(this.value, false);
                }
                if (colItem.ItemKey != ItemKey && colItem.notePanel.ShowNote)
                {
                    colItem.notePanel.SetNoteVisible(this.value, false);
                }
                if (areaItem.ItemKey != ItemKey && areaItem.notePanel.ShowNote)
                {
                    areaItem.notePanel.SetNoteVisible(this.value, false);
                }
            }
        }
    }
}
