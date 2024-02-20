using System;
using Game.RunData;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
namespace UI
{
    public class UIMiddleActionRowCtr : MonoBehaviour
    {

        public int tipsTimes;
        public Text switcherTag;
        public Text tipsTag;

        private int _curTimes;

        private NumberRunData Data => NumberRunData.Instance;

        private void Start()
        {
            _curTimes = tipsTimes;
            SetTipsText();
        }

        private void Update()
        {
            switcherTag.text = NumberRunData.Instance.dataCtr.isNote ? "On" : "Off";
        }

        public void ClickRestart()
        {
            Data.Generate();
        }

        public void ClickDeleteItem()
        {
            if(Data.CurKey == "")return;
            if(Data.dataCtr.numberData.Count == 0) return;
            if (!Data.CurItem.editAble) return; 
            Data.CurItem.Value = 0;
        }

        public void ClickNoteSwitcher()
        {
            NumberRunData.Instance.dataCtr.isNote = !NumberRunData.Instance.dataCtr.isNote;
        }

        public void ClickTip()
        {
            if (_curTimes <= 0) return;
            if (!Data.CurItem.editAble) return;
            if (Data.CurItem.Value != 0) return;
            _curTimes--;
            var curItem = Data.CurItem;
            curItem.Value = Data.dataCtr.answer[curItem.itemIndex];
            SetTipsText();
        }

        private void SetTipsText()
        {
            tipsTag.text = $"{_curTimes}";
        }
    }
}
