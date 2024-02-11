using System;
using Util;

namespace UI
{
    public class UIManager : BaseSingleton<UIManager>
    {

        public LevelRunData LevelData => LevelRunData.Instance;
        
        private void Start()
        {
            
        }
    }
}