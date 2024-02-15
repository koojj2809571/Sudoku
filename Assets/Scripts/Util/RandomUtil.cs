using System;
using System.Collections.Generic;
using Game.Item;
using Game.RunData;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Util
{
    public class RandomUtil
    {
        private List<NumberItem> NumberData => NumberRunData.Instance.dataCtr.numberData;

        public void RandomOriginNumbers(ref int count, Action onFail, Action<int> onSuccess)
        {
            while (true)
            {
                var areas = NumDataUtil.GetDataByRegion(Region.Area);
                var generateFail = false;
                for (var i = 1; i <= 9; i++)
                {
                    var usedRow = new List<int>();
                    var usedCol = new List<int>();
                    foreach (var area in areas)
                    {
                        var random = GetRandomItemIndex(area, usedRow, usedCol);
                        if (random == null)
                        {
                            generateFail = true;
                            break;
                        }
                        usedRow.Add(random.row);
                        usedCol.Add(random.column);
                        random.Value = i;
                        random.editAble = false;
                    }

                    if (generateFail) break;
                }

                if (generateFail)
                {
                    onFail();
                    count++;
                    continue;
                }
                
                onSuccess(count);
                break;
            }
        }

        [CanBeNull]
        private NumberItem GetRandomItemIndex(List<NumberItem> area, List<int> row, List<int> col)
        {
            var excludeHasValue = area.FindAll(e => e.value == 0);
            var excludeSameRow = excludeHasValue.FindAll(e => !row.Contains(e.row));
            var excludeSameCol = excludeSameRow.FindAll(e => !col.Contains(e.column));
            var random = Random.Range(0, excludeSameCol.Count);
            return excludeSameCol.Count == 0 ? null : excludeSameCol[random];
        }

        public void RandomEmpty(int seed, int levelCount)
        {
            var nextRandom = new System.Random(seed);
            var indexSet = new HashSet<int>();
            while (indexSet.Count < levelCount)
            {
                var index = nextRandom.Next(80);
                indexSet.Add(index);
                NumberData[index].Value = 0;
                NumberData[index].editAble = true;
            }
        }

        public List<int> FindColIndexList(int colIndex)
        {
            var result = new List<int>();
            var matrix4X4 = new Matrix4x4();
            var transpose = matrix4X4.transpose;
            return result;
        }
    }
}