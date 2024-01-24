using Game.Config;
using Game.Item;
using UI;
using UnityEngine;
using Util;

namespace Game.RunData
{
    public class NumberRunData : BaseSingleton<NumberRunData>
    {
        [Header("颜色配置")]
        public ColorConfig colorConf;

        [Header("控制器")]
        public UIInfoCtr infoCtr;
        public UIDialogCtr dialogCtr;

        [Header("数据配置")] 
        public NumberDataCtr dataCtr;

        public Color OriginTextColor(bool editAble)
        {
            return editAble ? colorConf.variableTextColor : colorConf.fixTextColor;
        }

        [HideInInspector]
        public InputNumber InputNumberDelegate;

        public string CurKey
        {
            get => dataCtr.curKey;
            set => dataCtr.curKey = value;
        }

        private string[] CurKeyArr
        {
            get
            {
                return CurKey == "" ? new string[] { } : CurKey.Split('-');
            }
        }

        private bool CurKeyInvalid => dataCtr.curKey == "" || CurKeyArr.Length != 4;

        public int CurArea => CurKeyInvalid ? -1 : int.Parse(CurKeyArr[0]);
        public int CurRow => CurKeyInvalid ? -1 : int.Parse(CurKeyArr[1]);
        public int CurColumn => CurKeyInvalid ? -1 : int.Parse(CurKeyArr[2]);
        public int CurItemIndex => CurKeyInvalid ? -1 : int.Parse(CurKeyArr[3]);
        public NumberItem CurItem => dataCtr.numDict[dataCtr.curKey];

        protected new void Awake()
        {
            base.Awake();
            dataCtr.infoCtr = infoCtr;
        }

        public void Generate()
        {
            dataCtr.ClearData();
            dataCtr.RandomNumber();
        }

        public void CheckSuccess()
        {
            foreach (var item in dataCtr.numberData)
            {
                if (item.error) return;
                if (item.value == 0) return;
            }

            infoCtr.stopTimer = true;
            dialogCtr.ShowDialog(DialogType.Finish);
        }
    }
}
