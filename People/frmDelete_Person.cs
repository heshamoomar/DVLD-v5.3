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

namespace DVLD_v1._0.People
{
    public partial class frmDelete_Person : Form
    {
        private int _PersonID = -1;

        public int PersonID
        {
            get { return _PersonID; }
        }

        public delegate void RefreshDataGridEventHandler(object sender, EventArgs e);

        public event RefreshDataGridEventHandler RefreshGrid;

        private void frmDelete_Person_FormClosed(object sender, FormClosedEventArgs e)
        {
            RefreshGrid?.Invoke(this, e);
        }

        public frmDelete_Person(int PersonID)
        {
            InitializeComponent();
            
            _PersonID = PersonID;

        }

        private void frmDelete_Person_Load(object sender, EventArgs e)
        {
            ctrlPersonCard1.LoadPersonInfo(PersonID);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (clsPerson.deletePerson(PersonID) == true)   //  if errors then remove check person integrity in delete function in clsPerson
            {
            MessageBox.Show("Person deleted successfully","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Cannot delete.\n Person with record ID = " + PersonID+" is tied to other tables in database.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
