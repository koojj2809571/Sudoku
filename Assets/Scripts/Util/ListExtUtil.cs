using System.Collections.Generic;
using System.Linq;

namespace Util
{
    public static class ListExtUtil
    {
        public static string  LogStr<T>(this List<T> list)  
        {  
            return $"[{string.Join(',',list)}]";  
        }  
        
        public static string  LogStr<T,TE>(this Dictionary<T,TE> dict)  
        {  
            return $"{{{string.Join(',',dict)}}}";  
        }
    }
}