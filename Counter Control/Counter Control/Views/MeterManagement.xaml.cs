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
using System.Data.Entity;

namespace Counter_Control.Views
{
    /// <summary>
    /// Interaction logic for MeterManagement.xaml
    /// </summary>
    public partial class MeterManagement : UserControl
    {
        public MeterManagement()
        {
            InitializeComponent();
            LoadMeterList();
        }

        // ========== GLOBAL VARIABLES ========== //
        List<tbl_Meters> list_meters = new List<tbl_Meters>();

        // ========== EVENTS ========== //

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CollectionView collection = (CollectionView)CollectionViewSource.GetDefaultView(tblMeters.ItemsSource);
                collection.Filter = new Predicate<object>(Filter);
                CollectionViewSource.GetDefaultView(tblMeters.Items).Refresh();
            }
            catch (Exception)
            {
            }
        }

        // ========== FUNCTIONS ========== //

        private bool Filter(object item)
        {
            return string.IsNullOrEmpty(txtFilter.Text)
            || (item as tbl_Meters).METER_NAME.ToString().IndexOf(txtFilter.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0
            || (item as tbl_Meters).METER_TYPE.ToString().IndexOf(txtFilter.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0
            || (item as tbl_Meters).METER_UNITS.ToString().IndexOf(txtFilter.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0
            ;
        }

        public void LoadMeterList()
        {
            /*
             * Get all data from DB and show it in the table
             */

            list_meters.Clear();
            tblMeters.ItemsSource = null;
            try
            {
                DB_context context = new DB_context();

                context.db_Meters.Load();
                list_meters = context.db_Meters.Local.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error, DataBase is not available.\r\n\r\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            tblMeters.ItemsSource = list_meters;
        }

        // function for editing selected meter
        private void EditMeter(object sender, RoutedEventArgs e)
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
                    int id = Convert.ToInt32((tblMeters.SelectedCells[0].Column.GetCellContent(row) as TextBlock).Text);

                    ManageMeter manageMeter = new ManageMeter(this, id);
                    manageMeter.Show();
                }
            }
        } // edit meter

        // function for deleting Meter and corresponding readings
        private void DeleteMeter(object sender, RoutedEventArgs e)
        {
            /*
             * find selected row, get ID from first column (hidden column "ID")
             * find entry in DB based on ID, 
             */
            MessageBoxResult result = MessageBox.Show("Delete this meter and this meter readouts?", "Info", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                {
                    if (vis is DataGridRow)
                    {
                        var row = (DataGridRow)vis;
                        int id = Convert.ToInt32((tblMeters.SelectedCells[0].Column.GetCellContent(row) as TextBlock).Text);

                        using (EnergyMetersEntities context = new EnergyMetersEntities())
                        {
                            /*
                             * TRY Using stored procedure to delete meter from tbl_Meters 
                             * and all readouts with matching ID_METER from tbl_Readouts
                             * 
                             * IN CASE OF STORED PROCEDURE FAIL
                             * try using linq expression
                             */
                            try
                            {
                                context.DeleteMeterAndReadouts(id);
                                MessageBox.Show("Success!", "Info", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                            }
                            catch
                            {
                                try
                                {
                                    using (DB_context EF_context = new DB_context())
                                    {
                                        var meter = (from c in EF_context.db_Meters where c.ID_METER == id select c).FirstOrDefault();

                                        EF_context.db_Meters.Remove(meter);

                                        var readouts = (from c in EF_context.db_Readouts where c.ID_METER == id select c).ToList();

                                        foreach (var item in readouts)
                                        {
                                            EF_context.db_Readouts.Remove(item);
                                        }

                                        EF_context.SaveChanges();

                                        MessageBox.Show("Success!", "Info", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                                    } // using
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error, DataBase is not available.\r\n\r\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            } // catch Stored procedure
                        } // using ...

                    } // if is datagrid row
                } // for ... cycle
            } // if result == yes

            // update list on finish
            LoadMeterList();
        } // delete meter

        // ========== BUTTONS ========== //

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ManageMeter manageMeter = new ManageMeter(this, 0);
            manageMeter.Show();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadMeterList();
        }

    }
}
