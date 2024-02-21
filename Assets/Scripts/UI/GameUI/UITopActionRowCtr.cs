using Game.RunData;
using UnityEngine;
using UnityEngine.SceneManagement;

// ReSharper disable once CheckNamespace
namespace UI
{
    public class UITopActionRowCtr : MonoBehaviour
    {
        
        public void ClickBack()
        {
            if (!NumberRunData.Instance.GameReady) return;
            SceneManager.LoadScene(0);
        }
        
        public void ClickSetting()
        {
            
        }
        
        public void ClickPause()
        {
            if (!NumberRunData.Instance.GameReady) return;
            GameUIManager.Instance.uiDialogCtr.ShowDialog(DialogType.Pause);
        }
        
        public void ContinueGame()
        {
            NumberRunData.Instance.dataCtr.gamePause = false;
            GameUIManager.Instance.uiDialogCtr.HideDialog();
        }
    }
}
