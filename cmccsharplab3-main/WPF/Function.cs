using Lab_1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF
{
    internal class Function
    {
        FValues _func;
        string _name;
        public Function() { }
        public Function(FValues func, string name) 
        {
            _name = name;
            _func = func;
        }
        public void call(double x, ref double y1, ref double y2)
        {
            _func(x, ref y1, ref y2);
        }
        public FValues function
        {
            get { return _func; }
        }
        public override string ToString()
        {
            return _name;
        }
    }
}
