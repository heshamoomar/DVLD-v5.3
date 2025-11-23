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

namespace DVLD_v1._0.Applications
{
    public partial class frmManage_Application_Types : Form
    {
        clsApplicationType _Application = new clsApplicationType();

        public frmManage_Application_Types()
        {
            InitializeComponent();
        }

        private void RefreshGrid(DataTable dt)
        {
            string noRecords = dt.Rows.Count.ToString();
            lblRecordsCount.Text = noRecords;
        }

        private void frmManage_Application_Types_Load(object sender, EventArgs e)
        {
            DataTable _dtApplicationsTypes = clsApplicationType.GetApplicationsTypes();

            dataGridView1.DataSource = _dtApplicationsTypes;

            string noRecords = _dtApplicationsTypes.Rows.Count.ToString();
            lblRecordsCount.Text = noRecords;

            RefreshGrid(_dtApplicationsTypes);

        }

        private void updateFeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;

            _Application.ApplicationTypeID = Int32.Parse(dataGridView1[0, currentRowIndex].Value.ToString());
            _Application.ApplicationTypeTitle = dataGridView1[1, currentRowIndex].Value.ToString();
            _Application.ApplicationFees = decimal.Parse(dataGridView1[2, currentRowIndex].Value.ToString());



            frmUpdate_Application_Fee frm = new frmUpdate_Application_Fee(_Application);
            frm.RefreshGrid += frmManage_Application_Types_Load;
            frm.ShowDialog();
        }
    }
}
