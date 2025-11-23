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

namespace DVLD_v1._0.Users
{
    public partial class frmManage_Users : Form
    {
        private DataTable _dtUsers;
        private string _filter_by = "";

        public frmManage_Users()
        {
            InitializeComponent();
        }

        private void RefreshRecordsCount(DataTable dt)
        {
            string noRecords = dt.Rows.Count.ToString();
            lblRecordsCount.Text = noRecords;
        }


        private void frmManage_Users_Load(object sender, EventArgs e)
        {
            _dtUsers = clsUser.GetUsersDatatable();
            dataGridView1.DataSource = _dtUsers;

            RefreshRecordsCount(_dtUsers);

            cbFilters.SelectedIndex = 0;
            tbFilter.Hide();

        }

        private DataView getFilterResults(string filterBy, string text)
        {

            DataView view = new DataView(_dtUsers);

            if (cbFilters.SelectedIndex == 0)   //  None
                return view;

            if (cbFilters.SelectedIndex == 1 || cbFilters.SelectedIndex == 2)   // UserID or PersonID
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
                    _dtUsers = clsUser.GetUsersDatatable();
                    dataGridView1.DataSource = _dtUsers;
                    break;

                case "UserID":
                    tbFilter.Show();
                    _filter_by = "UserID";
                    //MessageBox.Show(cbFilters.SelectedIndex.ToString());
                    break;

                case "PersonID":
                    tbFilter.Show();
                    _filter_by = "PersonID";
                    //MessageBox.Show(cbFilters.SelectedIndex.ToString());
                    break;

                case "FullName":
                    tbFilter.Show();
                    _filter_by = "FullName";
                    break;

                case "UserName":
                    tbFilter.Show();
                    _filter_by = "UserName";
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
                _dtUsers = clsUser.GetUsersDatatable();
                dataGridView1.DataSource = _dtUsers;
                return;
            }
            else
            {
                DataView view = getFilterResults(_filter_by, tbFilter.Text);
                dataGridView1.DataSource = view;
            }

        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAdd_Update_User frm = new frmAdd_Update_User();
            frm.RefreshGrid += frmManage_Users_Load;
            frm.ShowDialog();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID;

            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
            Int32.TryParse(dataGridView1[0, currentRowIndex].Value.ToString(), out UserID);

            frmDisplayUser frm = new frmDisplayUser(UserID);
            frm.ShowDialog();
        }

        private void addNewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdd_Update_User frm = new frmAdd_Update_User();
            frm.RefreshGrid += frmManage_Users_Load;
            frm.ShowDialog();


        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID;

            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
            Int32.TryParse(dataGridView1[0, currentRowIndex].Value.ToString(), out UserID);

            if (clsGlobalSettings._loggedUser.UserID == UserID)
            {
                MessageBox.Show("Cannot delete a logged-in user.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (clsUser.deleteUser(UserID) == true)   
            {
                DialogResult res = MessageBox.Show("Are you sure, do you want to delete this user?","Confirm",MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                MessageBox.Show("User deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _dtUsers = clsUser.GetUsersDatatable();
                dataGridView1.DataSource = _dtUsers;

                RefreshRecordsCount(_dtUsers);

                }

            }
            else
            {
                MessageBox.Show("Cannot delete.\n User with User ID = " + UserID + " it is tied to other tables in database.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }


        }
    }
}
