using Game.RunData;
using UnityEngine;
using UnityEngine.SceneManagement;

// ReSharper disable once CheckNamespace
namespace UI
{
    public class UITopActionRowCtr : MonoBehaviour
    {
        
        public UIDialogCtr pausePanel;
        public void ClickBack()
        {
            SceneManager.LoadScene(0);
        }
        
        public void ClickSetting()
        {
            
        }
        
        public void ClickPause()
        {
            GameUIManager.Instance.uiDialogCtr.ShowDialog(DialogType.Pause);
            // GameUIManager.Instance.finishDialogCtr.NextLevel();
        }
        
        public void ContinueGame()
        {
            NumberRunData.Instance.dataCtr.gamePause = false;
            GameUIManager.Instance.uiDialogCtr.HideDialog();
        }
    }
}
