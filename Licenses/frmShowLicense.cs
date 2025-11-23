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

namespace DVLD_v1._0.Licenses
{
    public partial class frmShowLicense : Form
    {
        int _LDL_APPID;

        clsLocal_License _LocalLicenseApplication = new clsLocal_License();

        clsLicense_Application _Application = new clsLicense_Application();

        clsPerson _Person = new clsPerson();

        clsIssuedLicenses _license = new clsIssuedLicenses();

        clsDriver _Driver = new clsDriver();

        public frmShowLicense(int LDL_APPID)
        {
            InitializeComponent();

            _LDL_APPID = LDL_APPID;

            _LocalLicenseApplication = clsLocal_License.Find(LDL_APPID);

            if (_LocalLicenseApplication != null)
                _Application = clsLicense_Application.Find(_LocalLicenseApplication.ApplicationID);

            if (_Application != null)
                _Person = clsPerson.Find(_Application.ApplicantPersonID);

            if (_Application != null)
                _license = clsIssuedLicenses.FindByApplicationID(_Application.ApplicationID);

            if (_Person != null)
                _Driver = clsDriver.FindByPersonID(_Person.PersonID);
        }

        public frmShowLicense(int ApplicationID, bool FindBYLicensesTable)
        {
            InitializeComponent();

            _Application = clsLicense_Application.Find(ApplicationID);

            if (_Application != null)
                _Person = clsPerson.Find(_Application.ApplicantPersonID);

            if (_Application != null)
                _license = clsIssuedLicenses.FindByApplicationID(_Application.ApplicationID);

            if (_Person != null)
                _Driver = clsDriver.FindByPersonID(_Person.PersonID);
        }

        private void frmShowLicense_Load(object sender, EventArgs e)
        {
            if (_license == null) return;

            lblClass.Text = clsLicensesBuiness.Find(_license.LicenseClass).ClassName.ToString();
            lblName.Text = _Person.FirstName + " " + _Person.SecondName + " " + _Person.ThirdName + " " + _Person.LastName;
            lblLicenseID.Text = _license.LicenseID.ToString();
            lblNationalNo.Text=_Person.NationalNo.ToString();
            lblGender.Text = _Person.Gender;
            lblIssueDate.Text=_license.IssueDate.ToString("MMMM dd, yyyy");

            switch (_license.IssueReason)
            {
                case 1:
                    lblIssueReason.Text = "First Time";
                    break;
                case 2:
                    lblIssueReason.Text = "Renew";
                    break;
                case 3:
                    lblIssueReason.Text = "Replacement For Lost";
                    break;
                case 4:
                    lblIssueReason.Text = "Replacement For Damaged";
                    break;
                default:
                    lblIssueReason.Text = _license.IssueReason.ToString();
                    break;
            }

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
    }
}
