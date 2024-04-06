using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPF
{
    public static class Command
    {
        public static RoutedCommand ExecuteCommand = new RoutedCommand("ExecuteCommand", typeof(WPF.Command));
    }
}