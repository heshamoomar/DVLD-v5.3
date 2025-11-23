using Business_Layer;
using DVLD_v1._0.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_v1._0.Controls
{
    public partial class ctrlPersonCard : UserControl
    {
        private clsPerson _Person;

        private int _PersonID = -1;

        public int PersonID
        {
            get { return _PersonID; }
        }

        public ctrlPersonCard()
        {
            InitializeComponent();
        }

        private void ctrlPersonCard_Load(object sender, EventArgs e)
        {

        }

        private void _FillPersonInfo()
        {
            _PersonID = _Person.PersonID;
            lblPersonID.Text = _Person.PersonID.ToString();
            lblNationalNo.Text = _Person.NationalNo.ToString();
            lblName.Text = _Person.FirstName +" "+ _Person.LastName;
            lblEmail.Text = _Person.Email;
            lblPhone.Text = _Person.Phone;
            lblDataOfBirth.Text = _Person.DateOfBirth.ToShortDateString();
            lblCountry.Text = _Person.Nationality;
            lblAdderss.Text = _Person.Address;
            lblGender.Text = _Person.Gender;

            if(_Person.Gender.StartsWith("M"))
            {
                pictureBox1.Image = Resources.man__1_;
            }
            else
            {
                pictureBox1.Image = Resources.woman__1_;
            }

            if (_Person.ImagePath != "") 
            {
                if (File.Exists(_Person.ImagePath))
                    pictureBox1.Load(_Person.ImagePath);
            }
        }

        public void ClearPersonInfo()
        {
            _Person = null;

            _PersonID = -1;
            lblPersonID.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblName.Text = "[????]";
            lblEmail.Text = "[????]";
            lblPhone.Text = "[????]";
            lblDataOfBirth.Text = "[????]";
            lblCountry.Text = "[????]";
            lblAdderss.Text = "[????]";
            lblGender.Text = "[????]";

            pictureBox1.Image = Resources.man__1_;
        }

        public void LoadPersonInfo(int PersonID)
        {
            _Person = clsPerson.Find(PersonID);
            if (_Person == null)
            {
                //_ResetPersonInfo();
                //MessageBox.Show("No Person with PersonID = " + PersonID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _FillPersonInfo();
            }
        }

        public void LoadPersonInfo(string NationalNo)
        {
            _Person = clsPerson.Find(NationalNo);
            if (_Person == null)
            {
                //_ResetPersonInfo();
                //MessageBox.Show("No Person with National No. = " + NationalNo.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _FillPersonInfo();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
