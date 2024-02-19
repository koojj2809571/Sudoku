using System.Linq;
using DG.Tweening;
using UI.LevelUI;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace UI
{
    public class LevelDialogCtr : MonoBehaviour
    {

        private RectTransform _rectTrans;
        public GameObject levelItem;
        public GameObject levelParent;
        private static float SW => 1440;
        private static float SH => 2960;
        
        private void Start()
        {
            _rectTrans = GetComponent<RectTransform>();
            _rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, SW - 60);
            _rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, SH - 60);
            var anchoredPos = _rectTrans.anchoredPosition3D;
            anchoredPos.y = -SH;
            _rectTrans.anchoredPosition3D = anchoredPos;
            InitLevelData();
        }
        
        public void DialogSwitcher(bool open)
        {
            _rectTrans.DOAnchorPos(new Vector2(0, open ? -30 : -SH), 0.5f, true);
        }

        private void InitLevelData()
        {
            var levelData = LevelRunData.Instance;
            for (var i = 0; i < levelData.GameSeed.Count; i++)
            {
                var itemGo = Instantiate(levelItem, levelParent.transform, true);
                var item = itemGo.GetComponent<LevelItem>();
                var rectTrans = itemGo.GetComponent<RectTransform>();
                var pos = rectTrans.anchoredPosition3D;
                pos.z = 0;
                rectTrans.anchoredPosition3D = pos;
                rectTrans.localScale = Vector3.one;
                item.SetLevel(i + 1);
                if (levelData.GameResult.Keys.Contains(i))
                {
                    item.SetFinish(levelData.GameResult[i]);
                }
                if (transform == null) continue;
                rectTrans.localScale = Vector3.one;
            }
        }
    }
}
