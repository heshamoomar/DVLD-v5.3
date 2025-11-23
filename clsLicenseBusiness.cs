using Data_Access_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer
{
    public class clsIssuedLicenses
    {
        public int LicenseID { get; set; } = -1;
        public int ApplicationID { get; set; } = -1;
        public int DriverID { get; set; } = -1;
        public int LicenseClass { get; set; } = -1;
        public DateTime IssueDate { get; set; } = DateTime.Now;
        public DateTime ExpirationDate { get; set; } = DateTime.Now;
        public string Notes { get; set; } = string.Empty;
        public decimal PaidFees { get; set; } = -1;
        public bool IsActive { get; set; } = false;
        public byte IssueReason { get; set; } = 0;
        public int CreatedByUserID { get; set; } = -1;


        public clsIssuedLicenses() { }

        // private constructor for internal use by me to fill a user datafields
        private clsIssuedLicenses(int LicenseID, int ApplicationID, int DriverID, int LicenseClass,
                DateTime IssueDate, DateTime ExpirationDate, string Notes, decimal PaidFees,
                bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            this.LicenseID = LicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.LicenseClass = LicenseClass;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.Notes = Notes;
            this.PaidFees = PaidFees;
            this.IsActive = IsActive;
            this.IssueReason = IssueReason;
            this.CreatedByUserID = CreatedByUserID;
        }

        public static clsIssuedLicenses FindByLicenseID(int LicenseID)
        {
            int ApplicationID = -1;
            int DriverID = -1;
            int LicenseClass = -1;
            DateTime IssueDate  = DateTime.Now;
            DateTime ExpirationDate = DateTime.Now;
            string Notes = string.Empty;
            decimal PaidFees = -1;
            bool IsActive = false;
            byte IssueReason = 0;
            int CreatedByUserID = -1;


            if (LicensesData.getInfoByLicenseID(LicenseID, ref ApplicationID, ref DriverID, ref LicenseClass,
                ref IssueDate, ref ExpirationDate, ref Notes, ref PaidFees, ref IsActive,
                ref IssueReason, ref CreatedByUserID))
            {
                return new clsIssuedLicenses(LicenseID, ApplicationID, DriverID, LicenseClass,
                    IssueDate, ExpirationDate, Notes, PaidFees, IsActive,
                    IssueReason, CreatedByUserID);
            }
            else
            {
                return null;
            }
        }

        public static clsIssuedLicenses FindByApplicationID(int ApplicationID)
        {
            int LicenseID = -1;
            int DriverID = -1;
            int LicenseClass = -1;
            DateTime IssueDate  = DateTime.Now;
            DateTime ExpirationDate = DateTime.Now;
            string Notes = string.Empty;
            decimal PaidFees = -1;
            bool IsActive = false;
            byte IssueReason = 0;
            int CreatedByUserID = -1;


            if (LicensesData.getInfoByApplicationID(ApplicationID, ref LicenseID, ref DriverID, ref LicenseClass,
                ref IssueDate, ref ExpirationDate, ref Notes, ref PaidFees, ref IsActive,
                ref IssueReason, ref CreatedByUserID))
            {
                return new clsIssuedLicenses(LicenseID, ApplicationID, DriverID, LicenseClass,
                    IssueDate, ExpirationDate, Notes, PaidFees, IsActive,
                    IssueReason, CreatedByUserID);
            }
            else
            {
                return null;
            }
        }

        public int AddNewLicense()
        {
            return this.LicenseID = LicensesData.addLicense(this.ApplicationID, this.DriverID, this.LicenseClass,
                            this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees,
                            this.IsActive, this.IssueReason, this.CreatedByUserID);
        }

        public bool UpdateLicense()
        {
            return LicensesData.updateLicense(this.LicenseID, this.ApplicationID, this.DriverID, this.LicenseClass,
                            this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees,
                            this.IsActive, this.IssueReason, this.CreatedByUserID);
        }

        public static bool ActiveLicenseExistsForApplication(int ApplicationID)
        {
            return LicensesData.ActiveLicenseExistsForApplication(ApplicationID);
        }

        public static bool ActiveLicenseExistsForDriver(int DriverID)
        {
            return LicensesData.ActiveLicenseExistsForDriver(DriverID);
        }

        public static DataTable ShowAllLicensesForDriver(int DriverID)
        {
            return LicensesData.GetAllLicensesByDriverID(DriverID);
        }

    }
}
