using System;
using System.Collections.Generic;
using System.Text;

namespace Programming.DataStructure
{
    public interface IDictionary<tkey, tvalue> : IEnumerable<KeyValuePair<tkey, tvalue>>
    {
        int Count { get; }
        tvalue this[tkey key] { get;set; }
        void Add(tkey key, tvalue value);
        void Remove(tkey key);
        void Clear();
    }
}
