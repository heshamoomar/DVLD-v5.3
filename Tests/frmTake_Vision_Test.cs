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
    public partial class frmTake_Vision_Test : Form
    {
        private enum enFormMode
        {
            vision = 1,
            written = 2,
            street = 3,
        }

        private enFormMode _Form_mode = enFormMode.vision; // default

        int LDLAppID;
        string DrivingClass;
        string NationalNo;
        string FullName;
        DateTime ApplicationDate;
        int PassedTests;
        string Status;

        clsTestsBusiness _TestType = new clsTestsBusiness();
        private clsLocal_License _LocalLicense = new clsLocal_License();
        clsTest_Appointment _Appointment = new clsTest_Appointment();

        public delegate void RefreshDataGridEventHandler(object sender, EventArgs e);
        public event RefreshDataGridEventHandler RefreshGrid;

        private void frmTake_Vision_Test_FormClosed(object sender, FormClosedEventArgs e)
        {
            RefreshGrid?.Invoke(this, e);
        }


        public frmTake_Vision_Test(DataTable dataTable, int currentRow, int TestAppointmentID, int formMode)
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

            _Appointment = clsTest_Appointment.Find(TestAppointmentID);

            _Form_mode = (enFormMode)formMode;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
        }

        private void frmTake_Vision_Test_Load(object sender, EventArgs e)
        {
            if (_Form_mode == enFormMode.vision)
            {
                pictureBox2.Image = Resources.eye;
            }
            else if (_Form_mode == enFormMode.written)
            {
                pictureBox2.Image = Resources.exam;
            }
            else
            {
                pictureBox2.Image = Resources.car;
            }

            _TestType = clsTestsBusiness.Find((int)_Form_mode);

            _LocalLicense = clsLocal_License.Find(LDLAppID);

            lblLicenseAppID.Text = _LocalLicense.ApplicationID.ToString();
            lblAppliedForLicense.Text=DrivingClass.ToString();
            lblApplicant.Text = FullName;
            lblAppFees.Text = _TestType.TestTypeFees.ToString();
            lblDate.Text = _Appointment.AppointmentDate.ToString("MMMM dd, yyyy");

            radioButton1.Checked = false;
            radioButton2.Checked = false;
            
        }

        clsTakeTest _Test = new clsTakeTest();

        private void btnSave_Click(object sender, EventArgs e)
        {

            _Appointment.IsLocked = true;   //  if test is taken (whether passed or failed) then mark it locked and update 
            _Appointment.UpdateTest_Appointment();

            _Test.TestAppointmentID = _Appointment.TestAppointmentID;

            if(radioButton1.Checked)
                _Test.TestResult = true;
            else
                _Test.TestResult = false;

            _Test.Notes = textBox1.Text;

            _Test.CreatedByUserID = clsGlobalSettings._loggedUser.UserID;

            _Test.TakeTest();

            MessageBox.Show("Test Completed.");
            this.Close();
        }

    }
}
