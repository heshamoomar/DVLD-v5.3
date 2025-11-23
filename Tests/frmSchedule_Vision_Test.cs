using Business_Layer;
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

namespace DVLD_v1._0.Tests
{
    public partial class frmSchedule_Vision_Test : Form
    {
        private enum enFormMode
        {
            vision = 1,
            written = 2,
            street = 3,
        }

        private enFormMode _Form_mode = enFormMode.vision; // default

        DataTable _dtLicenses, _Appointments;
        int _currentRow;

        public delegate void RefreshDataGridEventHandler(object sender, EventArgs e);
        public event RefreshDataGridEventHandler RefreshGrid;

        public frmSchedule_Vision_Test(DataTable dataTable, int currentRow, int testType)
        {
            InitializeComponent();

            _dtLicenses = dataTable;
            _currentRow = currentRow;

            if (testType == 1)
                _Form_mode = enFormMode.vision;
            else if (testType == 2)
                _Form_mode = enFormMode.written;
            else if (testType == 3)
                _Form_mode = enFormMode.street;
        }

        private void RefreshRecordsCount(DataTable dt)
        {
            string noRecords = dt.Rows.Count.ToString();
            lblRecordsCount.Text = noRecords;
        }

        private void btnScheduleTest_Click(object sender, EventArgs e)
        {

            int LocalDrivingLicenseApplicationID;

            DataRow row = _dtLicenses.Rows[_currentRow];

            LocalDrivingLicenseApplicationID = (int)row[0];

            if (_Form_mode == enFormMode.vision)
            {
            _Appointments = clsTestsBusiness.GetTestAppointmentsDatatable_Vision(LocalDrivingLicenseApplicationID);

            }
            else if (_Form_mode == enFormMode.written)
            {
                _Appointments = clsTestsBusiness.GetTestAppointmentsDatatable_Written(LocalDrivingLicenseApplicationID);
            }
            else
            {
                _Appointments = clsTestsBusiness.GetTestAppointmentsDatatable_Street(LocalDrivingLicenseApplicationID);
            }



            DataView view = new DataView(_Appointments);

            if (view.Count != 0)
            {
            //------------------------------------------------------------------------------------
            //check testType passed
            clsTakeTest _test = new clsTakeTest();

            foreach (DataRowView rowView in view)
            {
                // Access column values using the column name or index
                int TestAppointmentID = (int)rowView["TestAppointmentID"];

                _test = clsTakeTest.Find(TestAppointmentID);

                if (_test != null && _test.TestResult == true)  //  a testType was taken and passed therefore don't allow any more testType applications
                {
                    MessageBox.Show("Applicant has already passed this testType.");
                    return;
                }
            }

            //------------------------------------------------------------------------------------

            //check a testType already scheduled

            string select1 = "IsLocked = false";

            view.RowFilter = select1;

            if (view.Count > 0)
            {
                MessageBox.Show("A testType is already scheduled.");
                return;
            }
            //------------------------------------------------------------------------------------

            }




            frmSchedule_Vision_Test2 frm = new frmSchedule_Vision_Test2(_dtLicenses, _currentRow, (int)_Form_mode);
            frm.RefreshGrid += frmSchedule_Vision_Test_Load;
            frm.ShowDialog();
        }

        int TestAppointmentID;

        private void editAppointmentToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
            DateTime setDateTime = DateTime.Parse(dataGridView1[1, currentRowIndex].Value.ToString());
            TestAppointmentID = Int32.Parse(dataGridView1[0, currentRowIndex].Value.ToString());

            frmSchedule_Vision_Test2 frm = new frmSchedule_Vision_Test2(_dtLicenses, _currentRow, setDateTime, TestAppointmentID, (int)_Form_mode);  // call in update mode
            frm.RefreshGrid += frmSchedule_Vision_Test_Load;
            frm.ShowDialog();

        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
            TestAppointmentID = Int32.Parse(dataGridView1[0, currentRowIndex].Value.ToString());

            frmTake_Vision_Test frm =new frmTake_Vision_Test(_dtLicenses, _currentRow, TestAppointmentID, (int)_Form_mode);
            frm.RefreshGrid += frmSchedule_Vision_Test_Load;
            frm.ShowDialog();
        }

        private void frmSchedule_Vision_Test_FormClosed(object sender, FormClosedEventArgs e)
        {
            RefreshGrid?.Invoke(this, e);
        }

        private void frmSchedule_Vision_Test_Load(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID;

            DataRow row = _dtLicenses.Rows[_currentRow];

            LocalDrivingLicenseApplicationID = (int)row[0];


            if (_Form_mode == enFormMode.vision)
            {
                pictureBox1.Image = Resources.eye;
                label1.Text = "Vision Test Appointments";
                _Appointments = clsTestsBusiness.GetTestAppointmentsDatatable_Vision(LocalDrivingLicenseApplicationID);
                this.Text = "Schedule Vision Test";
            }
            else if (_Form_mode == enFormMode.written)
            {
                pictureBox1.Image = Resources.exam;
                label1.Text = "Written Test Appointments";
                _Appointments = clsTestsBusiness.GetTestAppointmentsDatatable_Written(LocalDrivingLicenseApplicationID);
                this.Text = "Schedule Written Test";
            }
            else
            {
                pictureBox1.Image = Resources.car;
                label1.Text = "Street Test Appointments";
                _Appointments = clsTestsBusiness.GetTestAppointmentsDatatable_Street(LocalDrivingLicenseApplicationID);
                this.Text = "Schedule Street Test";
            }

            ctrlDrivingLicenseApplicationInfo1.LoadLDLApplicationInfo(_dtLicenses, _currentRow);
            ctrlDrivingLicenseApplicationInfo1.FillInfo();


            dataGridView1.DataSource = _Appointments;
            RefreshRecordsCount(_Appointments);
        }
    }
}
