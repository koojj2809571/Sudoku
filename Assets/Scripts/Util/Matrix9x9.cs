using System;
using System.Collections.Generic;
using System.Linq;

namespace Util
{
    enum TransformType
    {
        Row, BigRow, Col, BigCol, Transpose, NumberSwap
    }
    public class Matrix9X9
    {

        public readonly List<int> Src;
        private readonly Dictionary<string, List<int>> _rows = new();
        private readonly Dictionary<string, List<int>> _cols = new();
        private readonly Dictionary<string, int> _matrix = new();

        public Matrix9X9(List<int> src)
        {
            this.Src = src;
            AssignmentRowsBySrc();
            AssignmentColsBySrc();
            AssignmentMatrixBySrc();
        }
        
        private void AssignmentRowsBySrc()
        {
            for (var i = 0; i < 9; i++)
            {
                _rows[$"R{i}"] =  Src.GetRange(i * 9, 9);
            }
        }
        
        private void AssignmentColsBySrc()
        {
            for (var i = 0; i < 9; i++)
            {
                List<int> colTemp = new();
                for (var j = 0; j < 9; j++)
                {
                    colTemp.Add(Src[j * 9 + i]);
                }
                _cols[$"C{i}"] = colTemp;
            }
        }

        private void AssignmentMatrixBySrc()
        {
            for (var i = 0; i < 9; i++)
            {
                for (var j = i * 9; j < i * 9 + 9; j++)
                {
                    _matrix[$"R{i}C{j % 9}"] =  Src[j];
                }
            }
        }
        
        private void AssignmentSrcByRows()
        {
            Src.Clear();
            for (var i = 0; i < 9; i++)
            {
                Src.AddRange(_rows[$"R{i}"]);
            }
            
        }

        private void AssignmentSrcByMatrix()
        {
            Src.Clear();
            Src.AddRange(_matrix.Values);
        }
        
