using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1
{
    public struct DataItem
    {
        public double X { get; set; }
        public double Y1 { get; set; }
        public double Y2 { get; set; }

        public DataItem(double x=0, double y1=0, double y2=0)
        {
            X = x;
            Y1 = y1;
            Y2 = y2;
        }
        public double Abs()
        {
            return Math.Sqrt(Math.Pow(Y1, 2) + Math.Pow(Y2, 2));
        }
        public string ToLongString(string format)
        {
            //return string.Format(format, X, Y1, Y2);
            return string.Format(format, X) + " " + string.Format(format, Y1) + " " + string.Format(format, Y2);
        }
        public override string ToString()
        {
            return ToLongString("{0:0.00}");
            return $"X:{X}, Y1:{Y1}, Y2:{Y2}";
        }
        public string VisualisationView
        {
            get
            {
                string format = "{0:0.000}" ;
                return string.Format(format, X) + " " + string.Format(format, Y1);
            }
        }
    }
}
