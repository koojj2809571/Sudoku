using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
namespace UI
{
    public class StartScreenCtr : MonoBehaviour
    {

        public LevelDialogCtr levelCtr;
        public Text finishLevelText;

        private void Start()
        {
            var levelCount = LevelRunData.Instance.LevelResult.Count;
            if (levelCount != 0)
            {
                finishLevelText.text = $"{levelCount} / 100";
            }
        }

        public void ClickPlay(int level)
        {
            LevelRunData.Instance.DiffLevel = level;
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
