using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Counter_Control.Views;

namespace Counter_Control
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // ===== EVENTS ===== //
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        // ===== BUTTONS ===== //

        private void btnCounters_Click(object sender, RoutedEventArgs e)
        {
            userControl.Content = new MeterManagement();
        }

        private void btnReadings_Click(object sender, RoutedEventArgs e)
        {
            userControl.Content = new ReadoutsManagement();
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            userControl.Content = new MeterReports();
        }
    }
}
