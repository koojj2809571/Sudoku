using System;
using Game.RunData;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    public class UIMiddleActionRowCtr : MonoBehaviour
    {

        public Text switcherTag;

        private NumberRunData Data => NumberRunData.Instance;

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
            LogUtil.Log("1");
            if(Data.CurKey == "")return;
            LogUtil.Log("2");
            if(Data.dataCtr.numberData.Count == 0) return;
            LogUtil.Log("3");
            if (!Data.CurItem.editAble) return; 
            Data.CurItem.Value = 0;
        }

        public void ClickNoteSwitcher()
        {
            NumberRunData.Instance.dataCtr.isNote = !NumberRunData.Instance.dataCtr.isNote;
        }
    }
}
