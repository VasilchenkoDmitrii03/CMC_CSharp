using System;

namespace Lab_1
{
    using Lab_1;
    using System.Runtime.InteropServices;
    internal class Program
    {
        static void Main(string[] args)
        {

            V1DataArray a1 = new V1DataArray("v1dataarray", DateTime.Now, 10, 0.0, 1.0,
                (double x, ref double y1, ref double y2) => { y1 = x*x*x; y2 = x; });

            SplineData s1 = new SplineData(a1, new double[] { 0, 3 },  20, new double[] { 0, 1 }) ;
            s1.Interpolate();
           Console.WriteLine(s1.ToLongString("{0:f5}"));
            //Console.WriteLine($"Interpolated integral: {s1.IntValue}");
            //Console.WriteLine("Self calculated integral: 0.164025");
            s1.Save(@"C:\Dmitrii\Test\tmp.txt", "{0:f5}");
        }
           
    }
}