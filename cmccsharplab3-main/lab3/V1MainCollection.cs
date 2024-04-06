using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1
{
    public class V1MainCollection : System.Collections.ObjectModel.ObservableCollection<V1Data>
    {
        public bool Contains(string key)
        {
            for (int i = 0; i < this.Count; ++i)
            {
                if (this[i].Key == key) return true;
            }
            return false;
        }
        public new bool Add(V1Data v1Data)
        {
            if (!this.Contains(v1Data.Key))
            {
                base.Add(v1Data);
                return true;
            }
            return false;
        }
        public V1MainCollection(int nV1DataArray, int nV1DataList)
        {

            double[] xvalues = new double[] { -1, 0,  1 };//new double[] { -1, 0, 0.1, 0.3, 0.6, 1 };
            for (int i = 0; i < nV1DataList; i++)
            {
                double[] xv = new double[xvalues.Length];
                for (int j = 0; j < nV1DataList; j++) xv[j] = /*(i + 1) * */ xvalues[j];
                FDI tmp = (double x) =>
                {
                    return new DataItem(x, 2 * x, 0);
                };
                this.Add(new V1DataList($"List{i}", DateTime.Now, xv, tmp));
            }
            for (int i = 0; i < nV1DataArray; i++)
            {
                double[] xv = new double[xvalues.Length];
                for (int j = 0; j < nV1DataList; j++) xv[j] = (i + 1) * xvalues[j];
                FValues tmp = (double x, ref double y1, ref double y2) =>
                {
                    y1 = 0;
                    y2 = 0;
                };
                this.Add(new V1DataArray($"Array{i}", DateTime.Now, xv, tmp));
            }
        }

        public double Average
        {
            get
            {
                if (this.Count == 0) return double.NaN;
                int n = this.Count;
                double sum = this.Sum(d => d.Sum(e => e.Abs()));
                return sum / n;
            }
        }
        public DataItem? MaxDif
        {
            get
            {
                if (this.Count == 0) return null;
                double Average = this.Average;
                IEnumerable<V1Data> NotEmpty = from coll in this
                                            where coll.Count() > 0
                                            select coll;
                double MaxDif = NotEmpty.Max(d => d.Max(e => e.Abs() - Average));
                DataItem res = (from coll in this
                                where coll.Count() > 0
                                from item in coll
                                where item.Abs() == MaxDif + Average
                                select item).First(); // Находим коллекцию,в которой есть нужный элемент, а затем достаем эту коллекцию и в ней этот элемент
                return res;
            }
        }
        public IEnumerable<DataItem> MaxDifs
        {
            get
            {
                if (this.Count == 0) return null;
                double Average = this.Average;
                IEnumerable<V1Data> NotEmpty = from coll in this
                                               where coll.Count() > 0
                                               select coll;
                double MaxDif = NotEmpty.Max(d => d.Max(e => e.Abs() - Average));
                IEnumerable<DataItem> res = (from coll in this
                                where coll.Count() > 0
                                from item in coll
                                where item.Abs() == MaxDif + Average
                                select item); // Находим коллекцию,в которой есть нужный элемент, а затем достаем эту коллекцию и в ней этот элемент
                return res;
            }
        }
        public IEnumerable<double> Coords{
            get
            {
                if (this.Count == 0) return null;
                IEnumerable<double> X = from coll in this
                                        where coll.Count() > 0
                                        from item in coll
                                        select item.X;
                IEnumerable<double> SortedX = from x in X
                                              orderby x
                                              select x;
                IEnumerable<double> query = SortedX.GroupBy(x => x)
                            .Where(g => g.Count() > 1)
                            .Select(y => y.Key)
                            .ToList();
                return query;
            }
       }
        private void print(IEnumerable<double> d, string msg = "")
        {
            Console.WriteLine(msg);
            foreach(double i in d)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();
        }
        public string ToLongString(string format)
        {
            string res = "";
            foreach (V1Data elem in this)
            {
                res += $"{elem.ToLongString(format)}\n";
            }
            return res;
        }
        public override string ToString()
        {
            string res = "";
            foreach (V1Data elem in this)
            {
                res += $"{elem.ToString()}\n";
            }
            return res;
        }
    }
}
