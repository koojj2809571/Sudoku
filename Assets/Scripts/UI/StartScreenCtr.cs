using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class StartScreenCtr : MonoBehaviour
    {

        public void ClickPlay()
        {
            SceneManager.LoadScene(1);
        }
    }
}
