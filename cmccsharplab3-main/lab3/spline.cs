using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Lab_1
{
    public struct SplineDataItem
    {
        public double X;
        public double[] Values;
        public SplineDataItem(double x, double value, double der_1, double der_2)
        {
            X = x;
            Values = new double[] { value, der_1, der_2 };
        }
        public string ToString(string format)
        {
            return string.Format(format, X) + " " + string.Format(format, Values[0])+" " + string.Format(format, Values[1])+" " + string.Format(format, Values[2]);
           // return string.Format(format, X) +  Values[0], Values[1], Values[2]);
        }
        public string ToTextedString(string format)
        {
            string res = $"X: {string.Format(format, X)}\nValue: {string.Format(format, Values[0])}\nFirstDerivative: {string.Format(format, Values[1])}\nSecond Derivative: {string.Format(format, Values[2])}";
            return res;
        }
        public override string ToString()
        {
            return string.Format("{0:f3}", X);
            return $"{X} {Values[0]} {Values[1]} {Values[2]}";
        }
    }
   public class SplineData
    {
        public V1DataArray BaseArr { get; set; }
        public double[] BoundDerValues { get; set; }
        public int PointsCount { get; set; }
        public List<SplineDataItem> SplineRes { get; set; }
        public double[] Bounds { get; set; }
        public double IntValue { get; set; }

        public SplineData()
        {
            BaseArr = new V1DataArray();
            Bounds = new double[2] { 0, 0 }; 
            PointsCount = 0;
            BoundDerValues =new double[2] { 0, 0 };
        }
        public SplineData(V1DataArray array, double[] boundValues, int count, double[] bounds)
        {
            BaseArr = array;
            BoundDerValues = boundValues;
            PointsCount = count;
            Bounds = bounds;
            SplineRes = new List<SplineDataItem>();
        }
        public string ToLongString(string format)
        {
            string res = BaseArr.ToLongString(format) + "\n";
            res += $"Derivative Bound: {BoundDerValues[0]} {BoundDerValues[1]}\n";
            res += $"Points Count: {PointsCount}\n";
            res += $"Bounds: {Bounds[0]} {Bounds[1]}";
            res += "Spline res:\n";
            const string tabsize = "{0,12:f5}";
            foreach (SplineDataItem item in SplineRes)
            {

                res += string.Format(tabsize, item.X);
                res += string.Format(tabsize, item.Values[0]);
                res += string.Format(tabsize, item.Values[1]);
                res += string.Format(tabsize, item.Values[2]) + "\n";
            }
            return res;
        }
        public void Interpolate()
        {
            // Узлы сплайна
            double xL = BaseArr.DataX.First(); // левый конец отрезка
            double xR = BaseArr.DataX.Last(); // правый конец отрезка
            int nX = BaseArr.DataX.Length; // число узлов сплайна
            double[] X = BaseArr.DataX; // массив узлов сплайна

            int nY = 1; // размерность векторной функции
            double[] Y = new double[nX * nY]; // массив заданных значений векторной функции
            for(int i =0;i <nY; i++)
            {
                for (int j = 0; j < nX; j++)
                {
                    Y[i * nX + j] = BaseArr.DataY[i][j];
                }
            }
            
                          
                
            double d1L = BoundDerValues[0]; // значение первой производной сплайна на левом конце
            double d1R = BoundDerValues[1]; // значение первой производной сплайна на правом конце
                            // Равномерная сетка, на которой вычисляются значения сплайна и производных
            int nS = PointsCount; // число узлов равномерной сетки
            double sL = Bounds[0]; // левый конец отрезка
            double sR = Bounds[1]; // правый конец отрезка
                            // Массив узлов на отрезке [sL, sR]
            double[] sites = new double[nS];
            double hS = (sR - sL) / (nS - 1); // шаг сетки
            sites[0] = sL;
            for (int j = 0; j < nS; j++) sites[j] = sites[0] + hS * j;
            double[] SplineValues = new double[6 * nS]; // массив вычисленных значений
                                                        // сплайна и его производных
            double limitL = Bounds[0]; // левый конец отрезка интегрирования
            double limitR = Bounds[1]; // правый конец отрезка интегрирования
            double[] integrals = new double[1]; // значение интеграла
            try
            {
                int ret = Lab3CubicSpline(
            nX, // число узлов сплайна
            X, // массив узлов сплайна
            nY, // размерность векторной функции
            Y, // массив заданных значений векторной функции
            d1L, // производная сплайна на левом конце
            d1R, // производная сплайна на правом конце
            nS, // число узлов равномерной сетки,на которой
                // вычисляются значения сплайна и его производных
            sL, // левый конец равномерной сетки
            sR, // правый конец равномерной сетки
            SplineValues, // массив вычисленных значений сплайна и производных
            limitL, // левый конец отрезка интегрирования
            limitR, // правый конец отрезка интегрирования
            integrals); // значение интеграла
             
            } 
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сплайн-интерполяции\n{ex}");
            }
            SplineRes.Clear();
            for (int i = 0; i < nS; ++i)
            {
                SplineRes.Add(new SplineDataItem(sites[i], SplineValues[3 * i], SplineValues[3 * i + 1], SplineValues[3 * i + 2]));
            }
            IntValue = integrals[0];
        }
        public bool Save(string filename, string format)
        {
            try
            {
                using (StreamWriter fs = new StreamWriter(filename))
                {
                    fs.Write(this.ToLongString(format));
                }
            }
            catch 
            {
                return false;
            }
            return true;
        }
        [DllImport("lab3_dll.dll",
        CallingConvention = CallingConvention.Cdecl)]
        public static extern int Lab3CubicSpline(int nX, double[] X, int nY, double[] Y, double d1L, double d1R,
int nS, double sL, double sR, double[] splineValues,
double limitL, double limitR, double[] integrals);



    }

}