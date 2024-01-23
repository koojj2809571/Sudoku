using Game.RunData;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Item
{
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
        public Text num;

        private string ItemKey
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
                if (Data.dataCtr.errorKeyCache.Contains(ItemErrorKey))
                {
                    Data.dataCtr.errorKeyCache.Remove(ItemErrorKey);
                }

                var lastValue = this.value;
                this.value = value;
                num.text = value != 0 ? this.value.ToString() : "";
                if (lastValue == 0 && value != 0)
                {
                    Data.InputNumberDelegate(value, 1);
                }

                if (lastValue != 0)
                {
                    Data.InputNumberDelegate(lastValue, -1);
                }
                if (!CheckError()) return;
                if (!Data.dataCtr.errorKeyCache.Contains(ItemErrorKey) && error)
                {
                    Data.dataCtr.errorKeyCache.Add(ItemErrorKey);
                    Data.dataCtr.errorTimes += 1;
                }

            }
        }
        
        private void Start()
        {
            Init();
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
            Data.dataCtr.RandomNumber();
        }

        public void ShowNoteItem(int clickNumber)
        {
            notePanel.noteItems[clickNumber - 1].SetActive(true);
        }
        
        public void HideNoteItem()
        {
            foreach (var t in notePanel.noteItems)
            {
                t.SetActive(false);
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
    }
}
