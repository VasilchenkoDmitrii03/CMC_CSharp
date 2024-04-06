using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using Lab_1;
using OxyPlot;
using OxyPlot.Legends;
using OxyPlot.Series;
namespace WPF
{
    internal class Plot
    {
        SplineData _data { get; set; }
        public PlotModel plotModel { get; private set; }
        public Plot(SplineData data)
        {
            _data = data;
            plotModel = new PlotModel() { Title = "Approximation" };
            OxyPlot.Axes.LinearAxis XAxis = new OxyPlot.Axes.LinearAxis { Position = OxyPlot.Axes.AxisPosition.Bottom };
            OxyPlot.Axes.LinearAxis YAxis = new OxyPlot.Axes.LinearAxis { Position = OxyPlot.Axes.AxisPosition.Left };
            XAxis.Title = "X";
            YAxis.Title = "Y";
            plotModel.Axes.Add(YAxis);
            plotModel.Axes.Add(XAxis);
            LineSeries lines1 = new OxyPlot.Series.LineSeries()
            {
                Title = $"Pointed function values",
                Color = OxyColors.Transparent,
                MarkerSize = 6,
                MarkerType = OxyPlot.MarkerType.Star,
                MarkerFill = OxyColors.Brown,
                MarkerStroke = OxyColors.Brown
            };
            for (int i = 0; i < _data.BaseArr.DataX.Length; i++)
            {
                lines1.Points.Add(new DataPoint(_data.BaseArr.DataX[i], _data.BaseArr[0][i]));
            }

            Legend leg = new Legend() { LegendPosition = LegendPosition.RightTop, LegendPlacement = LegendPlacement.Inside };
            plotModel.Legends.Add(leg);

            LineSeries lines2 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Blue,
                Title = "Spline",
                MarkerSize = 3,
                MarkerType = OxyPlot.MarkerType.Star,
                MarkerFill = OxyColors.Blue,
                MarkerStroke = OxyColors.Blue
            };

            foreach(SplineDataItem s in _data.SplineRes)
            {
                lines2.Points.Add(new DataPoint(s.X, s.Values[0]));
            }
            plotModel.Series.Add(lines2);
            leg = new Legend { LegendPosition = LegendPosition.LeftTop, LegendPlacement = LegendPlacement.Inside };
            plotModel.Legends.Add(leg);
            plotModel.Series.Add(lines1);
        }
    }
}
