using Game.RunData;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIInfoCtr : MonoBehaviour
    {
        
        public Text errorInfo;
        public Text time;

        public float totalTime;
        public bool stopTimer = true;
        
        // Start is called before the first frame update
        private void Start()
        {
            errorInfo.color = NumberRunData.Instance.colorConf.errorColor;
        }

        // Update is called once per frame
        private void Update()
        {
            if (!NumberRunData.Instance.dataCtr.gamePause)
            {
                totalTime += Time.deltaTime;
            }

            SetTimeText();
            SetErrorText();
        }

        private void SetTimeText()
        {
            if (stopTimer) return;
            var totalSeconds = (int)totalTime;
            var sec = totalSeconds % 60;
            var min = totalSeconds / 60;

            var secText = sec < 10 ? $"0{sec}" : $"{sec}";
            var minText = min < 10 ? $"0{min}" : $"{min}";

            time.text = $"{minText}:{secText}";
        }

        private void SetErrorText()
        {
            var errorTimes = NumberRunData.Instance.dataCtr.errorTimes;
            var showError = errorTimes != 0;
            errorInfo.text = showError ? $"Mistake: {errorTimes}" : "";
        }

        public void RestartTimer()
        {
            totalTime = 0;
        }
    }
}
