using Business_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_v1._0.Licenses
{
    public partial class frmIssue_Driver_License_First_Time : Form
    {
        DataTable _dataTable;
        int _currentRow;

        int LDLAppID;
        string DrivingClass;
        string NationalNo;
        string FullName;
        DateTime ApplicationDate;
        int PassedTests;
        string Status;

        public delegate void RefreshDataGridEventHandler(object sender, EventArgs e);
        public event RefreshDataGridEventHandler RefreshGrid;
        private void frmIssue_Driver_License_First_Time_FormClosed(object sender, FormClosedEventArgs e)
        {
            RefreshGrid?.Invoke(this, e);
        }

        public frmIssue_Driver_License_First_Time(DataTable dataTable, int currentRow)
        {
            InitializeComponent();
            _dataTable = dataTable;
            _currentRow = currentRow;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmIssue_Driver_License_First_Time_Load(object sender, EventArgs e)
        {
            ctrlDrivingLicenseApplicationInfo1.LoadLDLApplicationInfo(_dataTable, _currentRow);
            ctrlDrivingLicenseApplicationInfo1.FillInfo();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            clsIssuedLicenses _licenceToIssue = new clsIssuedLicenses();
            clsLicense_Application _Application;
            clsLocal_License _LocalLicenseApplication;

            DataRow row = _dataTable.Rows[_currentRow];

            LDLAppID = (int)row[0];
            DrivingClass = (string)row[1];
            NationalNo = (string)row[2];
            FullName = (string)row[3];
            ApplicationDate = (DateTime)row[4];
            PassedTests = (int)row[5];
            Status = (string)row[6];

            _LocalLicenseApplication = clsLocal_License.Find(LDLAppID);
            _Application = clsLicense_Application.Find(_LocalLicenseApplication.ApplicationID);



            // 1-search if exists driver record for this person already (by PersonID)
            // 2-if exists use the driver id in the license and issue it
            // 3-if not, create a new _driver for person, return _driver id, add it to license info then issue it
            // 4-issue license to his information
            // 5-update _Application.Status to 3 (Completed).


            clsDriver _driver = clsDriver.FindByPersonID(_Application.ApplicantPersonID);
            if (_driver != null)
            {
                _licenceToIssue.DriverID = _driver.DriverID;
            }
            else
            {
                _driver = new clsDriver();
                _driver.PersonID = _Application.ApplicantPersonID;
                _driver.CreatedByUserID= clsGlobalSettings._loggedUser.UserID;
                _driver.CreatedDate = DateTime.Now;
                int DriverID = _driver.AddNewDriver();

                _licenceToIssue.DriverID = DriverID;
            }



            _licenceToIssue.ApplicationID = _Application.ApplicationID;
            
            clsLicensesBuiness _licenseClass = clsLicensesBuiness.Find(DrivingClass);
            
            _licenceToIssue.LicenseClass = _licenseClass.LicenseClassID;
            _licenceToIssue.IssueDate = DateTime.Now;
            _licenceToIssue.ExpirationDate = DateTime.Now.AddYears(_licenseClass.DefaultValidityLength);
            _licenceToIssue.Notes = tbNotes.Text;
            _licenceToIssue.PaidFees = _licenseClass.ClassFees;
            _licenceToIssue.IsActive = true;
            _licenceToIssue.IssueReason = 1;
            _licenceToIssue.CreatedByUserID = clsGlobalSettings._loggedUser.UserID;

            int New_licenseID = _licenceToIssue.AddNewLicense();
            MessageBox.Show("License ID = "+ New_licenseID + " issued to driver " + _licenceToIssue.DriverID + " Successfully.");

            _Application.ApplicationStatus = 3; //  completed
            _Application.UpdateApplication();
            this.Close();
        }
    }
}

