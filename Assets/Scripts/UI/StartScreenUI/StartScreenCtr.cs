using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// ReSharper disable once CheckNamespace
namespace UI
{
    public class StartScreenCtr : MonoBehaviour
    {

        public LevelDialogCtr levelCtr;
        
        public void ClickPlay()
        {
            LevelRunData.Instance.SelectedLevelIndex = -1;
            SceneManager.LoadScene(1);
        }

        public void ClickLevelSelectBt()
        {
            levelCtr.gameObject.SetActive(true);
            StartCoroutine(OpenDialog());
        }

        private IEnumerator OpenDialog()
        {
            yield return new WaitForSeconds(0.3f);
            levelCtr.DialogSwitcher(true);
        }

        public void CloseDialog()
        {
            levelCtr.DialogSwitcher(false);
        }
    }
}
