using Data_Access_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer
{
    public class clsDriver
    {
        public int DriverID { get; set; } = -1;
        public int PersonID { get; set; } = -1;
        public int CreatedByUserID { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public clsDriver() { }

        // private constructor for internal use by me to fill a user datafields
        private clsDriver(int DriverID, int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            this.DriverID = DriverID;
            this.PersonID = PersonID;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedDate = CreatedDate;
        }

        public static clsDriver FindByPersonID(int PersonID)
        {
            int DriverID = -1;
            int CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.Now;

            if (DriversData.GetInfoByPersonID(PersonID, ref DriverID, ref CreatedByUserID, ref CreatedDate))
            {
                return new clsDriver(DriverID, PersonID, CreatedByUserID, CreatedDate);
            }
            else
            {
                return null;
            }
        }

        public static clsDriver FindByDriverID(int DriverID)
        {
            int PersonID = -1;
            int CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.Now;

            if (DriversData.GetInfoByDriverID(DriverID, ref PersonID, ref CreatedByUserID, ref CreatedDate))
            {
                return new clsDriver(DriverID, PersonID, CreatedByUserID, CreatedDate);
            }
            else
            {
                return null;
            }
        }

        public int AddNewDriver()
        {
            return this.DriverID = DriversData.addDriver(this.PersonID, this.CreatedByUserID, this.CreatedDate);
        }

        public static DataTable GetDriversDatatable()
        {
            return DriversData.GetAllDrivers();
        }


    }
}
