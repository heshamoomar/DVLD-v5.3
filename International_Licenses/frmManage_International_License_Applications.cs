using Business_Layer;
using DVLD_v1._0.People;
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
    public partial class frmManage_International_License_Applications : Form
    {
        private DataTable _dtIntLicenses;

        clsInternationalLicense _IntLicense = new clsInternationalLicense();

        clsLicense_Application _Application = new clsLicense_Application();

        clsPerson _Person = new clsPerson();

        public frmManage_International_License_Applications()
        {
            InitializeComponent();
        }

        private void RefreshRecordsCount(DataTable dt)
        {
            string noRecords = dt.Rows.Count.ToString();
            lblRecordsCount.Text = noRecords;
        }


        private void frmManage_International_License_Applications_Load(object sender, EventArgs e)
        {
            _dtIntLicenses = clsInternationalLicense.GetILDatatable();
            dataGridView1.DataSource = _dtIntLicenses;

            RefreshRecordsCount(_dtIntLicenses);

        }

        private void showPersonalDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int IDL_APPID;

            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
            Int32.TryParse(dataGridView1[0, currentRowIndex].Value.ToString(), out IDL_APPID);

            _IntLicense = clsInternationalLicense.Find(IDL_APPID);

            _Application = clsLicense_Application.Find(_IntLicense.ApplicationID);

            _Person = clsPerson.Find(_Application.ApplicantPersonID);

            frmDisplayPerson frm = new frmDisplayPerson(_Person.PersonID);
            frm.ShowDialog();
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int IDL_APPID;

            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
            Int32.TryParse(dataGridView1[0, currentRowIndex].Value.ToString(), out IDL_APPID);

            frmShowInternationalLicense frm = new frmShowInternationalLicense(IDL_APPID);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int IDL_APPID;

            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
            Int32.TryParse(dataGridView1[0, currentRowIndex].Value.ToString(), out IDL_APPID);

            _IntLicense = clsInternationalLicense.Find(IDL_APPID);

            _Application = clsLicense_Application.Find(_IntLicense.ApplicationID);

            _Person = clsPerson.Find(_Application.ApplicantPersonID);

            frmShow_Issued_Licenses frm = new frmShow_Issued_Licenses(IDL_APPID, _Person.NationalNo, true);
            frm.ShowDialog();

        }
    }
}
