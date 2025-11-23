using Business_Layer;
using DVLD_v1._0.Controls;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_v1._0.Users
{
    public partial class frmChange_Password___IsActive : Form
    {
        private clsPerson _Person = clsGlobalSettings._loggedPerson;
        private clsUser _User = clsGlobalSettings._loggedUser;

        public frmChange_Password___IsActive()
        {
            InitializeComponent();

            ctrlUserCard1.LoadUserInfo(_User.UserID);

            if (_User.IsActive)
            {
                checkBoxIsActive.Checked = true;
            }
            else
            {
                checkBoxIsActive.Checked = false;
            }

            btnUpdatePassword.Focus();

        }

        private void frmChange_Password___IsActive_Load(object sender, EventArgs e)
        {

        }

        ErrorProvider ErrorProvider1 = new ErrorProvider();
        ErrorProvider ErrorProvider2 = new ErrorProvider();
        ErrorProvider ErrorProvider3 = new ErrorProvider();
        private void tbOldPassword_Validating(object sender, CancelEventArgs e)
        {
            

        }

        private void tbNewPassword1_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbNewPassword1.Text))
            {
                ErrorProvider1.SetError(tbNewPassword1, "This field is required.");

                e.Cancel = true;
            }

            else if (tbNewPassword1.Text.Length <= 4)
            {
                ErrorProvider1.SetError(tbNewPassword1, "Password cannot be less than 4 characters.");

                e.Cancel = true;

            }
            else
            {
                ErrorProvider1.SetError(tbNewPassword1, string.Empty);
            }

        }

        private void tbNewPassword2_Validating(object sender, CancelEventArgs e)
        {
            if (tbNewPassword2.Text != tbNewPassword2.Text)
            {
                ErrorProvider2.SetError(tbNewPassword2, "Passwords do not match.");

                e.Cancel = true;

            }
            else
            {
                ErrorProvider2.SetError(tbNewPassword2, string.Empty);
            }

        }

        private void btnUpdatePassword_Click(object sender, EventArgs e)
        {
            if(tbOldPassword.Text.IsNullOrEmpty()||tbNewPassword1.Text.IsNullOrEmpty()|| tbNewPassword2.Text.IsNullOrEmpty())
            {
                return;
            }

            if (clsGlobalSettings.ComputeHash(tbOldPassword.Text) != _User.Password)
            {
                MessageBox.Show("Old password you provided does not match password for this user.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (tbNewPassword1.Text != tbNewPassword2.Text)
            {
                MessageBox.Show("Passwords do not match.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            _User.Password = tbNewPassword1.Text;


            if (checkBoxIsActive.Checked)
            {
                _User.IsActive = true;
            }
            else
            {
                _User.IsActive = false;
            }

            if (_User.UpdateUser())
            {
                MessageBox.Show("Password updated.");
                this.Close();
            }

        }
    }
}
