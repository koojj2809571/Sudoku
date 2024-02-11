using Game.RunData;
using UnityEngine;

// ReSharper disable once CheckNamespace
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
