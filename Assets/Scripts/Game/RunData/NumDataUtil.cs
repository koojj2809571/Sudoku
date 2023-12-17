using System.Collections.Generic;
using System.Linq;
using Game.Item;

namespace Game.RunData
{
    public enum Region
    {
        Row,Column,Area
    }
    public static class NumDataUtil
    {
        public static List<List<NumberItem>> GetDataByRegion(Region region)
        {
            var data = NumberRunData.Instance;
            var result = new List<List<NumberItem>>();
            for (var i = 0; i < 9; i++)
            {
                result.Add(new List<NumberItem>());
                result[i].AddRange(data.numberData.FindAll(e =>
                {
                    var temp = region switch
                    {
                        Region.Row => e.row == i + 1,
                        Region.Column => e.column == i + 1,
                        Region.Area => e.area == i + 1,
                        _ => false
                    };
                    return temp;
                }));
            }
            return result;
        }
        
        public static bool UsedInRegion(Region region, int regionIndex, int value)
        {
            var list = GetDataByRegion(region)[regionIndex - 1];
            var result = list.Count(item => item.value == value);
            return result > 1;
        }

    }
}