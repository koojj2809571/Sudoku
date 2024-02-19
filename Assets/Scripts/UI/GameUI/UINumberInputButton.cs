using System.Linq;
using Game.RunData;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
namespace UI
{

    public class UINumberInputButton : MonoBehaviour
    {
        
        public int content;
        public Text number;

        private bool _canClick;

        private string NumText => content.ToString();
        private NumberRunData NumRunData => NumberRunData.Instance;

        private void Start()
        {
            number.text = NumText;
            _canClick = true;
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
            NumRunData.FindFinishedRelationSquares();
            CheckCanClickCurNum();
            NumRunData.CheckSuccess();
        }

        private void CheckCanClickCurNum()
        {
            var allContain = NumRunData.dataCtr.AreaData.All(
                area => area.Select(
                    e => e.value
                ).ToList().Contains(content)
            );
            if (!allContain) return;
            _canClick = false;
            number.text = _canClick ? NumText : "";
        }
    }
}
