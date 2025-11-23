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

namespace DVLD_v1._0.Tests
{
    public partial class frmUpdate_Test_Fee : Form
    {
        clsTestsBusiness _TestType;

        public frmUpdate_Test_Fee(clsTestsBusiness _TestType)
        {
            InitializeComponent();

            this._TestType = _TestType;

            lblID.Text = _TestType.TestTypeID.ToString();
            lblTitle.Text = _TestType.TestTypeTitle;
            textBox1.Text = _TestType.TestTypeFees.ToString();

            btnUpdateFee.Focus();
        }

        public delegate void RefreshDataGridEventHandler(object sender, EventArgs e);

        public event RefreshDataGridEventHandler RefreshGrid;


        private void frmUpdate_Test_Fee_Load(object sender, EventArgs e)
        {
            this.ActiveControl = btnUpdateFee;

        }

        private void frmUpdate_Test_Fee_FormClosed(object sender, FormClosedEventArgs e)
        {
            RefreshGrid?.Invoke(this, e);
        }

        private void btnUpdateFee_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Please enter Fee amount to update first");
                return;
            }

            DialogResult dr = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                _TestType.TestTypeFees = decimal.Parse(textBox1.Text);

                _TestType.UpdateTestType();

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
