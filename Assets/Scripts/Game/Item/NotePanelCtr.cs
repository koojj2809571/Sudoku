using System.Linq;
using Game.RunData;
using UnityEngine;

namespace Game.Item
{
    public class NotePanelCtr : MonoBehaviour
    {
        private bool _showNote;
        public GameObject[] noteItems;

        private bool ShowNote => _showNote;

        private NumberItem _item;

        private NumberRunData Data => NumberRunData.Instance;

        private void Start()
        {
            _item = transform.parent.gameObject.GetComponent<NumberItem>();
        }

        public void SetNoteVisible(int clickNumber, bool visible)
        {
            noteItems[clickNumber - 1].SetActive(visible);
            _showNote = noteItems.Any(e => e.activeSelf);
        }
        
        public void HideNoteSquare()
        {
            foreach (var t in noteItems)
            {
                t.SetActive(false);
            }

            _showNote = false;
        }
        
        public void ClearRelationSquareNote()
        {
            var sameRow = Data.dataCtr.RowData[_item.row - 1];
            var sameCol = Data.dataCtr.ColData[_item.column - 1];
            var sameArea = Data.dataCtr.AreaData[_item.area - 1];
            for (var i = 0; i < sameRow.Count; i++)
            {
                var rowItem = sameRow[i];
                var colItem = sameCol[i];
                var areaItem = sameArea[i];
                if (rowItem.ItemKey != _item.ItemKey && rowItem.notePanel.ShowNote)
                {
                    rowItem.notePanel.SetNoteVisible(_item.value, false);
                }
                if (colItem.ItemKey != _item.ItemKey && colItem.notePanel.ShowNote)
                {
                    colItem.notePanel.SetNoteVisible(_item.value, false);
                }
                if (areaItem.ItemKey != _item.ItemKey && areaItem.notePanel.ShowNote)
                {
                    areaItem.notePanel.SetNoteVisible(_item.value, false);
                }
            }
        }
    }
}
