using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WPF.DataBinding
{
    class ArrayConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                double[] arr = (double[])value;
                if (arr == null) return "";
                string res = "";
                for(int i = 0; i < arr.Length; i++)
                {
                    res += arr[i];
                    if (i != arr.Length - 1) res += ",";
                }
                return res;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wrong array input");
            }
            return "";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double[] result;
            try
            {
                string tmp = (string)value;
                string[] strings = tmp.Split(new char[] { ' ', ',' });
                result = new double[strings.Length];
                for (int i = 0; i < strings.Length; i++)
                {
                    result[i] = System.Convert.ToDouble(strings[i]);
                }
                return result;
            }
            catch
            {
                MessageBox.Show("Wrong array input");
            }
            return null;
        }
    }
}
