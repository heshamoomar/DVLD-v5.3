using Business_Layer;
using DVLD_v1._0.People;
using DVLD_v1._0.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_v1._0.Controls
{
    public partial class ctrlDrivingLicenseApplicationInfo : UserControl
    {
        private clsLicense_Application _Application;
        clsLocal_License _LocalLicenseApplication;

        private int _ApplicationID = -1;

        public int ApplicationID
        {
            get { return _ApplicationID; }
        }

        int LDLAppID;
        string DrivingClass;
        string NationalNo;
        string FullName;
        DateTime ApplicationDate;
        int PassedTests;
        string Status;

        public ctrlDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }

        private void ctrlDrivingLicenseApplicationInfo_Load(object sender, EventArgs e)
        {

        }

        public void LoadLDLApplicationInfo(DataTable dataTable, int currentRow)
        {
            DataRow row = dataTable.Rows[currentRow];

            LDLAppID = (int)row[0];
            DrivingClass=(string)row[1];
            NationalNo=(string)row[2];
            FullName=(string)row[3];
            ApplicationDate=(DateTime)row[4];
            PassedTests=(int)row[5];
            Status=(string)row[6];

            _LocalLicenseApplication = clsLocal_License.Find(LDLAppID);
        }

        public void FillInfo()
        {
            _Application = clsLicense_Application.Find(_LocalLicenseApplication.ApplicationID);

            _ApplicationID = _Application.ApplicationID;

            lblLicenseAppID.Text = _ApplicationID.ToString();
            lblAppliedForLicense.Text = DrivingClass;
            lblPassedTests.Text = PassedTests.ToString() + "/3";
            lblAppID.Text = LDLAppID.ToString();
            lblAppStatus.Text = Status;
            lblAppFees.Text = _Application.PaidFees.ToString();
            switch (_Application.ApplicationTypeID)
            {
                case 1:
                    lblAppType.Text = "New Local Driving License Service";
                    break;

                case 2:
                    lblAppType.Text = "Renew Driving License Service";
                    break;

                case 3:
                    lblAppType.Text = "Replacement for a Lost Driving License";
                    break;

                case 4:
                    lblAppType.Text = "Replacement for a Damaged Driving License";
                    break;

                case 5:
                    lblAppType.Text = "Release Detained Driving License";
                    break;

                case 6:
                    lblAppType.Text = "New International License";
                    break;

                case 7:
                    lblAppType.Text = " ";
                    break;

                case 8:
                    lblAppType.Text = "Retake Test";
                    break;

                default:
                    break;
            }
            lblApplicant.Text = FullName;
            lblAppDate.Text = _Application.ApplicationDate.ToString("MMMM dd, yyyy");
            lblStatusDate.Text = _Application.LastStatusDate.ToString("MMMM dd, yyyy");
            clsUser _User = clsUser.Find(_Application.CreatedByUserID);
            lblCreatedBy.Text=_User.UserName;
        }


        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmDisplayPerson frm = new frmDisplayPerson(_Application.ApplicantPersonID);
            frm.ShowDialog();
        }
    }
}
