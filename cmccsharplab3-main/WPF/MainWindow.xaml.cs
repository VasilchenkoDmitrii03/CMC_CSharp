using Lab_1;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewData _data;
        SaveFileDialog _saveFileDialog;
        OpenFileDialog _openFileDialog;
        public MainWindow()
        {
            _data = new ViewData();
            this.DataContext = _data;
            _data.BuildDataArray();
            _data.BuildSplineData();
            InitializeComponent();
            _saveFileDialog = new SaveFileDialog();
            _saveFileDialog.InitialDirectory = @"C:\Dmitrii\Test";
            _openFileDialog = new OpenFileDialog();
            _openFileDialog.InitialDirectory = @"C:\Dmitrii\Test";
            _openFileDialog.Filter = "JSON|*.json";
        }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            _data.BuildDataArray();
            _data.BuildSplineData();
        }

        private void SplineDataListBox_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                DetailedInfoTextBlock.Text = _data.SplineArray[SplineDataListBox.SelectedIndex].ToTextedString("{0:0.00}");
            }
            catch { }
        }
        private void UpdateData(object sender, RoutedEventArgs e)
        {
            _data.BuildDataArray();
            _data.BuildSplineData();
        }
        private void SaveFile(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_saveFileDialog.ShowDialog() == true)
                {
                    _data.Save(_saveFileDialog.FileName+".json");
                }
            }
            catch
            {
                MessageBox.Show("Problems with saving file");
            }
            
        }
        private void OpenFile(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_openFileDialog.ShowDialog() == true)
                {
                    _data.Load(_openFileDialog.FileName);
                }
            }
            catch
            {
                MessageBox.Show("Problems with opening file");
            }
         }
    }
}