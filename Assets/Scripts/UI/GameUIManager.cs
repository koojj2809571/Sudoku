using TMPro;
using Util;

namespace UI
{
    public class GameUIManager : BaseSingleton<GameUIManager>
    {
        public FinishDialogCtr finishDialogCtr;
        public UIDialogCtr uiDialogCtr;
        public UIInfoCtr uiInfoCtr;
        public UIMiddleActionRowCtr uiMiddleActionRowCtr;
        public UITopActionRowCtr uiTopActionRowCtr;
        public TMP_Text levelName;

        // private void Start()
        // {
        //     UpdateGameName();
        // }

        private void Update()
        {
            UpdateGameName();
        }

        private void UpdateGameName()
        {
            var levelIndex = LevelRunData.Instance.SelectedGameIndex;
            levelName.text = levelIndex == -1 ? "Daily Challenge" : $"Level - {levelIndex + 1}";
        }
    }
}