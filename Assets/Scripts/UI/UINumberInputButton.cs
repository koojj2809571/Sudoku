using Game.RunData;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    public class UINumberInputButton : MonoBehaviour
    {
        
        public int content;
        public Text number;

        private string NumText => content.ToString(); 
        
        void Start()
        {
            number.text = NumText;
        }

        public void OnNumBtClick()
        {
            var data = NumberRunData.Instance;
            if(data.curKey == "") return;
            if(!data.CurItem.editAble) return;
            if (data.isNote && data.CurItem.value == 0)
            {
                LogUtil.Log($"Edit: {NumText}");
                data.CurItem.ShowNoteItem(content);
                return;
            }
            LogUtil.Log($"Input: {NumText}");
            data.CurItem.HideNoteItem();
            data.CurItem.Value = content;
            data.CheckSuccess();
        }
    }
}
