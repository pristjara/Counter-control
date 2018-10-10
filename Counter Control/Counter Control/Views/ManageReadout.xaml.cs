using Counter_Control.Model;
using Counter_Control.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Text.RegularExpressions;

namespace Counter_Control.Views
{
    /// <summary>
    /// Interaction logic for ManageReadout.xaml
    /// </summary>
    public partial class ManageReadout 
    {
        public ManageReadout(ReadoutsManagement owner, int ID_readout, int ID_meter)
        {
            _owner = owner;

            InitializeComponent();

            readout_ID = ID_readout;
            meter_ID = ID_meter;

            LoadMeterListToCMB();

            DataContext = this;

            // default date - today
            dp_readoutAdded.SelectedDate = DateTime.Now;
        }

        // ========== GLOBAL VARIABLES ========== //


        ReadoutsManagement _owner; // Declare owner to run functions in other form

        public int readout_ID;

        public int meter_ID; 


        // ========== EVENTS ========== //


        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (readout_ID == 0)
            {
                lbl_action.Content = "Add new readout";
                if (meter_ID != 0)
                {
                    cmbMeters.SelectedValue = meter_ID;
                    LoadPreviousReadout(meter_ID);
                }
            }
            else
            {
                lbl_action.Content = "Edit readout";

                cmbMeters.IsEnabled = false;

                // load data from DB into the form
                try
                {
                    using (DB_context context = new DB_context())
                    {
                        var readout = (from c in context.db_Readouts where c.ID_READOUT == readout_ID select c).FirstOrDefault();

                        var meter = (from c in context.db_Meters where c.ID_METER == readout.ID_METER select c).FirstOrDefault();

                        cmbMeters.Text = meter.METER_NAME + " - " + meter.METER_TYPE;

                        dp_readoutAdded.SelectedDate = readout.READOUT_DATE;

                        txtNewReadout.Text = readout.READOUT_VALUE.ToString();

                        txtComment.Text = readout.READOUT_COMMENT;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading from DB. \r\n\r\n" + ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                }
            } // edit readout
        } // window loaded

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // reload data in owner form
            _owner.LoadMeters();
            _owner.tbl_Readouts.ItemsSource = null;
        }
        
        private void cmbMeters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selected_id = Convert.ToInt32(cmbMeters.SelectedValue);
            if (selected_id == 0)
            {
                txtLastReadout.Text = "0";
                return;
            }

            LoadPreviousReadout(selected_id);
        }


