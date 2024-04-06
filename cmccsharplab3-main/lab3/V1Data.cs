using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1
{
   public  abstract class V1Data : IEnumerable<DataItem>
    {
        public string Key { get; set; }
        public DateTime DateTime { get; set; }
        public V1Data(string key, DateTime dt) 
        {
            Key = key;
            DateTime = dt;
        }

        public abstract IEnumerator<DataItem> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public abstract double MaxDistance { get; }
        public abstract string ToLongString(string format);
        public override string ToString()
        {
            return $"Key: {Key} DataItem: {DateTime.ToString()}";
        }
    }
    
}
