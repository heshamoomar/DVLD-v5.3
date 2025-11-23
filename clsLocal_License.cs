using Data_Access_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer
{
    public class clsLocal_License
    {
        public int LocalDrivingLicenseApplicationID { get; set; }
        public int ApplicationID { get; set; }
        public int LicenseClassID { get; set; }

        // default empty constructor
        public clsLocal_License()
        {
            LocalDrivingLicenseApplicationID = -1;
            ApplicationID = -1;
            LicenseClassID = -1;

        }

        private clsLocal_License(int LocalDrivingLicenseApplicationID, int ApplicationID, int LicenseClassID)
        {
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.ApplicationID = ApplicationID;
            this.LicenseClassID = LicenseClassID;
        }

        public int addLocalLicense()
        {
            return this.LocalDrivingLicenseApplicationID = Local_License_Data.addLocalLicense(this.ApplicationID, this.LicenseClassID);
        }

        public static DataTable GetLocalLicensesInfo()
        {
            return Local_License_Data.GetAllLocalLicenseInfo();
        }

        public static clsLocal_License Find(int LocalDrivingLicenseApplicationID)
        {
            int ApplicationID = -1;
            int LicenseClassID = -1;

            if (Local_License_Data.GetLicenseInfo(LocalDrivingLicenseApplicationID, ref ApplicationID, ref LicenseClassID))
            {
                return new clsLocal_License(LocalDrivingLicenseApplicationID, ApplicationID, LicenseClassID);
            }
            else
            {
                return null;
            }
        }

        public static bool deleteApplication(int LocalDrivingLicenseApplicationID)
        {
            return Local_License_Data.DeleteApplication(LocalDrivingLicenseApplicationID);
        }

    }
}
