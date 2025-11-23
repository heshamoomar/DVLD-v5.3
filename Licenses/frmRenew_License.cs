using Business_Layer;
using DVLD_v1._0.Properties;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace DVLD_v1._0.Licenses
{
    public partial class frmRenew_License : Form
    {
        int _LDL_ID = 0;
        int _New_LDL_APPID;

        decimal RenewFees;
        decimal LicenseFees;
        decimal totalFees;

        clsIssuedLicenses _license = new clsIssuedLicenses();

        clsLicense_Application _Application = new clsLicense_Application();

        clsPerson _Person = new clsPerson();

        clsDriver _Driver = new clsDriver();

        clsLocal_License _Local_License = new clsLocal_License();

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public frmRenew_License()
        {
            InitializeComponent();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.IsNullOrEmpty())
            { return; }

            _LDL_ID = Int32.Parse(textBox1.Text);
        }
        private void _Display_LocalLicense_Info()
        {
            if (_license == null) return;

            lblClass.Text = clsLicensesBuiness.Find(_license.LicenseClass).ClassName.ToString();
            lblName.Text = _Person.FirstName + " " + _Person.SecondName + " " + _Person.ThirdName + " " + _Person.LastName;
            lblLicenseID.Text = _license.LicenseID.ToString();
            lblOldLicenseID.Text = _license.LicenseID.ToString();
            lblNationalNo.Text = _Person.NationalNo.ToString();
            lblGender.Text = _Person.Gender;
            lblIssueDate.Text = _license.IssueDate.ToString("MMMM dd, yyyy");


            switch (_license.IssueReason)
            {
                case 1:
                    lblIssueReason.Text = "First Time";
                    break;
                case 2:
                    lblIssueReason.Text = "Renew";
                    break;
                case 3:
                    lblIssueReason.Text = "Repl. For Dameged";
                    break;
                case 4:
                    lblIssueReason.Text = "Repl. For Lost";
                    break;
                default:
                    lblIssueReason.Text = _license.IssueReason.ToString();
                    break;
            }

            lblIsActive.Text = _license.IsActive.ToString();
            lblDataOfBirth.Text = _Person.DateOfBirth.ToString("MMMM dd, yyyy");
            lblDriverID.Text = _Driver.DriverID.ToString();
            lblExpirationDate.Text = _license.ExpirationDate.ToString("MMMM dd, yyyy");
            lblILAppID.Text = _license.ApplicationID.ToString();

            if (_Person.Gender.StartsWith("M"))
            {
                pb1.Image = Resources.man__1_;
            }
            else
            {
                pb1.Image = Resources.woman__1_;
            }

            if (_Person.ImagePath != "")
            {
                if (File.Exists(_Person.ImagePath))
                    pb1.Load(_Person.ImagePath);
            }
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            _license = clsIssuedLicenses.FindByLicenseID(_LDL_ID);

            if (_license == null)
                return;

            _Application = clsLicense_Application.Find(_license.ApplicationID);

            if (_Application == null)
                return;

            _Person = clsPerson.Find(_Application.ApplicantPersonID);

            if (_Person == null)
                return;

            _Driver = clsDriver.FindByPersonID(_Person.PersonID);

            _Display_LocalLicense_Info();

            if (_license.ExpirationDate > DateTime.Now)
            {
                MessageBox.Show("License is not yet expired.",
                "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnIssue.Enabled = false;

                lblILExpirationDate.Text = _license.ExpirationDate.ToString("MMMM dd, yyyy");
            }
            else
            {
                btnIssue.Enabled = true;

                int validity = clsLicensesBuiness.Find(_license.LicenseClass).DefaultValidityLength;
                lblILExpirationDate.Text = DateTime.Now.AddYears(validity).ToString("MMMM dd, yyyy");

                RenewFees = clsApplicationType.Find("Renew Driving License Service").ApplicationFees;
                LicenseFees = clsLicensesBuiness.Find(_license.LicenseClass).ClassFees;
                totalFees = RenewFees + LicenseFees;

                lblRenewFees.Text = RenewFees.ToString();
                lblLicenseFees.Text = LicenseFees.ToString();
                lblTotalFees.Text = totalFees.ToString();
                llShowLicenseHistory.Enabled = true;
            }

        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void btnIssue_Click(object sender, EventArgs e)
        {
            // Now Create a new Application of type Renew Liecnse
            // Application type License Renew
            // Paid Fees fees of the License Renew application type
            // Last status update Datetime.Now
            // Make license active again and extend expiration date by the default validity length for this license type
            // update Application and License

            DialogResult dr = MessageBox.Show("Are you sure you want to Renew this license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
                return;

            clsLicense_Application NewApplication = new clsLicense_Application();
            NewApplication.ApplicantPersonID = _Person.PersonID;
            NewApplication.ApplicationDate = DateTime.Now;
            NewApplication.LastStatusDate = DateTime.Now;
            NewApplication.ApplicationTypeID = 2; //  Renew
            NewApplication.ApplicationStatus = 3; //  Completed
            NewApplication.PaidFees = clsApplicationType.Find("Renew Driving License Service").ApplicationFees;
            NewApplication.CreatedByUserID = clsGlobalSettings._loggedUser.UserID;
            int NewApplicationID = NewApplication.addApplication();

            _license.IsActive = false;  // make old license inactive
            _license.UpdateLicense();   // update the old one to make it inactive

            clsIssuedLicenses NewLicense = new clsIssuedLicenses();
            NewLicense.ApplicationID = NewApplicationID;
            NewLicense.DriverID = _Driver.DriverID;
            NewLicense.LicenseClass = _license.LicenseClass;
            NewLicense.IssueDate = DateTime.Now;

            int validity = clsLicensesBuiness.Find(_license.LicenseClass).DefaultValidityLength;
            NewLicense.ExpirationDate = DateTime.Now.AddYears(validity);
            NewLicense.Notes = tbNotes.Text;
            NewLicense.PaidFees = totalFees;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = 2; // Renew
            NewLicense.CreatedByUserID = clsGlobalSettings._loggedUser.UserID;
            int NewLiceseID = NewLicense.AddNewLicense();

            _Local_License.ApplicationID = NewApplication.ApplicationID;
            _Local_License.LicenseClassID = NewLicense.LicenseClass;
            _Local_License.addLocalLicense();

            lblILAppID.Text = NewApplicationID.ToString();
            lblInternationalLicenseID.Text = NewLiceseID.ToString();
            btnIssue.Enabled = false;
            llShowLicenseInfo.Enabled = true;

            MessageBox.Show("Lincese renewed with new License ID = " + NewLiceseID,
                "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void frmRenew_License_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = clsGlobalSettings._loggedUser.UserName;
            lblILAppDate.Text = DateTime.Now.ToString("MMMM dd, yyyy");
            lblILIssueDate.Text = DateTime.Now.ToString("MMMM dd, yyyy");
            lblRenewFees.Text = clsApplicationType.Find("Renew Driving License Service").ApplicationFees.ToString();

        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicense frm = new frmShowLicense(_Local_License.LocalDrivingLicenseApplicationID);
            frm.ShowDialog();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            frmShow_Issued_Licenses frm = new frmShow_Issued_Licenses(_Application.ApplicationID);
            frm.ShowDialog();

        }
    }
}
