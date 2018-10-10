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
using System.Windows.Shapes;
using System.Data.Entity;
using Counter_Control.Model;
using System.Text.RegularExpressions;

namespace Counter_Control.Views
{
    /// <summary>
    /// Interaction logic for ManageMeter.xaml
    /// </summary>
    public partial class ManageMeter
    {

        public ManageMeter(MeterManagement owner,  int ID)
        {
            _owner = owner;

            InitializeComponent();

            meter_ID = ID;

            FillCombobox();

            LoadMeterFromDB();
        }


        // ========== EVENTS ========== //


        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _owner.LoadMeterList();
        }

        private void cmbMeterType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbMeterType.SelectedValue.ToString() == "ELECTRICITY")
            {
                cmbMeterUnits.Text = "kWh";
            }
            else
            {
                cmbMeterUnits.Text = "m3";
            }
        }


        // ========== GLOBAL VARIABLES ========== //


        MeterManagement _owner; // Declare owner to run functions in other form

        public int meter_ID; // used to retrieve data from database


        // ========== FUNCTIONS ========== //


        public void FillCombobox()
        {
            // combo box "Meter Type"
            cmbMeterType.Items.Clear();

            cmbMeterType.Items.Add("--- Select Meter type ---");

            cmbMeterType.Items.Add("GAS");
            cmbMeterType.Items.Add("ELECTRICITY");
            cmbMeterType.Items.Add("HOT WATER");
            cmbMeterType.Items.Add("COLD WATER");

            cmbMeterType.SelectedIndex = 0;

            // combo box "Meter Units"
            cmbMeterUnits.Items.Clear();
            cmbMeterUnits.Items.Add("--- Select Meter unit ---");

            cmbMeterUnits.Items.Add("kWh");
            cmbMeterUnits.Items.Add("m3");

            cmbMeterUnits.SelectedIndex = 0;
        }
        
        public void LoadMeterFromDB()
        {
            if (meter_ID == 0)
            {
                // new meter, nothing to load
                lbl_action.Content = "Add new meter";
            }
            else
            {
                // Edit meter
                lbl_action.Content = "Edit meter";

                // get Meter info from DB and set into data fields
                try
                {
                    using (DB_context context = new DB_context())
                    {
                        var query = (from c in context.db_Meters where c.ID_METER == meter_ID select c).FirstOrDefault();

                        txtMeterName.Text = query.METER_NAME;
                        cmbMeterType.Text = query.METER_TYPE;
                        cmbMeterUnits.Text = query.METER_UNITS;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error! DataBase not available!" + "\r\n" + ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool SaveToDB()
        {
            Regex regex = new Regex(@"(^[A-Za-z0-9-.\s]*$)"); // regex for letters, numbers, '-' and spaces
            Match matchName = regex.Match(txtMeterName.Text.Trim());

            if (!matchName.Success)
            {
                MessageBox.Show("Inappropriate Meter name, please choose another." + "\r\n" + "Allowed only leters, digits and spaces", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (cmbMeterType.Text.Contains("-"))
            {
                MessageBox.Show("Select Meter type", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (cmbMeterUnits.Text.Contains("-"))
            {
                MessageBox.Show("Select Meter units", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            /*
             * if no ID was sent --> create new entry in DB
             * else get entry by ID, update entry and save changes
             */ 

            if (meter_ID == 0) // new entry
            {
                // try save to DB
                try
                {
                    using (DB_context context = new DB_context())
                    {
                        tbl_Meters new_meter = new tbl_Meters();

                        new_meter.METER_NAME = txtMeterName.Text.Trim();
                        new_meter.METER_TYPE = cmbMeterType.Text;
                        new_meter.METER_UNITS = cmbMeterUnits.Text;
                        // add new 
                        context.db_Meters.Add(new_meter);
                        context.SaveChanges();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving to DB. \r\n\r\n" + ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            else // edit entry
            {
                try
                {
                    using (DB_context context = new DB_context())
                    {
                        tbl_Meters new_meter = new tbl_Meters();

                        var query = (from c in context.db_Meters where c.ID_METER == meter_ID select c).FirstOrDefault();

                        query.METER_NAME = txtMeterName.Text.Trim();
                        query.METER_TYPE = cmbMeterType.Text;
                        query.METER_UNITS = cmbMeterUnits.Text;

                        // save edited entry
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving to DB. \r\n\r\n" + ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            return true;
        } // save to DB

        // ========== BUTTONS ========== //

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // if some text is written --> ask to close form.
            if (txtMeterName.Text.Trim().Length > 1)
            {
                MessageBoxResult result = MessageBox.Show("Exit without saving?", "Info", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
                if (result == MessageBoxResult.Yes)
                {
                    this.Close();
                }
            }
            else // no text written
            {
                this.Close();
            }
        } // btn close

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bool result = SaveToDB();
            if (result)
            {
                MessageBox.Show("Saved!", "Info", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                this.Close();
            }
        } // btn save

    }
}
