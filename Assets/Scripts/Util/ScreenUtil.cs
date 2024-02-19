using UnityEngine;
using UnityEngine.UI;

namespace Util
{
    public enum ScreenAdapter
    {
        ByWidth, ByHeight
    }
    public class ScreenUtil : MonoBehaviour
    {
        public float designH;
        public float designW;
        public ScreenAdapter adapter;
        private CanvasScaler _scaler;

        private void Start()
        {
            _scaler = GetComponent<CanvasScaler>();
            var factor = adapter switch
            {
                ScreenAdapter.ByHeight => Screen.height / designH,
                ScreenAdapter.ByWidth => Screen.width / designW,
                _ => 1f
            };

            _scaler.scaleFactor = factor;
        }
    }
}