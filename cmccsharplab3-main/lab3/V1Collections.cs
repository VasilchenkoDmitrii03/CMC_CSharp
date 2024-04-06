using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  System.Runtime.InteropServices;
using System.Text.Json;
using System.IO;
using System.Xml.Serialization;

namespace Lab_1
{
    public delegate DataItem FDI(double x);
    public delegate void FValues(double x, ref double y1, ref double y2);
    public class V1DataList : V1Data
    {
        public List<DataItem> Data { get; set; }
        public V1DataList(string key, DateTime dt) : base(key, dt)
        {
            Data = new List<DataItem>();
        }
        public V1DataList(string key, DateTime dt, double[] arr, FDI f) : base(key, dt)
        {
            Data = new List<DataItem>();
            foreach (double d in arr)
            {
                if (!contains(Data, d))
                {
                    Data.Add(f(d));
                }
            }
        }

        public override double MaxDistance{
            get
            {
                double min, max;
                min = max = (Data.Count > 0) ? Data[0].X : 0;
                foreach(DataItem item in Data)
                {
                    if(item.X < min) min  = item.X;
                    if(item.X > max) max = item.X;
                }
                return max - min;
            }
        }

        public override string ToString()
        {
            return $"Type:V1DataList, Base:{base.ToString()}, Count:{Data.Count}";
        }
        public override string ToLongString(string format)
        {
            string res = this.ToString() + "\nList:\n";
            foreach (DataItem item in Data)
            {
                res += $"x:{string.Format(format, item.X)}, y1:{string.Format(format, item.Y1)}, y2:{string.Format(format, item.Y2)}\n";
            }
            return res;
        }

        public static explicit operator V1DataArray(V1DataList source)
        {
            double[] X = new double[source.Data.Count];
            for(int i = 0;i < X.Length; i++)
            {
                X[i] = source.Data[i].X;
            }
            FValues tmp = (double x, ref double y1, ref double y2) =>
            {
                int ind = -1;
                for (int i = 0; i < X.Length; ++i)
                {
                    if (X[i] == x) { ind = i; break; }
                }
                y1 = source.Data[ind].Y1;
                y2 = source.Data[ind].Y2;
            };
            V1DataArray res = new V1DataArray(source.Key, source.DateTime, X, tmp);
            return res;
        }

        private bool contains(List<DataItem> list, double x)
        {
            foreach(DataItem item in list)
            {
                if (item.X == x) return true;
            }
            return false;
        }

        public override IEnumerator<DataItem> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

    }
   public  class V1DataArray : V1Data
    {
        public double[] DataX { get; set; }
        public double[][] DataY { get; set; }

        public V1DataArray() : base("", DateTime.Now)
        {
            DataX = new double[0];
            DataY = new double[0][];
        }
        public V1DataArray(string key, DateTime dt) :base(key, dt)
        {
            DataX = new double[0];
            DataY = new double[0][];
        }
        public V1DataArray(string key, DateTime date, double[] x, FValues F) : base(key, date)
        {
            DataX = new double[x.Length];
            DataY = new double[2][];
            DataY[0] = new double[x.Length];
            DataY[1] = new double[x.Length];
            x.CopyTo(DataX, 0);
            for(int i= 0;i < DataX.Length; ++i)
            {
                F(DataX[i], ref DataY[0][i], ref DataY[1][i]);
            }
        }
        public V1DataArray(string key, DateTime date, int nX, double xL, double xR, FValues F) : base(key, date)
        {
            double st = (xR - xL) / (nX-1);
            DataX = new double[nX];
            DataY = new double[2][];
            DataY[0] = new double[nX];
            DataY[1] = new double[nX];
            int ind = 0;
            for(double t = xL; ind < nX; t += st, ind++) 
            {
                if(ind == 9)
                {
                    int a = 10 + 3 * 2;
                }
                DataX[ind] = t;
                F(t, ref DataY[0][ind], ref DataY[1][ind]);
            }
        }

        public double[] this[int ind]
        {
            get
            {
                return DataY[ind];
            }
        }
        public override double MaxDistance
        {
            get
            {
                double max, min;
                min = max = (DataX.Length > 0) ? DataX[0] : 0;
                foreach(double t in DataX) 
                {
                    if (t < min) min = t;
                    if (t > max) max = t;
                }
                return max - min;
            }
        }
        public V1DataList V1DataList
        {
            get
            {
                V1DataList res = new V1DataList(this.Key, this.DateTime);
                for(int i= 0;i < DataX.Length; ++i)
                {
                    res.Data.Add(new DataItem(DataX[i], DataY[0][i], DataY[1][i]));
                }
                return res;
            }
        }

        public override IEnumerator<DataItem> GetEnumerator()
        {
            List<DataItem> res = new List<DataItem>();
            for(int i = 0;i < DataX.Length; ++i)
            {
                res.Add(new DataItem(DataX[i], DataY[0][i], DataY[1][i]));
            }
            return res.GetEnumerator();
        }

        public override string ToString()
        {
            return $"Type:V1DataArray, {base.ToString()}";
        }
        public override string ToLongString(string format)
        {
            string res =$"{ToString()}:\n";
            for (int i = 0; i < DataX.Length; i++)
            {
                res += $"x:{string.Format(format, DataX[i])}, y1:{string.Format(format, DataY[0][i])}, y2:{string.Format(format, DataY[1][i])}\n";
            }
            return res;
        }
        public int Count()
        {
            return DataX.Length;
        }

        public bool Save(string filename)
        {
            try
            {                
                using (StreamWriter fs = new StreamWriter(filename))
                {
                    fs.WriteLine(JsonSerializer.Serialize(DataX));
                    fs.WriteLine(JsonSerializer.Serialize(DataY));
                    fs.WriteLine(Key);
                    fs.WriteLine(DateTime);

                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static bool Save(string filename, V1DataArray arr)
        {
            try
            {
                
                using (StreamWriter fs = new StreamWriter(filename))
                {
                    fs.WriteLine(JsonSerializer.Serialize(arr.DataX));
                    fs.WriteLine(JsonSerializer.Serialize(arr.DataY));
                    fs.WriteLine(arr.Key);
                    fs.WriteLine(arr.DateTime);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static bool Load(string filename, ref V1DataArray arr)
        {
            try
            {
                string datax, datay, key, time;
                using (StreamReader fs = new StreamReader(filename))
                {
                    datax = fs.ReadLine();
                    datay = fs.ReadLine();
                    key = fs.ReadLine();
                    time = fs.ReadLine();
                }
                double[] dataxarr = JsonSerializer.Deserialize<double[]>(datax);
                double[][] datayarr = JsonSerializer.Deserialize<double[][]>(datay);

                arr = new V1DataArray(key, DateTime.Parse(time));
                arr.DataX = dataxarr;
                arr.DataY = datayarr;
            }
            catch
            {
                throw;
            }
            return true;
        }
    }
}