using System;
using Game.RunData;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
namespace UI
{
    public class FinishDialogCtr : MonoBehaviour
    {


        public Button againBt;
        public Button nextBt;
        public Text dialogTitle;
        public Image finishIcon;

        public Sprite success;
        public Sprite fail;

        public string successText;
        public string failText;

        public void PlayAgain()
        {
            NumberRunData.Instance.Generate();
            GameUIManager.Instance.uiDialogCtr.HideDialog();
        }

        public void NextLevel()
        {
            
        }

        public void SetFinishType(DialogType type)
        {
            switch (type)
            {
                case DialogType.FinishWithSuccess:
                    finishIcon.sprite = success;
                    dialogTitle.text = successText;
                    againBt.gameObject.SetActive(true);
                    nextBt.gameObject.SetActive(LevelRunData.Instance.SelectedLevelIndex != -1);
                    break;
                case DialogType.FinishWithFail:
                    finishIcon.sprite = fail;
                    dialogTitle.text = failText;
                    againBt.gameObject.SetActive(true);
                    nextBt.gameObject.SetActive(false);
                    break;
            }
        }
    }
}
