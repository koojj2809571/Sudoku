using System.Collections.Generic;
using System.Linq;
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

        [HideInInspector] 
        public NumberDataCtr dataCtr;

        public Color OriginTextColor(bool editAble)
        {
            return editAble ? colorConf.variableTextColor : colorConf.fixTextColor;
        }

        [HideInInspector]
        public NumberGradient NumberGradientDelegate;

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
        public NumberItem CurItem => dataCtr.NumDict[dataCtr.curKey];

        public bool GameReady => !dataCtr.isGenerating && !dataCtr.startAniProcessing;

        private void Start()
        {
            dataCtr = gameObject.GetComponent<NumberDataCtr>();
        }

        public void Generate()
        {
            dataCtr.ClearData();
            dataCtr.GenerateGame();
        }
        

        public void CheckSuccess()
        {
            foreach (var item in dataCtr.numberData)
            {
                if (item.error) return;
                if (item.value == 0) return;
            }

            GameUIManager.Instance.uiInfoCtr.stopTimer = true;
            GameUIManager.Instance.uiDialogCtr.ShowDialog(DialogType.FinishWithSuccess);
            LevelRunData.Instance.UpdateLevelResult(GameUIManager.Instance.uiInfoCtr.time.text);
        }

        public void FindFinishedRelationSquares()
        {
            var result = new List<NumberItem>();
            var relationRowItems = dataCtr.RowData[CurRow - 1];
            if (relationRowItems.All(e => e.value != 0))
            {
                result.AddRange(relationRowItems);
            }
            var relationColItems = dataCtr.ColData[CurColumn - 1];
            if (relationColItems.All(e => e.value != 0))
            {
                result.AddRange(relationColItems);
            }
            var relationAreaItems = dataCtr.AreaData[CurArea - 1];
            if (relationAreaItems.All(e => e.value != 0))
            {
                result.AddRange(relationAreaItems);
            }
            if (result.Count > 0)
            {
                NumberGradientDelegate(new GradientParam
                {
                    Type = ItemAniType.BgAni,
                    ProcessAniNumbers = result.ConvertAll((e) => e.ItemKey),
                    Target = colorConf.finishItemGradientColor,
                    IsPlayBack = true
                });
            }
        }
        
    }
}