        // ========== BUTTONS ========== //


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // if one of the fields is not empty, warn user
            if (txtNewReadout.Text.Trim().Length > 0 || txtComment.Text.Trim().Length > 0)
            {
                MessageBoxResult result = MessageBox.Show("Exit without saving?", "Info", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
                if (result == MessageBoxResult.Yes)
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        } // btn close

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // verify that all data is entered correctly

            int meter_ID = Convert.ToInt32(cmbMeters.SelectedValue);
            
            if (meter_ID < 1)
            {
                MessageBox.Show("Meter is not selected!", "Info", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (dp_readoutAdded.SelectedDate == null)
            {
                MessageBox.Show("Date is not selected!", "Info", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            // check is the value is OK

            if (txtNewReadout.Text.Trim().Length == 0)
            {
                MessageBox.Show("New readout value is missing!" + "\r\n" + "Please enter new readout value.", "Info", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtNewReadout.Focus();
                return;
            }

            string readout_text = txtNewReadout.Text.Trim().Replace('.', ',');
            if (!readout_text.Contains(","))
            {
                readout_text += ",00";
            }
            else
            {
                if (readout_text.IndexOf(',') == (readout_text.Length - 2))
                {
                    readout_text += "0";
                }
            }
            Regex regexReadout = new Regex(@"^\d+(,\d{1,3})?$");

            Match match = regexReadout.Match(readout_text);
            if (!match.Success)
            {
                MessageBox.Show("Wrong readout format!" + "\r\n" + "Please enter correct readout value.", "Info", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtNewReadout.Focus();
                return;
            }

            double readout = Convert.ToDouble(readout_text);

            double old_readout = Convert.ToDouble(txtLastReadout.Text);

            if (readout < old_readout)
            {
                MessageBox.Show("New readout is smaller than the previous one!" + "\r\n" + "Please enter correct readout value.", "Info", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtNewReadout.Focus();
                return;
            }

            bool result = SaveReadout(readout);
            if (result)
            {
                MessageBox.Show("Successfuly saved!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        } // btn save


        // ========== FUNCTIONS ========== //


        private void LoadMeterListToCMB()
        {
            /*
             * Get all meters from DB
             * Add them into combobox so that user can see only name and type of meter, but value is ID_METER
             */

            using (DB_context context = new DB_context())
            {
                var query = (from c in context.db_Meters select c).ToList();
                List<Meter> list_meters = new List<Meter>();

                Meter firstVal = new Meter();
                firstVal.MeterTitle = "--- Select Meter ---";
                firstVal.Meter_ID = 0;
                list_meters.Add(firstVal);

                foreach (var item in query)
                {
                    Meter meter = new Meter();
                    meter.MeterTitle = item.METER_NAME + " - " + item.METER_TYPE;
                    meter.Meter_ID = item.ID_METER;

                    list_meters.Add(meter);
                }

                cmbMeters.ItemsSource = list_meters;
                cmbMeters.SelectedIndex = 0;
            }
        }

        private bool SaveReadout(double new_readout_value)
        {
            // readout == 0 --> create new entry
            if (readout_ID == 0)
            {
                try
                {
                    using (DB_context context = new DB_context())
                    {
                        tbl_Readouts readout = new tbl_Readouts();

                        readout.ID_METER = Convert.ToInt32(cmbMeters.SelectedValue);
                        readout.READOUT_DATE = dp_readoutAdded.SelectedDate.Value;
                        readout.READOUT_VALUE = new_readout_value;
                        readout.READOUT_COMMENT = txtComment.Text.Trim();

                        // add new entry
                        context.db_Readouts.Add(readout);
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving to DB. \r\n\r\n" + ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            // readout != 0 --> edit existing entry
            else
            {
                try
                {
                    using (DB_context context = new DB_context())
                    {
                        var query = (from c in context.db_Readouts where c.ID_READOUT == readout_ID select c).FirstOrDefault();
                        query.READOUT_DATE = dp_readoutAdded.SelectedDate.Value;
                        query.READOUT_VALUE = new_readout_value;
                        query.READOUT_COMMENT = txtComment.Text.Trim();

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
        } // save readout

        private void LoadPreviousReadout(int meter_id)
        {
            // get previous reading from DB bssed on selected counter
            try
            {
                using (DB_context context = new DB_context())
                {
                    var readout_list = (from c in context.db_Readouts where c.ID_METER == meter_id select c).ToList();
                    
                    if (readout_list.Count == 0)
                    {
                        //if it is the first readout --> last = 0
                        txtLastReadout.Text = "0";
                    }
                    else
                    {
                        // if it is not the first readout, determine, is it new entry or edit?
                        if (readout_ID == 0)
                        {
                            // new 
                            txtLastReadout.Text = readout_list.Last().READOUT_VALUE.ToString();
                            dp_previousReadout.SelectedDate = readout_list.Last().READOUT_DATE;
                        }
                        else
                        {
                            // edit: find current readout in the list, get previous value
                            var readout = (from c in context.db_Readouts where c.ID_READOUT == readout_ID select c).First();

                            int index = readout_list.IndexOf(readout);
                            if (index == 0)
                            {
                                //if it is the first readout --> last = 0
                                txtLastReadout.Text = "0";
                                dp_previousReadout.SelectedDate = null;
                            }
                            else
                            {
                                txtLastReadout.Text = readout_list[index - 1].READOUT_VALUE.ToString();
                                dp_previousReadout.SelectedDate = readout_list[index - 1].READOUT_DATE;
                            } // editing first entry?
                        } // new or edit?
                    } // first readout?
                } // using db context
            }
            catch
            {
            }
        }

    } // main class
} // namespace
