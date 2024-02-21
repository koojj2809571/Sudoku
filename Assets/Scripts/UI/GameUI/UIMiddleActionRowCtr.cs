using Game;
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
            if (!Data.GameReady) return;
            CommandRecorder.Instance.Undo();
        }

        public void ClickDeleteItem()
        {
            if (!Data.GameReady) return;
            if(Data.CurKey == "")return;
            if(Data.dataCtr.numberData.Count == 0) return;
            var curItem = Data.CurItem;
            if (!curItem.editAble) return;
            var lastValue = curItem.Value;
            curItem.Value = 0;
            CommandRecorder.Instance.AddCommand(curItem.itemIndex, lastValue, curItem.ItemKey, curItem.error);
        }

        public void ClickNoteSwitcher()
        {
            if (!Data.GameReady) return;
            NumberRunData.Instance.dataCtr.isNote = !NumberRunData.Instance.dataCtr.isNote;
        }

        public void ClickTip()
        {
            if (!Data.GameReady) return;
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
