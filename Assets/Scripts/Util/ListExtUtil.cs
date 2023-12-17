using System.Collections.Generic;
using System.Linq;

namespace Util
{
    public static class ListExtUtil
    {
        public static  string  LogStr<T>(this List<T> list)  
        {  
            return $"[{string.Join(',',list)}]";  
        }  
    }
}