using Business_Layer;
using DVLD_v1._0.People;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_v1._0.Licenses
{
    public partial class frmLocal_License_Application : Form
    {
        private string _search_by = "";
        clsPerson _Person;
        clsLicensesBuiness _LicenseClass = new clsLicensesBuiness();

        clsApplicationType _ApplicationType = clsApplicationType.Find("New Local Driving License Service");

        clsLicense_Application _Application = new clsLicense_Application();
        clsLocal_License _LocalLicense = new clsLocal_License();


        public delegate void RefreshDataGridEventHandler(object sender, EventArgs e);
        public event RefreshDataGridEventHandler RefreshGrid;
        private void frmLocal_License_Application_FormClosed(object sender, FormClosedEventArgs e)
        {
            RefreshGrid?.Invoke(this, e);

        }



        public frmLocal_License_Application()
        {
            InitializeComponent();
        }

        private void frmLocal_License_Application_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dVLDDataSet2.LicenseClasses' table. You can move, or remove it, as needed.
            this.licenseClassesTableAdapter.Fill(this.dVLDDataSet2.LicenseClasses);

             lblCreatedBy.Text = clsGlobalSettings._loggedUser.UserName;

            lblApplicationDate.Text = DateTime.Today.ToString("yyyy-MM-dd");

            comboBox1.SelectedIndex = 0;
            string selected_license = comboBox1.GetItemText(comboBox1.SelectedItem);
            _LicenseClass = clsLicensesBuiness.Find(selected_license);

            lblApplicationFees.Text = _ApplicationType.ApplicationFees.ToString();

            btnClearPerson.Enabled = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tbFilter.Text.IsNullOrEmpty())
                return;

            if (_search_by == "PersonID")
            {
                _Person = clsPerson.Find(Int32.Parse(tbFilter.Text));

                if (_Person == null) // if the searched person doesn't exist, escape method
                    return;

                ctrlPersonCard1.LoadPersonInfo(_Person.PersonID);
                btnClearPerson.Enabled = true;

                return;
            }
            else if (_search_by == "NationalNo") // if the searched person doesn't exist, escape method
            {
                _Person = clsPerson.Find(tbFilter.Text);
                btnClearPerson.Enabled = true;

                if (_Person == null)
                    return;

                ctrlPersonCard1.LoadPersonInfo(_Person.NationalNo);

                return;
            }

            else
            {
                btnClearPerson.Enabled = false;
                return;
            }

        }

        private void btnClearPerson_Click(object sender, EventArgs e)
        {
            _Person = null;

            ctrlPersonCard1.ClearPersonInfo();

            btnClearPerson.Enabled = false;

        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the key pressed is not a digit and not a control key (like Backspace)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                // If the input is not valid, stop it from being entered
                e.Handled = true;
            }

        }

        private void cbFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbFilters.Text)
            {
                case "Person ID":
                    _search_by = "PersonID";
                    break;

                case "National Number":

                    _search_by = "NationalNo";
                    break;

                default:
                    break;
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected_license = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);

            _LicenseClass = clsLicensesBuiness.Find(selected_license);

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(_Person==null) return;

            DataTable _dtLicenses = clsLocal_License.GetLocalLicensesInfo();

            DataView view = new DataView(_dtLicenses);

            // If ClassName is a string, surround it with single quotes
            string select1 = "CONVERT(NationalNo, 'System.String') = '" + _Person.NationalNo +
                             "' AND ClassName = '" + _LicenseClass.ClassName + "' AND Status = 'New'";

            // Apply the filter
            view.RowFilter = select1;

            if (view.Count > 0)
            {
                MessageBox.Show("Choose another License Class, the selected Person already " +
                    "has an active application for the selected class.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _Application.ApplicantPersonID = _Person.PersonID;
            _Application.ApplicationDate = DateTime.Now;
            _Application.ApplicationTypeID = _ApplicationType.ApplicationTypeID;
            _Application.ApplicationStatus = 1;     //  New
            _Application.LastStatusDate = DateTime.Now;
            _Application.PaidFees = _ApplicationType.ApplicationFees;
            _Application.CreatedByUserID = clsGlobalSettings._loggedUser.UserID;


            int DLApplication =  _Application.addApplication();

            _LocalLicense.ApplicationID = DLApplication;

            _LocalLicense.LicenseClassID = _LicenseClass.LicenseClassID;

            int LDLApplication = _LocalLicense.addLocalLicense();


            lblApplicationID.Text = LDLApplication.ToString();

            MessageBox.Show("New Application for license saved.");
            btnSave.Enabled = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();


        }

        private void local_license_application_databack(object sender, int PersonID)
        {
            _Person = clsPerson.Find(PersonID);

            if (_Person == null) // if the searched person doesn't exist, escape method
                return;

            ctrlPersonCard1.LoadPersonInfo(_Person.PersonID);

            btnClearPerson.Enabled = true;

        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAdd_Update_Person frm = new frmAdd_Update_Person();
            frm.DataBack += local_license_application_databack;
            frm.ShowDialog();
        }

    }
}
