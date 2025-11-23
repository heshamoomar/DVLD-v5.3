using Business_Layer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_v1._0.Applications
{
    public partial class frmUpdate_Application_Fee : Form
    {
        clsApplicationType _ApplicationType;

        public frmUpdate_Application_Fee(clsApplicationType _ApplicationType)
        {
            InitializeComponent();

            this._ApplicationType = _ApplicationType;

            lblID.Text = _ApplicationType.ApplicationTypeID.ToString();
            lblTitle.Text = _ApplicationType.ApplicationTypeTitle;
            textBox1.Text = _ApplicationType.ApplicationFees.ToString();

            btnUpdateFee.Focus();
        }

        public delegate void RefreshDataGridEventHandler(object sender, EventArgs e);

        public event RefreshDataGridEventHandler RefreshGrid;

        private void frmUpdate_Application_Fee_FormClosed(object sender, FormClosedEventArgs e)
        {
            RefreshGrid?.Invoke(this, e);
        }

        private void frmUpdate_Application_Fee_Load(object sender, EventArgs e)
        {
            this.ActiveControl = btnUpdateFee;
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Please enter Fee amount to update first");
                return;
            }

            DialogResult dr = MessageBox.Show("Are you sure?","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                 _ApplicationType.ApplicationFees = decimal.Parse(textBox1.Text);

                _ApplicationType.UpdateApplicationType();

                 this.Close();
            }

            else if (dr == DialogResult.No)
            {

            }

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits, backspace, and one decimal point
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Allow only one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains("."))
            {
                e.Handled = true;
            }
        }
    }
}
