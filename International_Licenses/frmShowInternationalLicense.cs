using Business_Layer;
using DVLD_v1._0.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_v1._0.Licenses
{
    public partial class frmShowInternationalLicense : Form
    {
        int _IDL_APPID;

        clsInternationalLicense _InternationalLicense = new clsInternationalLicense();

        clsLicense_Application _Application = new clsLicense_Application();

        clsPerson _Person = new clsPerson();

        clsIssuedLicenses _license = new clsIssuedLicenses();

        clsDriver _Driver = new clsDriver();

        public frmShowInternationalLicense(int IDL_APPID)
        {
            InitializeComponent();

            _IDL_APPID = IDL_APPID;

            _InternationalLicense = clsInternationalLicense.Find(IDL_APPID);

            if (_InternationalLicense != null)
                _Application = clsLicense_Application.Find(_InternationalLicense.ApplicationID);

            if (_Application != null)
                _Person = clsPerson.Find(_Application.ApplicantPersonID);

            if (_Application != null)
                _license = clsIssuedLicenses.FindByApplicationID(_Application.ApplicationID);

            if (_Person != null)
                _Driver = clsDriver.FindByPersonID(_Person.PersonID);

        }

        private void frmShowInternationalLicense_Load(object sender, EventArgs e)
        {
            if (_InternationalLicense == null)
                return;

            lblName.Text = _Person.FirstName + " " + _Person.SecondName + " " + _Person.ThirdName + " " + _Person.LastName;
            lblIntLicenseID.Text = _InternationalLicense.InternationalLicenseID.ToString();
            lblLicenseID.Text = _InternationalLicense.IssuedUsingLocalLicenseID.ToString();
            lblNationalNo.Text = _Person.NationalNo;
            lblGender.Text = _Person.Gender;
            lblIssueDate.Text = _InternationalLicense.IssueDate.ToString("MMMM dd, yyyy");
            lblApplicationID.Text = _Application.ApplicationID.ToString();

            if (_InternationalLicense.IsActive == true)
                lblIsActive.Text = "YES";

            if (_InternationalLicense.IsActive == false)
                lblIsActive.Text = "NO";

            lblDateOfBirth.Text = _Person.DateOfBirth.ToString("MMMM dd, yyyy");
            lblDriverID.Text = _Driver.DriverID.ToString();
            lblExpirationDate.Text = _InternationalLicense.ExpirationDate.ToString("MMMM dd, yyyy");

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



        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }


    }
}
