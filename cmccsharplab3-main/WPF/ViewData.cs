using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Lab_1;
using OxyPlot;
namespace WPF
{
    class ViewData : INotifyPropertyChanged, IDataErrorInfo
    {
        public V1DataArray _array;
        public SplineData _spline;
        Random _random = new Random();
        public Plot plotModel { get; set; }

        public ViewData()
        {

            FillFunctions();
            _array = new V1DataArray("v1dataarray", DateTime.Now, 10, 0.0, 1.0,
              SelectedFunction.function);
            _spline = new SplineData();
            _spline = new SplineData(_array, new double[] { 0, 2.43 }, 19, new double[] { _array.DataY[1][0], _array.DataY[1][_array.Count() - 1] });
            Bounds = new double[2] { 0, 1.0 };
            PointsCount = 10;
            UniformGrid = true;
            Function = "Linear";
            LeftDer = 0;
            RightDer = 2.43;
            SplinePointsCount = 19;
            BuildDataArray();
            BuildSplineData();
        }

        public double[] Bounds
        {
            get;set;
        }
        public int PointsCount
        {
            get;set;
        }
        public bool UniformGrid
        {
            get; set;
        }
        public string Function
        {
            get;
            set;
        }
        public List<DataItem> DataArray
        {
            get;set;
        }
        public List<SplineDataItem> SplineArray
        {
            get;set;
        }
        public double LeftDer
        {
            get;set;
        }
        public double RightDer
        {
            get; set;
        }
        public int SplinePointsCount
        {
            get
            {
                return _spline.PointsCount;
            }
            set
            {
                _spline.PointsCount = value;
                OnPropertyChanged("SplinePointsCount");
            }
        }
        
        public List<Function> Functions { get; set; }
        public Function SelectedFunction { get; set; }

        public string Error { get { throw new FormatException(); } }

        public string this[string prop] {
            get
            {
                string msg = null;
                switch (prop)
                {
                    case "PointsCount":
                        if (PointsCount < 3) msg = "There are must be not less then 3 points in data";
                        break;
                    case "SplinePointsCount":
                        if (SplinePointsCount < 3) msg = "There are must be not less then 3 points in spline";
                        break;
                    case "Bounds":
                        if (Bounds != null && Bounds[0] >= Bounds[1]) msg = "Left bound must be less then right bound";
                        break;
                    default:
                        break;
                }
                return msg;
            }
        }

        private void FillFunctions()
        {
            Functions = new List<Function>();
            Functions.Add(new Function((double x, ref double y1, ref double y2) => { y1 = x; y2 = x; }, "linear"));
            Functions.Add(new Function((double x, ref double y1, ref double y2) => { y1 = x * x; y2 = x; }, "square"));
            Functions.Add(new Function((double x, ref double y1, ref double y2) => { y1 = x * x * x; y2 = x; }, "cubic"));
            Functions.Add(new Function((double x, ref double y1, ref double y2) => { y1 = Math.Sin(x); y2 = x; }, "sinus"));
            SelectedFunction = Functions[0];
            OnPropertyChanged("SelectedFunction");
            OnPropertyChanged("Functions");
        }

        public void BuildDataArray()
        {
            if (UniformGrid)
            {
                _array = new V1DataArray("key", DateTime.Now, PointsCount, Bounds[0], Bounds[1], SelectedFunction.function);
                DataArray = _array.ToList();
            }
            else
            {
                _array = new V1DataArray("key", DateTime.Now, GenerateRandomGrid(Bounds[0], Bounds[1], PointsCount), SelectedFunction.function);
                DataArray = _array.ToList();
            }
            OnPropertyChanged("DataArray");
            OnPropertyChanged("PointsCount");
            OnPropertyChanged("Bounds");
        }
        public void BuildSplineData()
        {
            _spline = new SplineData(_array, new double[] { LeftDer, RightDer }, SplinePointsCount, new double[] { _array.DataY[1][0], _array.DataY[1].Last() });
            _spline.Interpolate();
            SplineArray = _spline.SplineRes;
            OnPropertyChanged("SplineArray");
        }

        private double[] GenerateRandomGrid(double leftBound, double rightBound, int PointCount)
        {
            double[] res = new double[PointCount];
            res[0] = leftBound;
            res[res.Length - 1] = rightBound;
            double step = (rightBound - leftBound) / (PointCount-1);
            for(int i = 1; i < PointCount-1; i++)
            {
                res[i] = (i * step) + _random.NextDouble()* (step);
            }
            return res;
        }

        public void Save(string filename)
        {
            try
            {
                _array.Save(filename);
            }
           catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Load(string filename)
        {
            try
            {
                
                V1DataArray.Load(filename, ref _array);
                PointsCount = _array.DataX.Length;
                Bounds[0] = _array.DataX[0];
                Bounds[1] = _array.DataX.Last();
                OnPropertyChanged("PointsCount");
                OnPropertyChanged("Bounds");
                BuildDataArray();
                BuildSplineData();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public string tmp()
        {
            return $"{Bounds[0]}, {Bounds[1]}, {PointsCount}, {UniformGrid}, {Function}";
        }
    }
}
