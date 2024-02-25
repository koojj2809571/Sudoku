using System.Linq;
using Game;
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
            CommandRecorder.Instance.InputButtons.Add(content,this);
        }

        public void OnNumBtClick()
        {
            if(!_canClick) return;
            if (!NumRunData.GameReady) return;
            if(NumRunData.CurKey == "") return;
            var curItem = NumRunData.CurItem;
            var lastValue = curItem.Value;
            if(!curItem.editAble) return;
            if (NumRunData.dataCtr.isNote && curItem.value == 0)
            {
                curItem.notePanel.SetNoteVisible(content, true);
                return;
            }

            curItem.notePanel.HideNoteSquare();
            CommandRecorder.Instance.AddCommand(curItem.itemIndex, lastValue, curItem.ItemKey, curItem.error);
            curItem.Value = content;
            curItem.notePanel.ClearRelationSquareNote();
            NumRunData.FindFinishedRelationSquares();
            CheckCanClickCurNum();
            NumRunData.CheckSuccess();
        }

        public void CheckCanClickCurNum()
        {
            _canClick = !NumRunData.dataCtr.AreaData.All(
                area => area.Select(
                    e => e.value
                ).ToList().Contains(content)
            );
            number.text = _canClick ? NumText : "";
        }
    }
}
