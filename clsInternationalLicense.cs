using Data_Access_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer
{
    public class clsInternationalLicense
    {
        public int InternationalLicenseID { get; set; } = -1;
        public int ApplicationID { get; set; } = -1;
        public int DriverID { get; set; } = -1;
        public int IssuedUsingLocalLicenseID { get; set; } = -1;
        public DateTime IssueDate { get; set; } = DateTime.Now;
        public DateTime ExpirationDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        public int CreatedByUserID { get; set; } = -1;

        public clsInternationalLicense() { }

        private clsInternationalLicense(int InternationalLicenseID, int ApplicationID, 
            int DriverID, int IssuedUsingLocalLicenseID, DateTime IssueDate, DateTime ExpirationDate, 
            bool IsActive, int CreatedByUserID)
        {
            this.InternationalLicenseID = InternationalLicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.IssuedUsingLocalLicenseID = IssuedUsingLocalLicenseID;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.IsActive = IsActive;
            this.CreatedByUserID = CreatedByUserID;

        }
        public static DataTable GetILDatatable()
        {
            return International_LicenseData.GetAllILInfo();
        }

        public static DataTable GetILDatatableForDriver(int DriverID)
        {
            return International_LicenseData.GetAllDriverILInfo(DriverID);
        }

        public static bool isExistsActiveILForThisLocalLicense(int IssuedUsingLocalLicenseID)
        {
            return (International_LicenseData.isExistsActiveILForThisLocalLicense(IssuedUsingLocalLicenseID));
        }

        public int AddNewLicense()
        {
            return this.InternationalLicenseID = International_LicenseData.AddLicense(this.ApplicationID,
                this.DriverID, this.IssuedUsingLocalLicenseID, this.IssueDate, this.ExpirationDate, this.IsActive, this.CreatedByUserID);
        }

        public static clsInternationalLicense Find(int InternationalLicenseID)
        {
            int ApplicationID = -1;
            int DriverID = -1;
            int IssuedUsingLocalLicenseID = -1;
            DateTime IssueDate = DateTime.Now;
            DateTime ExpirationDate = DateTime.Now;
            bool IsActive = true;
            int CreatedByUserID = -1;


            if (International_LicenseData.getLicenseInfo(InternationalLicenseID, ref ApplicationID, ref DriverID,
                ref IssuedUsingLocalLicenseID, ref IssueDate, ref ExpirationDate, ref IsActive, ref CreatedByUserID))
            {
                return new clsInternationalLicense( InternationalLicenseID,  ApplicationID,
                    DriverID,  IssuedUsingLocalLicenseID,  IssueDate,  ExpirationDate,
                    IsActive,  CreatedByUserID);
            }
            else
            {
                return null;
            }
        }


    }
}
