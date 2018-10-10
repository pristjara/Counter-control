using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counter_Control.Class
{
    // Class for readouts
    class ReadoutReport
    {
        public int ID_READOUT_REPORT { get; set; }

        public string METER_NAME_REPORT { get; set; }

        public string METER_TYPE_REPORT { get; set; }

        public string READOUT_DATE_REPORT { get; set; }

        public string READOUT_MONTH_REPORT { get; set; }

        public double READOUT_CONSUPMTION_REPORT { get; set; }

    }
    
    // class for combobox with all Meters
    class Meter
    {
        public string MeterTitle { get; set; }
        public int Meter_ID { get; set; }
    }
}
