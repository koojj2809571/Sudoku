using System.Linq;
using UnityEngine;

namespace Game.Item
{
    public class NotePanelCtr : MonoBehaviour
    {
        private bool _showNote;
        public GameObject[] noteItems;

        public bool ShowNote => _showNote;

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
    }
}