        private void AssignmentSrcByCols()
        {
            Src.Clear();
            
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    Src.Add(_cols[$"C{j}"][i]);
                }
            }
        }

        private List<int> GetBigIndexList(int bigIndex)
        {
            var result = new List<int>();
            switch (bigIndex)
            {
                case 0: 
                    result.AddRange(new []{0,1,2});
                    break;
                case 1: 
                    result.AddRange(new []{3,4,5});
                    break;
                case 2: 
                    result.AddRange(new []{6,7,8});
                    break;
            }

            return result;
        }

        public void Transpose()
        {
            var temp = new Dictionary<string, List<int>>();
            foreach (var row in _rows)
            {
                temp.Add(row.Key, row.Value);
            }

            for (var i = 0; i < 9; i++)
            {
                _rows[$"R{i}"] = _cols[$"C{i}"];
                _cols[$"C{i}"] = temp[$"R{i}"];
            }
            
            AssignmentSrcByRows();
            AssignmentMatrixBySrc();
            AssignmentColsBySrc();
        }

        public void SwapRow(Random random)
        {
            var bigRows = GetBigIndexList(random.Next(3));
            SwapRow(bigRows[0], bigRows[2]);
            SwapRow(bigRows[2], bigRows[1]);
        }
        public void SwapRow(int index, int anotherIndex)
        {
            if (index >= 9 || anotherIndex >= 9 || index < 0 || anotherIndex < 0 || Math.Abs(anotherIndex - index) >= 3)
            {
                DebugUtil.Log("Matrix9X9.SwapRow()方法参数异常");
                return;
            }
            var temp = _rows[$"R{index}"].ToList();

            _rows[$"R{index}"] = _rows[$"R{anotherIndex}"];
            _rows[$"R{anotherIndex}"] = temp;
            AssignmentSrcByRows();
            AssignmentColsBySrc();
            AssignmentMatrixBySrc();
        }
        
        public void SwapBigRow()
        {
            SwapBigRow(0, 2);
            SwapBigRow(2, 1);
        }
        public void SwapBigRow(int index, int anotherIndex)
        {
            if (index >= 3 || anotherIndex >= 3 || index < 0 || anotherIndex < 0)
            {
                DebugUtil.Log("Matrix9X9.SwapBigRow()方法参数异常");
                return;
            }

            var indexList = GetBigIndexList(index);
            var anotherIndexList = GetBigIndexList(anotherIndex);

            var temp = indexList.Select(i => _rows[$"R{i}"].ToList()).ToList();

            for (var i = 0; i < 3; i++)
            {
                _rows[$"R{indexList[i]}"] = _rows[$"R{anotherIndexList[i]}"];
                _rows[$"R{anotherIndexList[i]}"] = temp[i];
            }
            
            
            AssignmentSrcByRows();
            AssignmentColsBySrc();
            AssignmentMatrixBySrc();
        }
        
        public void SwapCol(Random random)
        {
            var bigCols = GetBigIndexList(random.Next(3));
            SwapCol(bigCols[0], bigCols[2]);
            SwapCol(bigCols[2], bigCols[1]);
        }
        public void SwapCol(int index, int anotherIndex)
        {
            if (index >= 9 || anotherIndex >= 9 || index < 0 || anotherIndex < 0 || Math.Abs(anotherIndex - index) >= 3)
            {
                DebugUtil.Log("Matrix9X9.SwapCol()方法参数异常");
                return;
            }
            var temp = _cols[$"C{index}"].ToList();
            _cols[$"C{index}"] = _cols[$"C{anotherIndex}"];
            _cols[$"C{anotherIndex}"] = temp;
            AssignmentSrcByCols();
            AssignmentRowsBySrc();
            AssignmentMatrixBySrc();
        }
        
        public void SwapBigCol()
        {
            SwapBigCol(0, 2);
            SwapBigCol(2, 1);
        }
        public void SwapBigCol(int index, int anotherIndex)
        {
            if (index >= 3 || anotherIndex >= 3 || index < 0 || anotherIndex < 0)
            {
                DebugUtil.Log("Matrix9X9.SwapBigCol()方法参数异常");
                return;
            }
            var indexList = GetBigIndexList(index);
            var anotherIndexList = GetBigIndexList(anotherIndex);

            var temp = indexList.Select(i => _cols[$"C{i}"].ToList()).ToList();

            for (var i = 0; i < 3; i++)
            {
                _cols[$"C{indexList[i]}"] = _cols[$"C{anotherIndexList[i]}"];
                _cols[$"C{anotherIndexList[i]}"] = temp[i];
            }
            AssignmentSrcByCols();
            AssignmentRowsBySrc();
            AssignmentMatrixBySrc();
        }

        public void SwapNumber(Random random)
        {
            var num1 = random.Next(1,10);
            var num2 = random.Next(1,10);
            var num3 = random.Next(1,10);
            var num4 = random.Next(1,10);
            SwapNumber(num1, num2);
            SwapNumber(num3, num4);
        }
        public void SwapNumber(int num, int anotherNum)
        {
            if (num > 9 || anotherNum > 9 || num < 1 || anotherNum < 1)
            {
                DebugUtil.Log("Matrix9X9.SwapNumber()方法参数异常");
                return;
            }

            var numKeys = new List<string>();
            var anotherNumKeys = new List<string>();
            foreach (var kv in _matrix)
            {
                if (kv.Value == num)
                {
                    numKeys.Add(kv.Key);
                }

                if (kv.Value == anotherNum)
                {
                    anotherNumKeys.Add(kv.Key);
                }
            }

            for (var i = 0; i < numKeys.Count; i++)
            {
                _matrix[numKeys[i]] = anotherNum;
                _matrix[anotherNumKeys[i]] = num;
            }
            
            AssignmentSrcByMatrix();
            AssignmentRowsBySrc();
            AssignmentColsBySrc();
        }

        public void RandomTransform(Random random, int transTimes)
        {
            if (transTimes < 1 || transTimes > Enum.GetValues(typeof(TransformType)).Length)
            {
                DebugUtil.Log("Matrix9X9.RandomTransform()方法参数异常");
                return;
            }

            if (transTimes == 1)
            {
                Transpose();
            }
            if (transTimes == 2)
            {
                SwapRow(random);
            }
            if (transTimes == 3)
            {
                SwapBigRow();
            }
            if (transTimes == 4)
            {
                SwapCol(random);
            }
            if (transTimes == 5)
            {
                SwapBigCol();
            }
            if (transTimes == 6)
            {
                SwapNumber(random);
            }
        }

        public override string ToString()
        {
            var result = "\nMatrix9X9 Log\nRows:\n";
            foreach (var row in _rows.Values)
            {
                result += $"\n{row.LogStr()}\n";
            }

            result += "\nColumns:\n";

            foreach (var col in _cols.Values)
            {
                result += $"\n{col.LogStr()}\n";
            }
            
            result += "\nMatrix:\n";

            result += _matrix.LogStr();
            
            result += "\nSrc:\n";

            result += Src.LogStr();
            
            return result;
        }
    }
}