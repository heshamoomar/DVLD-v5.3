using Business_Layer;
using DVLD_v1._0.Licenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_v1._0.Drivers
{
    public partial class frmDisplay_Drivers : Form
    {
        private DataTable _dtDrivers;
        private string _filter_by = "";

        public frmDisplay_Drivers()
        {
            InitializeComponent();
        }

        private void RefreshRecordsCount(DataTable dt)
        {
            string noRecords = dt.Rows.Count.ToString();
            lblRecordsCount.Text = noRecords;
        }

        private void frmDisplay_Drivers_Load(object sender, EventArgs e)
        {
            _dtDrivers = clsDriver.GetDriversDatatable();
            dataGridView1.DataSource = _dtDrivers;

            RefreshRecordsCount(_dtDrivers);

            cbFilters.SelectedIndex = 0;
            tbFilter.Hide();

        }

        private DataView getFilterResults(string filterBy, string text)
        {

            DataView view = new DataView(_dtDrivers);

            if (cbFilters.SelectedIndex == 0)   //  None
                return view;

            if ( cbFilters.SelectedIndex == 0 || cbFilters.SelectedIndex == 1 ||
                 cbFilters.SelectedIndex == 2 || cbFilters.SelectedIndex == 3 )
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
                    _dtDrivers = clsDriver.GetDriversDatatable();
                    dataGridView1.DataSource = _dtDrivers;
                    break;

                case "Driver ID":
                    tbFilter.Show();
                    _filter_by = "DriverID";
                    break;

                case "Person ID":
                    tbFilter.Show();
                    _filter_by = "PersonID";
                    break;

                case "National No.":
                    tbFilter.Show();
                    _filter_by = "NationalNo";
                    break;

                case "Full Name":
                    tbFilter.Show();
                    _filter_by = "FullName";
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
                _dtDrivers = clsDriver.GetDriversDatatable();
                dataGridView1.DataSource = _dtDrivers;
                return;
            }
            else
            {
                DataView view = getFilterResults(_filter_by, tbFilter.Text);
                dataGridView1.DataSource = view;
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showLatestIssuedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID;
            string NationalNo;

            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
            Int32.TryParse(dataGridView1[0, currentRowIndex].Value.ToString(), out DriverID);
            NationalNo = dataGridView1[2, currentRowIndex].Value.ToString();

            frmShow_Issued_Licenses frm = new frmShow_Issued_Licenses(DriverID, true, NationalNo);
            frm.ShowDialog();
        }

        private void showLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID;
            string NationalNo;

            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
            Int32.TryParse(dataGridView1[0, currentRowIndex].Value.ToString(), out DriverID);
            NationalNo = dataGridView1[2, currentRowIndex].Value.ToString();

            frmShow_Issued_Licenses frm = new frmShow_Issued_Licenses(DriverID, true, true, NationalNo);
            frm.ShowDialog();
        }
    }
}
