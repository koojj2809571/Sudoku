using UnityEngine;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
namespace UI
{
    [ExecuteAlways]
    public class HandleButtonCtr : MonoBehaviour
    {

        public GameObject goBadge;
        public bool showBadge;
        public Image btImage;
        public Sprite btIcon;
        
        private void Start()
        {
            btImage.sprite = btIcon;
            goBadge.SetActive(showBadge);
        }
    }
}