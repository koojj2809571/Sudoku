using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Config;
using Game.Item;
using UI;
using UnityEngine;
using Util;

/* TODO_LIST
 *
 * 完成(√):
 * 1.关联方快被全部填满时显示颜色渐变动画(行,列,九宫格)
 *
 * 待完成(...):
 * 2.胜利失败弹窗优化动效弹出
 * 3.关卡模式下胜利弹窗显示两个按钮, 重来和下一关
 * 4.记录已通关关卡及通关时间
 * 5.随机模式可选择简单,中等,困难
 *
 * 可选完成项(?):
 * 6.游戏中新增回退操作按钮,回退上一步操作
 */

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
        public InputNumber inputNumberDelegate;
        
        [HideInInspector]
        public NumberBgGradient numberGradientDelegate;

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

        protected new void Awake()
        {
            base.Awake();
            dataCtr.infoCtr = infoCtr;
        }

        public void Generate()
        {
            dataCtr.ClearData();
            if (LevelRunData.Instance.SelectedLevelIndex != -1)
            {
                dataCtr.GenerateByLevel();
            }
            else
            {
                dataCtr.RandomNumber();
            }
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

        public void FindFinishedRelationSquares()
        {
            var result = new List<NumberItem>();
            var relationRowItems = dataCtr.rowData[CurRow - 1];
            if (relationRowItems.All(e => e.value != 0))
            {
                result.AddRange(relationRowItems);
            }
            var relationColItems = dataCtr.colData[CurColumn - 1];
            if (relationColItems.All(e => e.value != 0))
            {
                result.AddRange(relationColItems);
            }
            var relationAreaItems = dataCtr.areaData[CurArea - 1];
            if (relationAreaItems.All(e => e.value != 0))
            {
                result.AddRange(relationAreaItems);
            }
            if (result.Count > 0)
            {
                numberGradientDelegate(result.ConvertAll((e) => e.ItemKey));
            }
        }
    }
}
