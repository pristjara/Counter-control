using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
using Counter_Control.Class;
using Counter_Control.Model;
using LiveCharts;
using LiveCharts.Wpf;

namespace Counter_Control.Views
{
    /// <summary>
    /// Interaction logic for MeterReports.xaml
    /// </summary>
    public partial class MeterReports : UserControl, INotifyPropertyChanged
    {
        public MeterReports()
        {
            InitializeComponent();

            FillCombobox();

            DataContext = this;
        }


        // ========== GLOBAL VAIRABLES ========== //


        // GRAPH VARIABLES

        public SeriesCollection _seriesCollection;
        public SeriesCollection seriesCollection
        {
            get { return _seriesCollection; }
            set
            {
                _seriesCollection = value;
                OnPropertyChanged("seriesCollection");
            }
        }

        public string[] _Labels;
        public string[] Labels
        {
            get { return _Labels; }
            set
            {
                _Labels = value;
                OnPropertyChanged("Labels");
            }
        }

        public Func<double, string> Formatter { get; set; }

        // LISTS
        private List<tbl_Meters> listMeters;
        private List<tbl_Readouts> listReadouts;
        private List<ReadoutReport> readoutReports = new List<ReadoutReport>();


        // ========== EVENTS ========== //


        private void GroupBox_Loaded(object sender, RoutedEventArgs e)
        {
            // graph or table
            SelectReadoutType();
        }

        private void ShowReadouts(object sender, RoutedEventArgs e)
        {
            // graph or table
            SelectReadoutType();
        }

        private void LoadOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // when combobox or datepicker value changes update info in the table and in the graph
            LoadIntoTable();
            CreateGraph();
        }


        // ========== FUNCTIONS ========== //


        public void SelectReadoutType()
        {
            if (rbTable.IsChecked == true)
            {
                LoadIntoTable();
                grdTable.Visibility = Visibility.Visible;
                grdGraph.Visibility = Visibility.Collapsed;
            }
            if (rbGraph.IsChecked == true)
            {
                CreateGraph();
                grdTable.Visibility = Visibility.Collapsed;
                grdGraph.Visibility = Visibility.Visible;
            }
        }

        public void CreateGraph()
        {
            /*
             * Func that creates graph:
             * Gather consumption rates from the list
             * Add items to the graph based on the combobox selection
             */ 

            ChartValues<double> GasValues = new ChartValues<double>();
            ChartValues<double> ElectroValues = new ChartValues<double>();
            ChartValues<double> HotWaterValues = new ChartValues<double>();
            ChartValues<double> ColdWaterValues = new ChartValues<double>();

            foreach (var item in readoutReports)
            {
                switch (item.METER_TYPE_REPORT)
                {
                    case "GAS":
                        GasValues.Add(item.READOUT_CONSUPMTION_REPORT);
                        break;
                    case "ELECTRICITY":
                        ElectroValues.Add(item.READOUT_CONSUPMTION_REPORT);
                        break;
                    case "HOT WATER":
                        HotWaterValues.Add(item.READOUT_CONSUPMTION_REPORT);
                        break;
                    case "COLD WATER":
                        ColdWaterValues.Add(item.READOUT_CONSUPMTION_REPORT);
                        break;
                    default:
                        break;
                }
            }

            seriesCollection = new SeriesCollection();

            seriesCollection.Clear();

            if (cmbMeterType.SelectedItem.ToString().Contains("---") || cmbMeterType.SelectedItem.ToString().Contains("GAS"))
            {
                ColumnSeries csGAS = new ColumnSeries();
                csGAS.Title = "GAS";
                csGAS.Values = GasValues;
                seriesCollection.Add(csGAS);
            }
            if (cmbMeterType.SelectedItem.ToString().Contains("---") || cmbMeterType.SelectedItem.ToString().Contains("ELECTRICITY"))
            {
                ColumnSeries csELECTRICITY = new ColumnSeries();
                csELECTRICITY.Title = "ELECTRICITY";
                csELECTRICITY.Values = ElectroValues;
                seriesCollection.Add(csELECTRICITY);
            }
            if (cmbMeterType.SelectedItem.ToString().Contains("---") || cmbMeterType.SelectedItem.ToString().Contains("HOT WATER"))
            {
                ColumnSeries csHW = new ColumnSeries();
                csHW.Title = "HOT WATER";
                csHW.Values = HotWaterValues;
                seriesCollection.Add(csHW);
            }
            if (cmbMeterType.SelectedItem.ToString().Contains("---") || cmbMeterType.SelectedItem.ToString().Contains("COLD WATER"))
            {
                ColumnSeries csCW = new ColumnSeries();
                csCW.Title = "COLD WATER";
                csCW.Values = ColdWaterValues;
                seriesCollection.Add(csCW);
            }

            //Labels = readoutReports.Select(x => x.READOUT_DATE_REPORT).Distinct().ToArray();
            Labels = readoutReports.Select(x => x.READOUT_MONTH_REPORT).Distinct().ToArray();
            Formatter = value => value.ToString("N");
        }

