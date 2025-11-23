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
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_v1._0.Tests
{


    public partial class frmSchedule_Vision_Test2 : Form
    {
        private enum enFormMode
        {
            update = 1,
            addNew = 2,
        }
        private enFormMode _form_mode = enFormMode.addNew;

        private enum enTestMode
        {
            vision = 1,
            written = 2,
            street = 3,
        }

        private enTestMode _test_mode = enTestMode.vision; // default



        int LDLAppID;
        string DrivingClass;
        string NationalNo;
        string FullName;
        DateTime ApplicationDate;
        int PassedTests;
        string Status;
        clsTestsBusiness _TestType = new clsTestsBusiness();
        clsTest_Appointment _Appointment = new clsTest_Appointment();
        private clsLicense_Application _Application=new clsLicense_Application();
        private clsLocal_License _LocalLicense = new clsLocal_License();

        public delegate void RefreshDataGridEventHandler(object sender, EventArgs e);
        public event RefreshDataGridEventHandler RefreshGrid;
        private void frmSchedule_Vision_Test2_FormClosed(object sender, FormClosedEventArgs e)
        {
            RefreshGrid?.Invoke(this, e);
        }

        public frmSchedule_Vision_Test2(DataTable dataTable, int currentRow, int testMode)
        {
            InitializeComponent();

            DataRow row = dataTable.Rows[currentRow];

            LDLAppID = (int)row[0];
            DrivingClass = (string)row[1];
            NationalNo = (string)row[2];
            FullName = (string)row[3];
            ApplicationDate = (DateTime)row[4];
            PassedTests = (int)row[5];
            Status = (string)row[6];


            _test_mode = (enTestMode)testMode;

        }

        public frmSchedule_Vision_Test2(DataTable dataTable, int currentRow, DateTime setDateTime, int TestAppointmentID, int testMode)  // edit mode
        {
            InitializeComponent();

            DataRow row = dataTable.Rows[currentRow];

            LDLAppID = (int)row[0];
            DrivingClass = (string)row[1];
            NationalNo = (string)row[2];
            FullName = (string)row[3];
            ApplicationDate = (DateTime)row[4];
            PassedTests = (int)row[5];
            Status = (string)row[6];

            dateTimePicker1.Value = setDateTime;
            _Appointment.TestAppointmentID = TestAppointmentID; // important to know which record from tests shall be updated
            _form_mode = enFormMode.update;

            if (testMode == 1)
                _test_mode = enTestMode.vision;
            else if (testMode == 2)
                _test_mode = enTestMode.written;
            else
                _test_mode = enTestMode.street;

        }

        private void frmSchedule_Vision_Test2_Load(object sender, EventArgs e)
        {
            if (_test_mode == enTestMode.vision)
            {
                pictureBox2.Image = Resources.eye;
            }
            else if (_test_mode == enTestMode.written)
            {
                pictureBox2.Image = Resources.exam;
            }
            else
            {
                pictureBox2.Image = Resources.car;
            }


            _TestType = clsTestsBusiness.Find((int)_test_mode);

            _LocalLicense = clsLocal_License.Find(LDLAppID);

            lblLicenseAppID.Text = _LocalLicense.ApplicationID.ToString();
            lblAppliedForLicense.Text = DrivingClass;
            lblApplicant.Text = FullName;
            lblAppFees.Text = _TestType.TestTypeFees.ToString();


            dateTimePicker1.MinDate = DateTime.Today.AddDays(1);
            btnSave.Enabled = false;



            //_Appointment.TestTypeID = 1; // vision test
            _Appointment.TestTypeID = (int)_test_mode;
            _Appointment.LocalDrivingLicenseApplicationID = LDLAppID;
            _Appointment.PaidFees = (decimal)_TestType.TestTypeFees;
            _Appointment.CreatedByUserID = clsGlobalSettings._loggedUser.UserID;
            _Appointment.IsLocked = false;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = true;

            _Appointment.AppointmentDate = dateTimePicker1.Value.Add(DateTime.Now.TimeOfDay);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_form_mode == enFormMode.update)
            {
                _Appointment.UpdateTest_Appointment();
                MessageBox.Show("Appointment date updated Successfully");
                this.Close();
                return;
            }

            if(_Appointment.AddNewTest_Appointment()>0)
            {
                MessageBox.Show("Data Saved Successfully");
            }

            this.Close();
        }

    }
}
