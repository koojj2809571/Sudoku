using UnityEngine;
using UnityEngine.UI;

namespace Game.Item
{
    public class NoteItem : MonoBehaviour
    {
        public Text noteNum;

        private string ShowNumber => gameObject.name[^1..];
        
        private void Start()
        {
            noteNum.text = ShowNumber;
        }
    }
}