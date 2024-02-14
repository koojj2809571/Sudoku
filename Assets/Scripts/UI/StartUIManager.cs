using System;
using Util;

namespace UI
{
    public class StartUIManager : BaseSingleton<StartUIManager>
    {

        public LevelRunData LevelData => LevelRunData.Instance;
        
        private void Start()
        {
            
        }
    }
}