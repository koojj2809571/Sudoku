using System;
using UnityEngine;

namespace Game.Config
{
    [Serializable]
    public class ColorConfig
    {
        public Color originBgColor;
        public Color fixTextColor;
        public Color variableTextColor;
        public Color selectedColor;
        public Color relationColor;
        public Color sameColor;
        public Color errorColor;
        public Color finishItemGradientColor;
    }
}