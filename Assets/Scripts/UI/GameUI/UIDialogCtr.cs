using System;
using DG.Tweening;
using Game.RunData;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace UI
{
    public enum DialogType
    {
        Pause, FinishWithSuccess, FinishWithFail 
    }
    public class UIDialogCtr : MonoBehaviour
    {

        public GameObject pausePanel;
        public GameObject finishPanel;
        
        public void ShowDialog(DialogType type)
        {
            NumberRunData.Instance.dataCtr.gamePause = true;
            gameObject.SetActive(true);
            switch (type)
            {
                case DialogType.Pause:
                    pausePanel.SetActive(true);
                    break;
                case DialogType.FinishWithFail:
                case DialogType.FinishWithSuccess:
                    finishPanel.SetActive(true);
                    GameUIManager.Instance.finishDialogCtr.SetFinishType(type);
                    finishPanel.GetComponent<RectTransform>().DOLocalRotate(Vector3.zero, 1f);
                    break;
                default:
                    gameObject.SetActive(false);
                    break;
            }
        }

        public void HideDialog()
        {
            var transforms = GetComponentsInChildren<Transform>();
            foreach (var subTran in transforms)
            {
                subTran.gameObject.SetActive(false);
            }
        }
    }
}
