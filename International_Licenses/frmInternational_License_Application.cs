using Business_Layer;
using DVLD_v1._0.Properties;
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
using Microsoft.IdentityModel.Tokens;
using DVLD_v1._0.Licenses;

namespace DVLD_v1._0.International_Licenses
{
    public partial class frmInternational_License_Application : Form
    {
        int _LDL_ID = 0;

        clsIssuedLicenses _license = new clsIssuedLicenses();

        clsLicense_Application _Application = new clsLicense_Application();

        clsPerson _Person = new clsPerson();

        clsDriver _Driver = new clsDriver();

        clsInternationalLicense _newInternationalLicense = new clsInternationalLicense();

        public frmInternational_License_Application()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text.IsNullOrEmpty())
                { return; }

            _LDL_ID = Int32.Parse(textBox1.Text);
        }

        private void _Display_LocalLicense_Info()
        {
            if (_license == null) return;

            lblClass.Text = clsLicensesBuiness.Find(_license.LicenseClass).ClassName.ToString();
            lblName.Text = _Person.FirstName + " " + _Person.SecondName + " " + _Person.ThirdName + " " + _Person.LastName;
            lblLicenseID.Text = _license.LicenseID.ToString();
            lblLicenseID2.Text = _license.LicenseID.ToString();
            lblNationalNo.Text = _Person.NationalNo.ToString();
            lblGender.Text = _Person.Gender;
            lblIssueDate.Text = _license.IssueDate.ToString("MMMM dd, yyyy");
            lblIssueReason.Text = _license.IssueReason.ToString();
            lblIsActive.Text = _license.IsActive.ToString();
            lblDataOfBirth.Text = _Person.DateOfBirth.ToString("MMMM dd, yyyy");
            lblDriverID.Text = _Driver.DriverID.ToString();
            lblExpirationDate.Text = _license.ExpirationDate.ToString("MMMM dd, yyyy");

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

            btnIssue.Enabled = true;
            _Display_LocalLicense_Info();
        }

        private void btnAddPerson_KeyPress(object sender, KeyPressEventArgs e)
        {

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
            if (_license == null)
                return;

            if (_license.LicenseClass != 3)
            {
                MessageBox.Show("International License is only available for Class 3 - Ordinary driving license.",
                    "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_license.IsActive == false)
            {
                MessageBox.Show("Person needs to have an active local license.",
                    "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (clsInternationalLicense.isExistsActiveILForThisLocalLicense(_license.LicenseID))    // if already exists and is active
            {
                MessageBox.Show("An active international license is already issued for this local license.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _newInternationalLicense = new clsInternationalLicense();
            _newInternationalLicense.ApplicationID = _license.ApplicationID;
            _newInternationalLicense.DriverID = _license.DriverID;
            _newInternationalLicense.IssuedUsingLocalLicenseID = _license.LicenseID;
            _newInternationalLicense.IssueDate = DateTime.Now;
            _newInternationalLicense.ExpirationDate = DateTime.Now.AddYears(2);
            _newInternationalLicense.IsActive = true;
            _newInternationalLicense.CreatedByUserID = clsGlobalSettings._loggedUser.UserID;

            DialogResult dr = MessageBox.Show("Are you sure you want to issue the license?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.Yes && _newInternationalLicense.AddNewLicense() > 0)
            {
                MessageBox.Show("International license with ID = " + _newInternationalLicense.InternationalLicenseID + " has been issued.",
                "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);

                lblILAppID.Text = _newInternationalLicense.InternationalLicenseID.ToString();
                btnIssue.Enabled = false;
                llShowLicenseInfo.Enabled = true;
            }

        }

        private void frmInternational_License_Application_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = clsGlobalSettings._loggedUser.UserName;
            lblILAppDate.Text = DateTime.Now.ToString("MMMM dd, yyyy");
            lblILIssueDate.Text = DateTime.Now.ToString("MMMM dd, yyyy");
            lblILFees.Text = clsApplicationType.Find("New International License").ApplicationFees.ToString();
            lblILExpirationDate.Text = DateTime.Now.AddYears(2).ToString("MMMM dd, yyyy");  //  2 year validity
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowInternationalLicense frm = new frmShowInternationalLicense(_newInternationalLicense.InternationalLicenseID);
            frm.ShowDialog();
        }
    }
}
