using System;
using UnityEngine;

namespace UI
{
    public enum DialogType
    {
        Pause, Finish
    }
    public class UIDialogCtr : MonoBehaviour
    {

        public GameObject pausePanel;
        public GameObject finishPanel;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void ShowDialog(DialogType type)
        {
            gameObject.SetActive(true);
            switch (type)
            {
                case DialogType.Pause:
                    pausePanel.SetActive(true);
                    break;
                case DialogType.Finish:
                    finishPanel.SetActive(true);
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
