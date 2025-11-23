using Business_Layer;
using DVLD_v1._0.Properties;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Business_Layer.clsPerson;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace DVLD_v1._0.People
{
    public partial class frmAdd_Update_Person : Form
    {
        private bool _image_set = false;

        private enum enFormMode
        {
            update=1,
            addNew=2,
        }
        
        private enFormMode _form_mode = enFormMode.addNew;

        public delegate void RefreshDataGridEventHandler(object sender, EventArgs e);
        public delegate void SendBackNewPersonIDEventHandler(object sender, int PersonID);

        public event RefreshDataGridEventHandler RefreshGrid;
        public event SendBackNewPersonIDEventHandler DataBack;  // for new licenese applications forms

        private void frmAdd_Update_Person_FormClosed(object sender, FormClosedEventArgs e)
        {
            RefreshGrid?.Invoke(this, e);
        }

        public frmAdd_Update_Person()
        {
            InitializeComponent();
        }

        public frmAdd_Update_Person(int PersonID)   //  call update mode of the form
        {
            InitializeComponent();

            _form_mode = enFormMode.update;

            label1.Text = "Update Person";

            this.Text = "Update Person";

            clsPerson _Person = clsPerson.Find(PersonID);

            lblPersonID.Text = _Person.PersonID.ToString();

            tbFirstName.Text = _Person.FirstName;
            tbSecondName.Text = _Person.SecondName;
            tbThirdName.Text = _Person.ThirdName;
            tbLastName.Text = _Person.LastName;
            tbNationalNo.Text = _Person.NationalNo;
            dateTimePicker1.Value = _Person.DateOfBirth;

            _image_path = _Person.getImagePathForPerson();
            if (_image_path != "")
            {
                _image_set = true;
            }

            if (_Person.Gender.StartsWith("M"))
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }
            else
            {
                radioButton1.Checked = false;
                radioButton2.Checked = true;
            }

            tbPhone.Text = _Person.Phone;
            tbEmail.Text = _Person.Email;

            // TODO: This line of code loads data into the 'dVLDDataSet.Countries' table. You can move, or remove it, as needed.
            this.countriesTableAdapter.Fill(this.dVLDDataSet.Countries);

            cbCountry.SelectedIndex = _Person.NationalityCountryID - 1; // dropdown menus are 0 based
            tbAddress.Text = _Person.Address;

            if (_Person.ImagePath.IsNullOrEmpty())
            {
                _image_set = false;
                if (_Person.Gender.StartsWith("M"))
                    pictureBox1.Image = Resources.man__1_;
                else
                    pictureBox1.Image = Resources.woman__1_;
            }
            else
            {

            _image_set = true;
            linkLabel2.Show();

            // Load the selected image into the PictureBox
            pictureBox1.Image = new Bitmap(_Person.ImagePath);    // returns path

            }




        }

        private void frmAdd_Update_Person_Load(object sender, EventArgs e)
        {
            if (_form_mode == enFormMode.addNew)
            {
            // TODO: This line of code loads data into the 'dVLDDataSet.Countries' table. You can move, or remove it, as needed.
            this.countriesTableAdapter.Fill(this.dVLDDataSet.Countries);
            cbCountry.SelectedIndex = 50;
            }

            if (_form_mode == enFormMode.addNew)     //  check male radio button when adding a new person (by default no radio button is checked)
                radioButton1.Checked = true;

            dateTimePicker1.Validating += dateTimePicker1_Validating;

            btnAdd.Focus();
            this.ActiveControl = btnAdd;
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked && _image_set == false)
            {
                pictureBox1.Image = Resources.man__1_;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked && _image_set == false)
            {
                pictureBox1.Image = Resources.woman__1_;
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private string _image_path = "";

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Create an instance of OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Filter the file types, only allowing image files
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            openFileDialog.Title = "Select an Image";

            // Show the dialog and get the result
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _image_set = true;
                linkLabel2.Show();

                // Load the selected image into the PictureBox
                pictureBox1.Image = new Bitmap(openFileDialog.FileName);    // returns path
                //MessageBox.Show(openFileDialog.FileName);

                _image_path = openFileDialog.FileName;

                // Optionally, resize the PictureBox to fit the image
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void frmAdd_Update_Person_DataBack(object sender, string imagePath)
        {
            _image_set = true;
            linkLabel2.Show();

            // Load the selected image into the PictureBox
            pictureBox1.Image = new Bitmap(imagePath);

            _image_path = imagePath;

            // Optionally, resize the PictureBox to fit the image
            //pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmTakePhoto frm = new frmTakePhoto();
            frm.DataBack += frmAdd_Update_Person_DataBack;
            frm.ShowDialog();
        }

        private void tbFirstName_TextChanged(object sender, EventArgs e)
        {

        }

        ErrorProvider ErrorProvider1 = new ErrorProvider();
        ErrorProvider ErrorProvider2 = new ErrorProvider();
        ErrorProvider ErrorProvider3 = new ErrorProvider();
        ErrorProvider ErrorProvider4 = new ErrorProvider();
        ErrorProvider ErrorProvider5 = new ErrorProvider();
        ErrorProvider ErrorProvider6 = new ErrorProvider();
        ErrorProvider ErrorProvider7 = new ErrorProvider();

        private void tbFirstName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbFirstName.Text))
            {
                ErrorProvider1.SetError(tbFirstName, "This field is required");

                e.Cancel = true;
            }
            else
            {
                ErrorProvider1.SetError(tbFirstName, string.Empty);
            }
        }

        private void tbLastName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbLastName.Text))
            {
                ErrorProvider2.SetError(tbLastName, "This field is required");

                e.Cancel = true;
            }
            else
            {
                ErrorProvider2.SetError(tbLastName, string.Empty);
            }
        }

        private void tbNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbNationalNo.Text))
            {
                ErrorProvider3.SetError(tbNationalNo, "This field is required");

                e.Cancel = true;
            }
            else if (clsPerson.isPersonExists(tbNationalNo.Text) && _form_mode == enFormMode.addNew)
            {
                ErrorProvider3.SetError(tbNationalNo, "This National number already exists");

                e.Cancel = true;

            }
            else
            {
                ErrorProvider3.SetError(tbNationalNo, string.Empty);
            }


        }

        private void tbPhone_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbPhone.Text))
            {
                ErrorProvider4.SetError(tbPhone, "This field is required");

                e.Cancel = true;
            }
            else if (!int.TryParse(tbPhone.Text, out _))
            {
                ErrorProvider4.SetError(tbPhone, "Please enter a number");
                e.Cancel = true;
            }
            else
            {
                ErrorProvider4.SetError(tbPhone, string.Empty);
            }

        }

        private void tbDateOfBirth_Validated(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
        }

        private void dateTimePicker1_Validating(object sender, CancelEventArgs e)
        {
            DateTime selectedDate = dateTimePicker1.Value;
            DateTime today = DateTime.Today;

            // Calculate the date 18 years ago
            DateTime eighteenYearsAgo = today.AddYears(-18);

            // Check if the selected date is after the 18-years-ago date
            if (selectedDate > eighteenYearsAgo)
            {
                ErrorProvider5.SetError(dateTimePicker1, "A person has to be over 18 years old");
                e.Cancel = true; // Prevent moving to another control
            }
            else
            {
                ErrorProvider5.SetError(dateTimePicker1, string.Empty); // Clear the error
            }

        }

        private void tbEmail_Validating(object sender, CancelEventArgs e)
        {
            string email = tbEmail.Text;
            if (string.IsNullOrEmpty(email))
            {
                ErrorProvider6.SetError(tbEmail, string.Empty); // Clear the error
                return; //  only validate if use inputted
            }
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            // Check if the email format is valid
            if (!Regex.IsMatch(email, emailPattern))
            {
                ErrorProvider6.SetError(tbEmail, "Invalid email format");
                e.Cancel = true; // Prevents moving to another control
            }
            else
            {
                ErrorProvider6.SetError(tbEmail, string.Empty); // Clear the error
            }

        }

        private void tbAddress_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbAddress.Text))
            {
                ErrorProvider7.SetError(tbAddress, "This field is required");

                e.Cancel = true;
            }
            else
            {
                ErrorProvider7.SetError(tbAddress, string.Empty);
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (tbFirstName.Text.IsNullOrEmpty() || tbLastName.Text.IsNullOrEmpty() || tbNationalNo.Text.IsNullOrEmpty() ||
                tbPhone.Text.IsNullOrEmpty() || tbAddress.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Enter required person info first.", "Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            if (clsPerson.isPersonExists(tbNationalNo.Text) && _form_mode == enFormMode.addNew) // for repeated clicking on save button after initial save
            {
                MessageBox.Show("Person already added.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsPerson _Person = new clsPerson();

            _Person.FirstName = tbFirstName.Text;
            _Person.SecondName = tbSecondName.Text;
            _Person.ThirdName = tbThirdName.Text;
            _Person.LastName = tbLastName.Text;
            _Person.NationalNo = tbNationalNo.Text;
            _Person.DateOfBirth = dateTimePicker1.Value;

            if (radioButton1.Checked)
                _Person.Gender = "Male";
            else
                _Person.Gender = "Female";

            _Person.Phone = tbPhone.Text;
            _Person.Email = tbEmail.Text;
            _Person.NationalityCountryID = cbCountry.SelectedIndex + 1; // dropdown menus are 0 based
            _Person.Address = tbAddress.Text;
            _Person.ImagePath = _image_path;


            //int newPersonID = _User.Save();   // call the save function and let me know whether person added/updated successfully or not

            //if (newPersonID != -1)
            //{
            //    if(_Form_mode==enFormMode.addNew)
            //        MessageBox.Show("Person added successfully with ID = "+ newPersonID,"Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    else
            //        MessageBox.Show("Data updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}

            //else
            //    MessageBox.Show("Data is not saved successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //_User.Mode = enMode.Update;

            if(_form_mode == enFormMode.addNew)
            {
                int newPersonID = _Person.AddNewPerson();
                MessageBox.Show("Person added successfully with ID = " + newPersonID, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DataBack?.Invoke(this, newPersonID); // for new licenese applications forms
            }
            else
            {
                // I made this database function to load the
                // already existing image in database in case user didn't want to
                // update it (without this the image will be overwriten to "")
                // NOTE only use it when image path already "" otherwise it will keep overwriting 
                // your chosen image to "" , hence the if(_image_path=="")
                if (_image_path=="")
                    _image_path = _Person.getImagePathForPerson();


                _image_set = true;


                _Person.PersonID = Int32.Parse(lblPersonID.Text);   //  very important to upload user id from the form into the _User object before sending it to be updated; otherwise no update will happen
                _Person.UpdatePerson();
                MessageBox.Show("Data updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void cbCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (radioButton1.Checked)
            {
                pictureBox1.Image = Resources.man__1_;
                _image_set = false;
            }

            if (radioButton2.Checked)
            {
                pictureBox1.Image = Resources.woman__1_;
                _image_set = false;
            }
            linkLabel2.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

    }
}
