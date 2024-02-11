using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.LevelUI
{
    public class LevelItem : MonoBehaviour
    {

        public GameObject finishGo;
        public TMP_Text levelNum;
        public TMP_Text finishTime;
        private int _curLevel;
        
        public void SetLevel(int level)
        {
            _curLevel = level;
            levelNum.text = $"{level}";
        }

        public void SetFinish(string time)
        {
            finishGo.SetActive(true);
            finishTime.text = time;
        }

        public void SelectCurrentLevel()
        {
            LevelRunData.Instance.SelectedLevelIndex = _curLevel - 1;
            SceneManager.LoadScene(1);
        }
    }
}
