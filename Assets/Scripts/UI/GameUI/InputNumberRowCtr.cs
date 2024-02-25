using UnityEngine;

// ReSharper disable once CheckNamespace
namespace UI
{
    public class InputNumberRowCtr : MonoBehaviour
    {
        public GameObject inputNumberPrefab;

        private void Start()
        {
            for (var i = 0; i < 9; i++)
            {
                var inputNum = Instantiate(inputNumberPrefab, transform);
                var button = inputNum.GetComponent<UINumberInputButton>();
                button.content = i + 1;
            }
        }
    }
}