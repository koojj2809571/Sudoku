using System;
using Game.Config;
using Game.Item;
using Game.RunData;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
namespace UI
{
    public class GameBoardGenerator : MonoBehaviour
    {
        public int size;
        public float areRowSpacing;
        public GameObject prefabRow;
        public GameObject prefabArea;
        public GameObject prefabSquare;

        private NumberDataCtr Data => NumberRunData.Instance.dataCtr;

        private void Start()
        {
            GenerateBoard();
            Data.GenerateGame();
        }

        private void GenerateBoard()
        {
            for (var i = 0; i < size; i++)
            {
                var areaRow = Instantiate(prefabRow,transform);
                areaRow.name = $"Row{i + 1}";
                areaRow.tag = "RootRow";
                var hLayout = areaRow.GetComponent<HorizontalLayoutGroup>();
                hLayout.spacing = areRowSpacing;
                GenerateArea(i, areaRow.transform);
            }
        }

        private void GenerateArea(int outerRowIndex, Transform parent)
        {
            for (var i = 0; i < size; i++)
            {
                var area = Instantiate(prefabArea,parent);
                area.name = $"Area{i + 1}";
                area.tag = "Area";
                GenerateInnerRow(outerRowIndex * size + i, area.transform);
            }
        }
        
        private void GenerateInnerRow(int areaIndex, Transform parent)
        {
            for (var i = 0; i < size; i++)
            {
                var innerRow = Instantiate(prefabRow,parent);
                innerRow.name = $"Row{i + 1}";
                innerRow.tag = "SubRow";
                GenerateSquare(i, areaIndex, innerRow.transform);
            }
        }

        private void GenerateSquare(int innerRowIndex, int areaIndex, Transform parent)
        {
            var area = areaIndex + 1;
            var rowIndex = (int)Math.Floor(areaIndex / (float)size) * size + innerRowIndex;
            var row = rowIndex + 1;
            for (var i = 0; i < size; i++)
            {
                var column = areaIndex % size * size + i + 1;
                var square = Instantiate(prefabSquare,parent);
                var item = square.GetComponent<NumberItem>();
                item.Init(area, row, column, Data.numberData.Count);
                Data.numberData.Add(item);
                Data.NumDict.Add(item.ItemKey, item);
                square.name = $"Square{i + 1}";
                square.tag = "SubItem";
            }
        }
    }
}