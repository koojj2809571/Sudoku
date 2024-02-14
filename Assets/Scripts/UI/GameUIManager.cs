using UnityEngine;
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
    }
}