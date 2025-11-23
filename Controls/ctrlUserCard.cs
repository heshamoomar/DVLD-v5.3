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

namespace DVLD_v1._0.Controls
{
    public partial class ctrlUserCard : UserControl
    {
        //private clsPerson _Person;


        private int _PersonID = -1;

        public int PersonID
        {
            get { return _PersonID; }
        }

        private clsUser _User;

        private int _UserID = -1;

        public int UserID
        {
            get { return _UserID; }
        }


        public ctrlUserCard()
        {
            InitializeComponent();
        }

        private void ctrlUserCard_Load(object sender, EventArgs e)
        {

        }

        private void ctrlPersonCard1_Load(object sender, EventArgs e)
        {

        }

        public void LoadUserInfo(int UserID)
        {
            _User = clsUser.Find(UserID);
            if (_User == null)
            {


                //_ResetPersonInfo();
                MessageBox.Show("No User with User = " + UserID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // very important line 
                // make the user that will be loaded in form be the corresponding person from the user info you got
                _PersonID = _User.PersonID;

                _FillPersonInfo();
                _FillUserInfo();
            }

        }

        private void _FillUserInfo()
        {
            lblUserID.Text = _User.UserID.ToString();
            lblUsername.Text = _User.UserName.ToString();

            if (_User.IsActive == true)
            {
                lblIsActive.Text = "Yes";
            }
            else
            {
                lblIsActive.Text = "No";

            }

        }

        private void _FillPersonInfo()
        {
                ctrlPersonCard1.LoadPersonInfo(_PersonID);

        }

        private void lblUserID_Click(object sender, EventArgs e)
        {
            //lblUserID.Text = _User.UserID.ToString();
            //lblUsername.Text= _User.UserName;
            //if(_User.IsActive==true)
            //{
            //    lblIsActive.Text = "YES";
            //}
            //else
            //{
            //    lblIsActive.Text = "NO";

            //}
        }
    }
}
