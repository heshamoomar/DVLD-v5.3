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
    public partial class frmShow_Issued_Licenses : Form
    {
        clsLicense_Application _Application = new clsLicense_Application();
        clsPerson _Person = new clsPerson();
        clsDriver _Driver = new clsDriver();

        private enum enFormMode
        {
            Local = 1,
            Local2 = 2,
            Int = 3,
            Driver = 4,
            DriverInt = 5,
        }

        private enFormMode _form_mode = enFormMode.Local;

        int _LDL_APPID;
        int _IDL_APPID;
        string _NationalNo;
        DataTable _dtIssuedLicenses = new DataTable();

        public frmShow_Issued_Licenses(int LDL_APPID, string NationalNo)
        {
            InitializeComponent();

            _LDL_APPID = LDL_APPID;
            _NationalNo = NationalNo;
        }

        public frmShow_Issued_Licenses(int ApplicationID)
        {
            InitializeComponent();

            _Application = clsLicense_Application.Find(ApplicationID);
            _Person = clsPerson.Find(_Application.ApplicantPersonID);
            _NationalNo = _Person.NationalNo;

            _form_mode = enFormMode.Local2;
        }

        public frmShow_Issued_Licenses(int DriverID, bool Driver, string NationalNo)
        {
            InitializeComponent();

            _Driver = clsDriver.FindByDriverID(DriverID);
            _NationalNo = NationalNo;

            _form_mode = enFormMode.Driver;
        }

        public frmShow_Issued_Licenses(int DriverID, bool Driver, bool International, string NationalNo)
        {
            InitializeComponent();

            _Driver = clsDriver.FindByDriverID(DriverID);
            _NationalNo = NationalNo;

            _form_mode = enFormMode.DriverInt;
        }

        public frmShow_Issued_Licenses(int IDL_APPID, string NationalNo, bool International)
        {
            InitializeComponent();

            _IDL_APPID = IDL_APPID;
            _NationalNo = NationalNo;

            if(International)
            {
                _form_mode = enFormMode.Int;
            }
        }

        private void RefreshRecordsCount(DataTable dt)
        {
            string noRecords = dt.Rows.Count.ToString();
            lblRecordsCount.Text = noRecords;
            if (dataGridView1.Rows.Count > 0)   // In case the datagridview is empty so I don't try to access a non-existing column when trying to sort
                dataGridView1.Sort(dataGridView1.Columns[0], System.ComponentModel.ListSortDirection.Descending);
        }

        private void frmShow_Issued_Licenses_Load(object sender, EventArgs e)
        {
            if(_form_mode==enFormMode.Int)
            {
                label1.Text = "International License History";

                clsInternationalLicense _Int_License = clsInternationalLicense.Find(_IDL_APPID);

                _dtIssuedLicenses = clsInternationalLicense.GetILDatatableForDriver(_Int_License.DriverID);
                dataGridView1.DataSource = _dtIssuedLicenses;
                
                RefreshRecordsCount(_dtIssuedLicenses);               
            }

            else if(_form_mode == enFormMode.Local2)
            {
                clsDriver _driver = clsDriver.FindByPersonID(_Person.PersonID);

                if (_driver != null)
                {
                    _dtIssuedLicenses = clsIssuedLicenses.ShowAllLicensesForDriver(_driver.DriverID);
                    dataGridView1.DataSource = _dtIssuedLicenses;

                    RefreshRecordsCount(_dtIssuedLicenses);

                }
            }

            else if(_form_mode == enFormMode.Driver)
            {
                if (_Driver != null)
                {
                    _dtIssuedLicenses = clsIssuedLicenses.ShowAllLicensesForDriver(_Driver.DriverID);
                    dataGridView1.DataSource = _dtIssuedLicenses;

                    RefreshRecordsCount(_dtIssuedLicenses);
                }
            }

            else if(_form_mode == enFormMode.DriverInt)
            {
                label1.Text = "International License History";

                _dtIssuedLicenses = clsInternationalLicense.GetILDatatableForDriver(_Driver.DriverID);
                dataGridView1.DataSource = _dtIssuedLicenses;

                RefreshRecordsCount(_dtIssuedLicenses);
            }

            else
            {
                clsLocal_License _Local_License = clsLocal_License.Find(_LDL_APPID);

                _Application = clsLicense_Application.Find(_Local_License.ApplicationID);

                clsDriver _driver = clsDriver.FindByPersonID(_Application.ApplicantPersonID);

                if (_driver != null)
                {
                    _dtIssuedLicenses = clsIssuedLicenses.ShowAllLicensesForDriver(_driver.DriverID);
                    dataGridView1.DataSource = _dtIssuedLicenses;

                    RefreshRecordsCount(_dtIssuedLicenses);

                }
            }


            ctrlPersonCard1.LoadPersonInfo(_NationalNo);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // show an international license
            if(_form_mode==enFormMode.Int|| _form_mode==enFormMode.DriverInt)
            {
                int IntLicenseID;

                int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
                Int32.TryParse(dataGridView1[0, currentRowIndex].Value.ToString(), out IntLicenseID);

                frmShowInternationalLicense frm = new frmShowInternationalLicense(IntLicenseID);
                frm.ShowDialog();
            }
            else
            {
                // show normal license
                int ApplicationID;

                int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
                Int32.TryParse(dataGridView1[1, currentRowIndex].Value.ToString(), out ApplicationID);

                frmShowLicense frm = new frmShowLicense(ApplicationID, true);
                frm.ShowDialog();
            }
        }
    }
}
