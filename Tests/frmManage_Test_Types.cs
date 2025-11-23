using Business_Layer;
using DVLD_v1._0.Applications;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_v1._0.Tests
{
    public partial class frmManage_Test_Types : Form
    {
        clsTestsBusiness _Test = new clsTestsBusiness();

        public frmManage_Test_Types()
        {
            InitializeComponent();
        }

        private void RefreshGrid(DataTable dt)
        {
            string noRecords = dt.Rows.Count.ToString();
            lblRecordsCount.Text = noRecords;
        }

        private void frmManage_Test_Types_Load(object sender, EventArgs e)
        {
            DataTable _dtTestsTypes = clsTestsBusiness.GetTestsTypes();

            dataGridView1.DataSource = _dtTestsTypes;

            string noRecords = _dtTestsTypes.Rows.Count.ToString();
            lblRecordsCount.Text = noRecords;

            RefreshGrid(_dtTestsTypes);
        }

        private void updateFeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;

            _Test.TestTypeID = Int32.Parse(dataGridView1[0, currentRowIndex].Value.ToString());
            _Test.TestTypeTitle = dataGridView1[1, currentRowIndex].Value.ToString();
            _Test.TestTypeDescription = dataGridView1[2, currentRowIndex].Value.ToString();
            _Test.TestTypeFees = decimal.Parse(dataGridView1[3, currentRowIndex].Value.ToString());



            frmUpdate_Test_Fee frm = new frmUpdate_Test_Fee(_Test);
            frm.RefreshGrid += frmManage_Test_Types_Load;
            frm.ShowDialog();

        }
    }
}
