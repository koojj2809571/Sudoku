using Game.RunData;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            NumberRunData.Instance.dataCtr.gamePause = true;
            pausePanel.ShowDialog(DialogType.Pause);
        }
        
        public void ContinueGame()
        {
            NumberRunData.Instance.dataCtr.gamePause = false;
            pausePanel.HideDialog();
        }
    }
}
