using Business_Layer;
using DVLD_v1._0.Tests;
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
    public partial class frmManage_Local_License_Applications : Form
    {
        private DataTable _dtLicenses;
        private string _filter_by = "";

        public frmManage_Local_License_Applications()
        {
            InitializeComponent();

            _dtLicenses = clsLocal_License.GetLocalLicensesInfo();
            dataGridView1.DataSource = _dtLicenses;

            RefreshRecordsCount(_dtLicenses);

            cbFilters.SelectedIndex = 0;
            tbFilter.Hide();

        }

        private void RefreshRecordsCount(DataTable dt)
        {
            string noRecords = dt.Rows.Count.ToString();
            lblRecordsCount.Text = noRecords;
        }

        private void frmManage_Local_License_Applications_Load(object sender, EventArgs e)
        {
            _dtLicenses = clsLocal_License.GetLocalLicensesInfo();
            dataGridView1.DataSource = _dtLicenses;

            RefreshRecordsCount(_dtLicenses);
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmLocal_License_Application frm = new frmLocal_License_Application();

            frm.RefreshGrid += frmManage_Local_License_Applications_Load; //  subscribe to this function before going to the Add_Update_Person form

            frm.ShowDialog();

        }

        private DataView getFilterResults(string filterBy, string text)
        {

            DataView view = new DataView(_dtLicenses);

            if (cbFilters.SelectedIndex == 0)   //  None
                return view;

            if (cbFilters.SelectedIndex == 1 || cbFilters.SelectedIndex == 3 || cbFilters.SelectedIndex == 5|| cbFilters.SelectedIndex == 6)
            {
                string select1 = "CONVERT(" + filterBy + ", 'System.String') LIKE '" + text + "%'";


                view.RowFilter = select1;

                return view;
            }

            string select = filterBy + " LIKE '" + text + "%'";

            view.RowFilter = select;


            return view;
        }

        private void cbFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbFilters.Text)
            {
                case "None":
                    _filter_by = "";
                    tbFilter.Hide();
                    _dtLicenses = clsLocal_License.GetLocalLicensesInfo();
                    dataGridView1.DataSource = _dtLicenses;
                    break;

                case "L.D.L. AppID":
                    tbFilter.Show();
                    _filter_by = "LocalDrivingLicenseApplicationID";
                    break;

                case "Class Name":
                    tbFilter.Show();
                    _filter_by = "ClassName";
                    break;

                case "National No.":
                    tbFilter.Show();
                    _filter_by = "NationalNo";
                    break;

                case "Full Name":
                    tbFilter.Show();
                    _filter_by = "FullName";
                    break;

                case "Application Date":
                    tbFilter.Show();
                    _filter_by = "ApplicationDate";
                    break;

                case "Passed Tests":
                    tbFilter.Show();
                    _filter_by = "PassedTests";
                    break;

                case "Status":
                    tbFilter.Show();
                    _filter_by = "Status";
                    break;

                default:
                    break;
            }

            DataView view = getFilterResults(_filter_by, tbFilter.Text);
            dataGridView1.DataSource = view;

        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            if (tbFilter.Text == "")
            {
                _dtLicenses = clsLocal_License.GetLocalLicensesInfo();
                dataGridView1.DataSource = _dtLicenses;
                return;
            }
            else
            {
                DataView view = getFilterResults(_filter_by, tbFilter.Text);
                dataGridView1.DataSource = view;
            }

        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to cancel this new Application?", 
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.No)
            {
                return;
            }

            int LDL_APPID;

            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
            Int32.TryParse(dataGridView1[0, currentRowIndex].Value.ToString(), out LDL_APPID);

            clsLocal_License _License = clsLocal_License.Find(LDL_APPID);

            int ApplicationID = _License.ApplicationID;

            clsLicense_Application _Application = clsLicense_Application.Find(ApplicationID);

            _Application.ApplicationStatus = 2; //  Cancelled
            _Application.LastStatusDate = DateTime.Now;

            _Application.UpdateApplication();


            _dtLicenses = clsLocal_License.GetLocalLicensesInfo();
            dataGridView1.DataSource = _dtLicenses;

            RefreshRecordsCount(_dtLicenses);

        }

        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  pass the _dtIntLicenses datatable and the row I want to work on to the vision test form

            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
            frmSchedule_Vision_Test frm = new frmSchedule_Vision_Test(_dtLicenses, currentRowIndex, 1);
            frm.RefreshGrid += frmManage_Local_License_Applications_Load;
            frm.ShowDialog();
        }

        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
            frmSchedule_Vision_Test frm = new frmSchedule_Vision_Test(_dtLicenses, currentRowIndex, 2);
            frm.RefreshGrid += frmManage_Local_License_Applications_Load;
            frm.ShowDialog();

        }

        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
            frmSchedule_Vision_Test frm = new frmSchedule_Vision_Test(_dtLicenses, currentRowIndex, 3);
            frm.RefreshGrid += frmManage_Local_License_Applications_Load;
            frm.ShowDialog();

        }


        private void Refresh_ScheduleTests_MenuItem()
        {
            int Passed_tests;

            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
            Int32.TryParse(dataGridView1[5, currentRowIndex].Value.ToString(), out Passed_tests);

            ToolStripMenuItem scheduleTests = null;
            ToolStripMenuItem DeleteApplication = null;
            ToolStripMenuItem IssueDrivingLicense = null;

            foreach (ToolStripItem item in contextMenuStrip1.Items)
            {
                if (item is ToolStripMenuItem && item.Text == "Schedule Tests")
                {
                    scheduleTests = (ToolStripMenuItem)item;
                }
                if (item is ToolStripMenuItem && item.Text == "Delete Application")
                {
                    DeleteApplication = (ToolStripMenuItem)item;
                }
                if (item is ToolStripMenuItem && item.Text == "Issue Driving License (First Time)")
                {
                    IssueDrivingLicense = (ToolStripMenuItem)item;
                }
            }

            ToolStripMenuItem vision_test = (ToolStripMenuItem)scheduleTests.DropDownItems[0];
            ToolStripMenuItem written_test = (ToolStripMenuItem)scheduleTests.DropDownItems[1];
            ToolStripMenuItem street_test = (ToolStripMenuItem)scheduleTests.DropDownItems[2];

            if (Passed_tests == 0)
            {
                scheduleTests.Enabled = true;

                vision_test.Enabled = true;
                written_test.Enabled = false;
                street_test.Enabled = false;

                DeleteApplication.Enabled = true;
                IssueDrivingLicense.Enabled = false;
            }

            else if (Passed_tests == 1)
            {
                scheduleTests.Enabled = true;

                vision_test.Enabled = false;
                written_test.Enabled = true;
                street_test.Enabled = false;

                DeleteApplication.Enabled = false;
                IssueDrivingLicense.Enabled = false;
            }

            else if (Passed_tests == 2)
            {
                scheduleTests.Enabled = true;

                vision_test.Enabled = false;
                written_test.Enabled = false;
                street_test.Enabled = true;

                DeleteApplication.Enabled = false;
                IssueDrivingLicense.Enabled = false;
            }

            else if(Passed_tests == 3)
            {
                scheduleTests.Enabled = false;  //  passed all 3 tests

                DeleteApplication.Enabled = false;
                IssueDrivingLicense.Enabled = true;
            }

        }
        private void Refresh_CancelApplication_MenuItem()

        {
            string Status;

            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
            Status = dataGridView1[6, currentRowIndex].Value.ToString();


            ToolStripMenuItem cancelApplication = null;
            ToolStripMenuItem scheduleTests = null;
            ToolStripMenuItem ShowLicenseHistory = null;


            foreach (ToolStripItem item in contextMenuStrip1.Items)
            {
                if (item is ToolStripMenuItem && item.Text == "Cancel Application")
                {
                    cancelApplication = (ToolStripMenuItem)item;
                }
                if (item is ToolStripMenuItem && item.Text == "Schedule Tests")
                {
                    scheduleTests = (ToolStripMenuItem)item;
                }
                if (item is ToolStripMenuItem && item.Text == "Show Person License History")
                {
                    ShowLicenseHistory = (ToolStripMenuItem)item;
                }
            }

            if (Status == "New")
            {
                cancelApplication.Enabled = true;
            }

            else if (Status == "Cancelled" || Status == "Completed")
            {
                cancelApplication.Enabled = false;
                scheduleTests.Enabled = false;
            }

            if(Status == "Completed")
            {
                ShowLicenseHistory.Enabled = true;
            }
        }
        private void Refresh_IssueLicense_MenuItem()

        {
            string Status;

            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
            Status = dataGridView1[6, currentRowIndex].Value.ToString();


            ToolStripMenuItem IssueDrivingLicense = null;

            foreach (ToolStripItem item in contextMenuStrip1.Items)
            {
                if (item is ToolStripMenuItem && item.Text == "Issue Driving License (First Time)")
                {
                    IssueDrivingLicense = (ToolStripMenuItem)item;
                    break;
                }
            }

            if (Status == "Completed")
            {
                IssueDrivingLicense.Enabled = false;
            }


        }
        private void Refresh_ShowLicense_MenuItem()

        {
            int LDLAppID;

            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
            Int32.TryParse(dataGridView1[0, currentRowIndex].Value.ToString(), out LDLAppID);

            clsLocal_License _LocalLicenseApplication;
            _LocalLicenseApplication = clsLocal_License.Find(LDLAppID);
            clsLicense_Application _Application = new clsLicense_Application();
            _Application = clsLicense_Application.Find(_LocalLicenseApplication.ApplicationID);

            if (_Application == null)
            {
                return;
            }

            clsIssuedLicenses _licence = clsIssuedLicenses.FindByApplicationID(_Application.ApplicationID);

            if (_Application == null)   // no license found in license table
            {
                return;
            }

            ToolStripMenuItem ShowLicense = null;

            foreach (ToolStripItem item in contextMenuStrip1.Items)
            {
                if (item is ToolStripMenuItem && item.Text == "Show License")
                {
                    ShowLicense = (ToolStripMenuItem)item;
                    break;
                }
            }

            if (_licence != null)  // this application has a license
            {
                ShowLicense.Enabled = true;
            }

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            Refresh_ScheduleTests_MenuItem();
            Refresh_CancelApplication_MenuItem();
            Refresh_IssueLicense_MenuItem();
            Refresh_ShowLicense_MenuItem();
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LDL_APPID;

            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
            Int32.TryParse(dataGridView1[0, currentRowIndex].Value.ToString(), out LDL_APPID);

            clsLocal_License _LocalLicenseApplication = clsLocal_License.Find(LDL_APPID);

            clsLicense_Application _Application = clsLicense_Application.Find(_LocalLicenseApplication.ApplicationID);

            if (clsIssuedLicenses.ActiveLicenseExistsForApplication(_Application.ApplicationID))  // a license already exits and active for this application 
                return;

            frmIssue_Driver_License_First_Time frm = new frmIssue_Driver_License_First_Time(_dtLicenses, currentRowIndex);
            frm.RefreshGrid += frmManage_Local_License_Applications_Load;
            frm.ShowDialog();

        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            int LDL_APPID;

            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
            Int32.TryParse(dataGridView1[0, currentRowIndex].Value.ToString(), out LDL_APPID);

            frmShowLicense frm = new frmShowLicense(LDL_APPID);
            frm.ShowDialog();
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LDL_APPID;
            
            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
            Int32.TryParse(dataGridView1[0, currentRowIndex].Value.ToString(), out LDL_APPID);


            DialogResult dr = MessageBox.Show("Do you want to delete this application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
            {
                return;
            }

            clsLocal_License _Local_License = clsLocal_License.Find(LDL_APPID);
            
            clsLicense_Application _Application = clsLicense_Application.Find(_Local_License.ApplicationID);
            
            clsLocal_License.deleteApplication(_Local_License.LocalDrivingLicenseApplicationID);
            
            clsLicense_Application.deleteApplication(_Application.ApplicationID);
            
            _dtLicenses = clsLocal_License.GetLocalLicensesInfo();
            dataGridView1.DataSource = _dtLicenses;
            
            RefreshRecordsCount(_dtLicenses);
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LDL_APPID;
            string NationalNo;

            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
            Int32.TryParse(dataGridView1[0, currentRowIndex].Value.ToString(), out LDL_APPID);
            NationalNo = dataGridView1[2, currentRowIndex].Value.ToString();

            frmShow_Issued_Licenses frm = new frmShow_Issued_Licenses(LDL_APPID, NationalNo);
            frm.ShowDialog();
        }
    }


}
