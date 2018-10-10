using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Counter_Control.Model
{
    public class tbl_Meters
    {
        [Key]
        public int ID_METER { get; set; }

        [MaxLength(250)]
        public string METER_NAME { get; set; }

        [MaxLength(250)]
        public string METER_TYPE { get; set; }

        [MaxLength(50)]
        public string METER_UNITS { get; set; }
    }

    public class tbl_Readouts
    {
        [Key]
        public int ID_READOUT { get; set; }

        public int ID_METER { get; set; }

        public DateTime READOUT_DATE { get; set; }

        public double READOUT_VALUE { get; set; }

        [MaxLength(250)]
        public string READOUT_COMMENT { get; set; }

    }
    
    public class DB_context : DbContext
    {
        public DB_context() : base("DefaultConnection")
        { }

        public DbSet<tbl_Meters> db_Meters { get; set; }
        public DbSet<tbl_Readouts> db_Readouts { get; set; }
    }

}
