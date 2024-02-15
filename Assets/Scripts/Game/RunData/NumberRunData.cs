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
 * 2.胜利失败弹窗优化动效弹出
 * 3.关卡模式下胜利弹窗显示两个按钮, 重来和下一关
 * 4.记录已通关关卡及通关时间
 * 5.随机模式可选择简单,中等,困难
 * 6.按100个关卡为9X9的矩阵种子, 以变换矩阵(交换大小行/大小列/数字,矩阵装置)形式生成随机游戏,提高生成速度,日常挑战保证随机结果当日不变化
 *
 * 待完成(...):
 * 42,43,44关卡数据生成错误带修正
 *
 * 可选完成项(?):
 * 7.游戏中新增回退操作按钮,回退上一步操作
 */

namespace Game.RunData
{
    public class NumberRunData : BaseSingleton<NumberRunData>
    {
        [Header("颜色配置")]
        public ColorConfig colorConf;

        [Header("数据配置")] 
        public NumberDataCtr dataCtr;

        public Color OriginTextColor(bool editAble)
        {
            return editAble ? colorConf.variableTextColor : colorConf.fixTextColor;
        }

        [HideInInspector]
        public InputNumber InputNumberDelegate;
        
        [HideInInspector]
        public NumberBgGradient NumberGradientDelegate;

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

        public void Generate()
        {
            dataCtr.ClearData();
            dataCtr.GenerateBySeed();
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
                NumberGradientDelegate(result.ConvertAll((e) => e.ItemKey));
            }
        }
    }
}
