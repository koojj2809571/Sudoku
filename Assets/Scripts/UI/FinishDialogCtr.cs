using Game.RunData;
using UnityEngine;

namespace UI
{
    public class FinishDialogCtr : MonoBehaviour
    {

        public UIDialogCtr dialogCtr;
        
        public void PlayAgain()
        {
            NumberRunData.Instance.Generate();
            dialogCtr.HideDialog();
        }
    }
}
