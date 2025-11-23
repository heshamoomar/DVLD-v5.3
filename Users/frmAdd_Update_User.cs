using Business_Layer;
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
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD_v1._0.Users
{
    public partial class frmAdd_Update_User : Form
    {

        public delegate void RefreshDataGridEventHandler(object sender, EventArgs e);

        public event RefreshDataGridEventHandler RefreshGrid;
        private void frmAdd_Update_User_FormClosed(object sender, FormClosedEventArgs e)
        {
            RefreshGrid?.Invoke(this, e);
        }


        private string _search_by = "";
        clsPerson _Person;

        // you need to call the default constructor first because you want to load
        // info to the _User object (PersonID) before calling the AddNewUser() method which will then return an object
        clsUser _User = new clsUser();


        public frmAdd_Update_User()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            
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

        private void ctrlPersonCard1_Load(object sender, EventArgs e)
        {
            
        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            
        }

        //private clsPerson getPerson(string filterBy, string Filtertext)
        //{

        //    if (cbFilters.SelectedIndex == 0)   //  None

        //    if (cbFilters.SelectedIndex == 0 || cbFilters.SelectedIndex == 1)    // Person ID or National No.
        //    {
        //        string select1 = "CONVERT(" + filterBy + ", 'System.String') LIKE '" + Filtertext + "%'";


        //        view.RowFilter = select1;
        //    }

        //    string select = filterBy + " LIKE '" + Filtertext + "%'";

        //    view.RowFilter = select;


        //}


        private void button1_Click(object sender, EventArgs e)
        {
            if (tbFilter.Text.IsNullOrEmpty())
                return;

            if(_search_by== "PersonID")
            {
                _Person = clsPerson.Find(Int32.Parse(tbFilter.Text));

                if (_Person == null) // if the searched person doesn't exist, escape method
                    return;

                ctrlPersonCard1.LoadPersonInfo(_Person.PersonID);
                btnClearPerson.Visible = true;

                return;
            }
            else if (_search_by == "NationalNo") // if the searched person doesn't exist, escape method
            {           
                _Person = clsPerson.Find(tbFilter.Text);
                btnClearPerson.Visible = true;

                if (_Person == null)
                    return;

                ctrlPersonCard1.LoadPersonInfo(_Person.NationalNo);
                
                return;
            }

            else
            {
                btnClearPerson.Visible = false;
                return;
            }

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

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(tbUserName.Text.IsNullOrEmpty()||tbPassword1.Text.IsNullOrEmpty()||tbPassword2.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Please enter full user details to create.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (_Person != null && clsPerson.IsPersonUser(_Person.PersonID))
            {
                MessageBox.Show("This person is already a user.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            if(clsUser.UserNameExistsInDatabase(tbUserName.Text))
            {
                MessageBox.Show("UserName already assigned to a user,\nChoose another one.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (_Person == null) // user will be added but not linked to a person
            {
                DialogResult result = MessageBox.Show("Do you want to add this user?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // user will correspond to no row in people table
                    // in userDataAccess I made sure that PersonID of -1 will be translated to DBNull
                    _User.PersonID = -1;

                    _User.UserName = tbUserName.Text;
                    _User.Password = tbPassword1.Text;

                    if (checkBoxIsActive.Checked)
                    { _User.IsActive = true; }
                    else { _User.IsActive = false; }

                    int UserID = _User.AddNewUser();
                    MessageBox.Show("User added with userID = " + UserID + ".");
                    this.Close();
                }
                else
                {
                    // The user clicked "No"
                    // Do something else or cancel the action
                    MessageBox.Show("User not added.");
                }
            }
            else if (_Person != null) // user will be added and linked to an existing person
            {
                DialogResult result = MessageBox.Show("Do you want to make Person of ID = " + _Person.PersonID + " a user?"
                    , "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    //_User = new clsUser(); // you need to call the default constructor first because you want to load
                    //// info to the _User object (PersonID) before calling the AddNewUser() method which will then return an object

                    // user will be added and correspond to a person in people tabale after it comes back from the AddNewUser with full details (minus _User.PersonID)
                    _User.PersonID = _Person.PersonID;/////////////////////////////

                    _User.UserName = tbUserName.Text;
                    _User.Password = tbPassword1.Text;

                    if (checkBoxIsActive.Checked)
                    { _User.IsActive = true; }
                    else { _User.IsActive = false; }

                    int UserID = _User.AddNewUser();

                    _User.PersonID = _Person.PersonID;

                    MessageBox.Show("Person was made a user with userID = " + UserID + ".");
                    this.Close();
                }
                else
                {
                    // The user clicked "No"
                    // Do something else or cancel the action
                    MessageBox.Show("Person not made a user.");
                }

            }

            else
                return;

        }

        private void btnClearPerson_Click(object sender, EventArgs e)
        {
            _Person = null;

            ctrlPersonCard1.ClearPersonInfo();

            btnClearPerson.Visible = false;
        }

        ErrorProvider ErrorProvider1 = new ErrorProvider();
        ErrorProvider ErrorProvider2 = new ErrorProvider();
        private void tbPassword1_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbPassword1.Text))
            {
                ErrorProvider1.SetError(tbPassword1, "This field is required.");

                e.Cancel = true;
            }

            else if (tbPassword1.Text.Length <= 4)
            {
                ErrorProvider1.SetError(tbPassword1, "Password cannot be less than 4 characters.");

                e.Cancel = true;

            }
            else
            {
                ErrorProvider1.SetError(tbPassword1, string.Empty);
            }

        }
        private void tbPassword2_Validating(object sender, CancelEventArgs e)
        {
            if (tbPassword1.Text != tbPassword2.Text)
            {
                ErrorProvider2.SetError(tbPassword2, "Passwords do not match.");

                e.Cancel = true;

            }
            else
            {
                ErrorProvider2.SetError(tbPassword2, string.Empty);
            }

        }






        private void tbPassword1_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmAdd_Update_User_Load(object sender, EventArgs e)
        {

        }

        private void tbUserName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