        private void FillCombobox()
        {
            cmbMeterName.Items.Clear();
            cmbMeterType.Items.Clear();

            cmbMeterType.Items.Add("--- Select Meter type ---");

            cmbMeterType.Items.Add("GAS");
            cmbMeterType.Items.Add("ELECTRICITY");
            cmbMeterType.Items.Add("HOT WATER");
            cmbMeterType.Items.Add("COLD WATER");

            cmbMeterType.SelectedIndex = 0;

            // get unique meter names from DB, add them to combobox
            using (DB_context context = new DB_context())
            {
                cmbMeterName.Items.Add("--- Select Meter name ---");
                var meter_names = (from c in context.db_Meters select c).GroupBy(x => x.METER_NAME).Select(grp => grp.FirstOrDefault()).ToList();
                foreach (var item in meter_names)
                {
                    cmbMeterName.Items.Add(item.METER_NAME);
                }

                cmbMeterName.SelectedIndex = 0;
            }
        }

        private void LoadIntoTable()
        {
            using (DB_context context = new DB_context())
            {
                // clear contetns of the table
                tblReadouts.ItemsSource = null;
                readoutReports.Clear();

                // get all info from both tables in DB, otherwise too many SQL requests...
                listMeters = (from c in context.db_Meters select c).ToList();
                listReadouts = (from c in context.db_Readouts select c).ToList();

                foreach (var item in listReadouts)
                {
                    // foreach readout get corrisponding meter name and type from meter list
                    ReadoutReport report = new ReadoutReport();

                    report.ID_READOUT_REPORT = item.ID_READOUT;
                    var meter = listMeters.Where(x => x.ID_METER.Equals(item.ID_METER)).First();
                    report.METER_NAME_REPORT = meter.METER_NAME;
                    report.METER_TYPE_REPORT = meter.METER_TYPE;
                    report.READOUT_DATE_REPORT = item.READOUT_DATE.ToString("MM / yyyy", CultureInfo.InvariantCulture);
                    report.READOUT_MONTH_REPORT = item.READOUT_DATE.ToString("MMMM", CultureInfo.InvariantCulture);

                    //get previous readout and calculate consumption value
                    double prev_readout = LoadPreviousReadout(meter.ID_METER, item.ID_READOUT);

                    report.READOUT_CONSUPMTION_REPORT = item.READOUT_VALUE - prev_readout;

                    // if combobox value doesn't mathc --> skip adding to list
                    // if datepicker range is not OK --> skip adding to list
                    try
                    {
                        if (!cmbMeterType.SelectedItem.ToString().Contains("---") && report.METER_TYPE_REPORT != cmbMeterType.SelectedItem.ToString())
                        {
                            continue;
                        }
                        if (!cmbMeterName.SelectedItem.ToString().Contains("---") && report.METER_NAME_REPORT != cmbMeterName.SelectedItem.ToString())
                        {
                            continue;
                        }
                        if (dpFrom.SelectedDate != null && item.READOUT_DATE < dpFrom.SelectedDate)
                        {
                            continue;
                        }
                        if (dpTo.SelectedDate != null && item.READOUT_DATE > dpTo.SelectedDate)
                        {
                            continue;
                        }
                    }
                    catch { }

                    readoutReports.Add(report);
                }

                // sort list descending by readout date, show data in the table
                readoutReports = readoutReports.OrderByDescending(x => x.READOUT_DATE_REPORT).ToList();
                tblReadouts.ItemsSource = readoutReports;
            }
        }

        private double LoadPreviousReadout(int meter_id, int readout_id)
        {
            // get readout list for current meter
            var readout_list = (from c in listReadouts where c.ID_METER == meter_id select c).ToList();

            if (readout_list.Count == 0)
            {
                //in case of error --> last = 0
                return 0;
            }
            else
            {
                // find current readout in the list, get previous value
                var readout = (from c in listReadouts where c.ID_READOUT == readout_id select c).First();

                int index = readout_list.IndexOf(readout);
                if (index == 0)
                {
                    //if it is the first readout --> last = 0
                    return 0;
                }
                else
                {
                    return readout_list[index - 1].READOUT_VALUE;
                } 
            } 
        }


        // ========== BUTTONS ========== //


        private void btnGetData_Click(object sender, RoutedEventArgs e)
        {
            LoadIntoTable();
        }

        private void btnClearFilters_Click(object sender, RoutedEventArgs e)
        {
            cmbMeterName.SelectedIndex = 0;
            cmbMeterType.SelectedIndex = 0;
            dpFrom.SelectedDate = null;
            dpTo.SelectedDate = null;
        }


        // ========== PROPERTIES ========== //


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
