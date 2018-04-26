using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.DataStructures
{
    /// <summary>
    /// Eigene Implementierung einer PriorityQueue, da es diese in .NET von Haus aus nicht gibt.
    /// </summary>
    public class PriorityQueue<TPriority, TValue> where TPriority : IComparable<TPriority>
    {

        private SortedList<TPriority, TValue> _list;
        private Dictionary<TValue, KeyValuePair<TPriority, TValue>> _accessDict;


        public PriorityQueue(int capacity = -1)
        {
            if (capacity > 0)
            {
                _list = new SortedList<TPriority, TValue>(capacity);
                _accessDict = new Dictionary<TValue, KeyValuePair<TPriority, TValue>>(capacity);
            }
            else
            {
                _list = new SortedList<TPriority, TValue>();
                _accessDict = new Dictionary<TValue, KeyValuePair<TPriority, TValue>>();
            }
        }



        public void Enqueue(TValue value, TPriority priority)
        {
            _list.Add(priority, value);
            _accessDict.Add(value, new KeyValuePair<TPriority, TValue>(priority, value));
        }


        public TValue Dequeue()
        {
            var firstPair = _list.First();
            _list.Remove(firstPair.Key);
            _accessDict.Remove(firstPair.Value);
            return firstPair.Value;
        }


        public void UpdatePriority(TValue node, TPriority newPriority)
        {
            var pair = _accessDict[node];
        }

    }
}
