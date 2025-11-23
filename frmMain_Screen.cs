using DVLD_v1._0.Applications;
using DVLD_v1._0.Drivers;
using DVLD_v1._0.International_Licenses;
using DVLD_v1._0.Licenses;
using DVLD_v1._0.Tests;
using DVLD_v1._0.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_v1._0
{
    public partial class frmMain_Screen : Form
    {

        public frmMain_Screen(string loggedUser)
        {
            InitializeComponent();
            lblLoggedUser.Text += loggedUser;

        }

        private void reToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManage_People frmManage_People = new frmManage_People();
            frmManage_People.ShowDialog();
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
            frmLogin_Screen frmLogin_Screen = new frmLogin_Screen();
            frmLogin_Screen.ShowDialog();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManage_Users frmManage_Users = new frmManage_Users();
            frmManage_Users.ShowDialog();
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = clsGlobalSettings._loggedUser.UserID;

            frmDisplayUser frmDisplayUsers = new frmDisplayUser(UserID);

            frmDisplayUsers.ShowDialog();

        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChange_Password___IsActive frm = new frmChange_Password___IsActive();
            frm.ShowDialog();
        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManage_Application_Types frm= new frmManage_Application_Types();
            frm.ShowDialog();
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManage_Test_Types frm= new frmManage_Test_Types();
            frm.ShowDialog();
        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLocal_License_Application frm= new frmLocal_License_Application();
            frm.ShowDialog();
        }

        private void listApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void localDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManage_Local_License_Applications frm = new frmManage_Local_License_Applications();
            frm.ShowDialog();
        }

        private void internationalDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManage_International_License_Applications frm = new frmManage_International_License_Applications();
            frm.ShowDialog();
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDisplay_Drivers frm= new frmDisplay_Drivers();
            frm.ShowDialog();
        }

        private void internationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmInternational_License_Application frm = new frmInternational_License_Application();
            frm.ShowDialog();
        }

        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRenew_License frm = new frmRenew_License();
            frm.ShowDialog();
        }

        private void lostDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLost_or_Damaged_License frm = new frmLost_or_Damaged_License();
            frm.ShowDialog();
        }
    }
}
