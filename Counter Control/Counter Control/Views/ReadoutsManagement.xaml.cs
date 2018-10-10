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
using Counter_Control.Model;
using Counter_Control.Views;

namespace Counter_Control.Views
{
    /// <summary>
    /// Interaction logic for ReadoutsManagement.xaml
    /// </summary>
    public partial class ReadoutsManagement : UserControl
    {
        public ReadoutsManagement()
        {
            InitializeComponent();
            LoadMeters();
        }

        // ========== GLOBAL VARIABLES ========== //


        List<tbl_Meters> list_Meters = new List<Model.tbl_Meters>();
        List<tbl_Readouts> list_Readouts = new List<Model.tbl_Readouts>();


        // ========== EVENTS ========== //


        private void ViewReadouts(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
            {
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    int id = Convert.ToInt32((tbl_Meters.SelectedCells[0].Column.GetCellContent(row) as TextBlock).Text);

                    LoadReadoutsForMeter(id);
                }
            }
        }

        private void txtMeterFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CollectionView collection = (CollectionView)CollectionViewSource.GetDefaultView(tbl_Meters.ItemsSource);
                collection.Filter = new Predicate<object>(MeterFilter);
                CollectionViewSource.GetDefaultView(tbl_Meters.Items).Refresh();
            }
            catch (Exception)
            {
            }
        }

        private void txtReadoutFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CollectionView collection = (CollectionView)CollectionViewSource.GetDefaultView(tbl_Readouts.ItemsSource);
                collection.Filter = new Predicate<object>(ReportFilter);
                CollectionViewSource.GetDefaultView(tbl_Readouts.Items).Refresh();
            }
            catch (Exception)
            {
            }
        }


        // ========== FUNCTIONS ========== //


        private bool ReportFilter(object item)
        {
            return string.IsNullOrEmpty(txtReadoutFilter.Text)
            || (item as tbl_Readouts).READOUT_DATE.ToString().IndexOf(txtReadoutFilter.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0
            || (item as tbl_Readouts).READOUT_VALUE.ToString().IndexOf(txtReadoutFilter.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0
            || (item as tbl_Readouts).READOUT_COMMENT.ToString().IndexOf(txtReadoutFilter.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0
            ;
        }

        private bool MeterFilter(object item)
        {
            return string.IsNullOrEmpty(txtMeterFilter.Text)
            || (item as tbl_Meters).METER_NAME.ToString().IndexOf(txtMeterFilter.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0
            || (item as tbl_Meters).METER_TYPE.ToString().IndexOf(txtMeterFilter.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0
            || (item as tbl_Meters).METER_UNITS.ToString().IndexOf(txtMeterFilter.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0
            ;
        }

        public void LoadMeters()
        {
            /*
             * Load all meters
             * user clicks the button near the meter to load readouts for this meter
             */ 
            list_Meters.Clear();
            tbl_Meters.ItemsSource = null;

            try
            {
                using (DB_context context = new DB_context())
                {
                    list_Meters = (from c in context.db_Meters select c).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error, DataBase is not available.\r\n\r\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            tbl_Meters.ItemsSource = list_Meters;
        }

        public void LoadReadoutsForMeter(int ID)
        {
            /*
             * Retrieve readouts based on meter selected
             * Set values into table
             */
            list_Readouts.Clear();
            tbl_Readouts.ItemsSource = null;
            try
            {
                using (DB_context context = new DB_context())
                {
                    list_Readouts = (from c in context.db_Readouts where c.ID_METER == ID orderby c.ID_READOUT descending select c).ToList();

                    var meter = (from c in context.db_Meters where c.ID_METER == ID select c).FirstOrDefault();
                    lblReadoutsHeader.Content = "ALL READOUTS FOR: " + meter.METER_NAME.ToUpper() + " - " + meter.METER_TYPE.ToUpper();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error, DataBase is not available.\r\n\r\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            tbl_Readouts.ItemsSource = list_Readouts;
        }

        public void OpenWindowAddReadout(int ID_readout, int ID_meter)
        {
            ManageReadout manageReadout = new ManageReadout(this, ID_readout, ID_meter);
            manageReadout.Show();
        }


        // ========== BUTTONS ========== //


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            // manual select meter
            OpenWindowAddReadout(0, 0);
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadMeters();
        }

        private void EditReadout(object sender, RoutedEventArgs e)
        {
            /*
             * find selected row, get ID from first column (hidden column "ID")
             * send "self" that "update" function could be called from another window
             * send ID to the Editing window, open it; 
             * Function in the editing window loads data based on ID
             */
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
            {
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    int id = Convert.ToInt32((tbl_Readouts.SelectedCells[0].Column.GetCellContent(row) as TextBlock).Text);

                    ManageReadout manageReadout = new ManageReadout(this, id, 0);
                    manageReadout.Show();
                }
            }
        } // edit meter

        private void DeleteReadout(object sender, RoutedEventArgs e)
        {
            int ID_meter = 0;
            /*
             * find selected row, get ID from first column (hidden column "ID")
             * find entry in DB based on ID, 
             * delete entry, save changes
             */
            MessageBoxResult result = MessageBox.Show("Delete this entry?", "Info", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                {
                    if (vis is DataGridRow)
                    {
                        var row = (DataGridRow)vis;
                        int id = Convert.ToInt32((tbl_Readouts.SelectedCells[0].Column.GetCellContent(row) as TextBlock).Text);

                        using (DB_context context = new DB_context())
                        {
                            var query = (from c in context.db_Readouts where c.ID_READOUT == id select c).FirstOrDefault();

                            ID_meter = query.ID_METER;

                            context.db_Readouts.Remove(query);
                            context.SaveChanges();

                            MessageBox.Show("Success!", "Info", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        } // using
                    } // if is datagrid row
                } // for ... cycle
            } // if result == yes
            else
            {
                return;
            }

            // update list on finish
            LoadReadoutsForMeter(ID_meter);
        }

        private void AddReadout(object sender, RoutedEventArgs e)
        {
            /*
             * find selected row, get ID from first column (hidden column "ID")
             * 
             */

            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
            {
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    int id = Convert.ToInt32((tbl_Meters.SelectedCells[0].Column.GetCellContent(row) as TextBlock).Text);

                    OpenWindowAddReadout(0 ,id);
                } // if is datagrid row
            } // for ... cycle
        } // Add readout

    }
}
