using Game.RunData;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{

    public delegate void InputNumber(int number, int changeValue);
    
    public class UINumberInputButton : MonoBehaviour
    {
        
        public int content;
        public Text number;
        public int numberUsedTimes;

        private bool _canClick;

        private string NumText => content.ToString();
        private NumberRunData NumRunData => NumberRunData.Instance;
        
        void Start()
        {
            number.text = NumText;
            numberUsedTimes = 0;
            _canClick = true;
            NumRunData.inputNumberDelegate += OnInputNumber;
        }

        public void OnNumBtClick()
        {
            if(!_canClick) return;
            if(NumRunData.CurKey == "") return;
            if(!NumRunData.CurItem.editAble) return;
            if (NumRunData.dataCtr.isNote && NumRunData.CurItem.value == 0)
            {
                NumRunData.CurItem.notePanel.SetNoteVisible(content, true);
                return;
            }
            NumRunData.CurItem.notePanel.HideNoteSquare();
            NumRunData.CurItem.Value = content;
            NumRunData.CurItem.ClearRelationSquareNote();
            NumRunData.CheckSuccess();
        }

        private void OnInputNumber(int numberValue, int changeValue)
        {
            if(numberValue != content) return;
            numberUsedTimes += changeValue;
            _canClick = numberUsedTimes < 9;
            number.text = _canClick ? NumText : "";
        }
    }
}
