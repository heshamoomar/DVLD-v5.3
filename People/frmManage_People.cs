using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Business_Layer;
using DVLD_v1._0.People;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD_v1._0
{
    public partial class frmManage_People : Form
    {
        private DataTable _dtPeople;
        private string _filter_by = "";

        public frmManage_People()
        {
            InitializeComponent();
        }

        private void RefreshRecordsCount(DataTable dt)
        {
            string noRecords= dt.Rows.Count.ToString();
            lblRecordsCount.Text = noRecords;
        }

        private void frmManage_People_Load(object sender, EventArgs e)
        {
            _dtPeople = clsPerson.GetPeopleDatatable();
            dataGridView1.DataSource = _dtPeople;

            RefreshRecordsCount(_dtPeople);

            cbFilters.SelectedIndex = 0;
            tbFilter.Hide();

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private DataView getFilterResults(string filterBy, string text)
        {

            DataView view = new DataView(_dtPeople);

            if (cbFilters.SelectedIndex == 0)   //  None
                return view;

            if (cbFilters.SelectedIndex == 0 || cbFilters.SelectedIndex == 1 || cbFilters.SelectedIndex == 9)    // Person ID or National No. or Phone
            {
                string select1 = "CONVERT(" + filterBy + ", 'System.String') LIKE '" + text + "%'";


                view.RowFilter = select1;

                return view;
            }

            string select = filterBy + " LIKE '" + text + "%'";

            view.RowFilter = select;


            return view;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            switch (cbFilters.Text)
            {
                case "None":
                    _filter_by = "";
                    tbFilter.Hide();
                    _dtPeople = clsPerson.GetPeopleDatatable();
                    dataGridView1.DataSource = _dtPeople;
                    break;

                case "Person ID":
                    tbFilter.Show();
                    _filter_by = "PersonID";
                    break;

                case "National No.":
                    tbFilter.Show();
                    _filter_by = "NationalNo";
                    break;

                case "First Name":
                    tbFilter.Show();
                    _filter_by = "FirstName";
                    break;

                case "Second Name":
                    tbFilter.Show();
                    _filter_by = "SecondName";
                    break;

                case "Third Name":
                    tbFilter.Show();
                    _filter_by = "ThirdName";
                    break;

                case "Last Name":
                    tbFilter.Show();
                    _filter_by = "LastName";
                    break;

                case "Nationality":
                    tbFilter.Show();
                    _filter_by = "Nationality";
                    break;

                case "Gender":
                    tbFilter.Show();
                    _filter_by = "Gender";
                    break;

                case "Phone":
                    tbFilter.Show();
                    _filter_by = "Phone";
                    break;

                case "Email":
                    tbFilter.Show();
                    _filter_by = "Email";
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
                _dtPeople = clsPerson.GetPeopleDatatable();
                dataGridView1.DataSource = _dtPeople;
                return;
            }
            else
            {
                DataView view = getFilterResults(_filter_by, tbFilter.Text);
                dataGridView1.DataSource = view;
            }
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAdd_Update_Person frm = new frmAdd_Update_Person();

            frm.RefreshGrid += frmManage_People_Load; //  subscribe to this function before going to the Add_Update_Person form

            frm.ShowDialog();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID;

            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
            Int32.TryParse(dataGridView1[0, currentRowIndex].Value.ToString(), out PersonID);

            frmAdd_Update_Person frm = new frmAdd_Update_Person(PersonID);  //  open form in update mode (using the second constructor)

            frm.RefreshGrid += frmManage_People_Load; //  subscribe to this function before going to the Add_Update_Person form

            frm.ShowDialog();

        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdd_Update_Person frm = new frmAdd_Update_Person();

            frm.RefreshGrid += frmManage_People_Load; //  subscribe to this function before going to the Add_Update_Person form

            frm.ShowDialog();

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID;

            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
            Int32.TryParse(dataGridView1[0, currentRowIndex].Value.ToString(), out PersonID);

            frmDelete_Person frm = new frmDelete_Person(PersonID);

            frm.RefreshGrid += frmManage_People_Load; //  subscribe to this function before going to the Add_Update_Person form

            frm.ShowDialog();

        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID;

            int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
            Int32.TryParse(dataGridView1[0, currentRowIndex].Value.ToString(), out PersonID);

            frmDisplayPerson frm = new frmDisplayPerson(PersonID);
            frm.ShowDialog();
        }
    }
}
